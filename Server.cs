using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

using Newtonsoft.Json;


namespace HomeAutomationServer
{
    public class Server : IDisposable
    {
        //private readonly TcpListener _tcpListener;
        public bool Closing { get; set; }
        private readonly Form1 _form;
        private readonly IPAddress _ipAddress;
        private readonly int _dataPort;
        private readonly int _logPort;
        private TcpListener _dataListener;
        private TcpListener _logListener;
        private Thread _dataListenThread;
        private Thread _logListenThread;
       

        //private EndPoint _dataEndpoint;
        //private EndPoint _logEndPoint;

        public List<TcpClient> Connections;    

        public Server(Form1 form, string ipAddress, int dataPort, int logPort = 59400)
        {
            _form = form;
            _ipAddress = IPAddress.Parse(ipAddress);
            _dataPort = dataPort;
            _logPort = logPort;
            Closing = false;
        }
        private void Init()
        {
            _dataListener = new TcpListener(_ipAddress, _dataPort);
            _logListener = new TcpListener(_ipAddress, _logPort);

            _dataListenThread = new Thread(Listen);
            _logListenThread = new Thread(Listen);

            //_dataEndpoint = new IPEndPoint(_ipAddress, _dataPort);
            //_logEndPoint = new IPEndPoint(_ipAddress, _logPort);
    
            _form.PrintLogEntry("Server init complete\n");
        }
        public void Start()
        {
            if (_dataListener == null || _logListener == null) 
                Init();
            if (_dataListenThread != null) 
                _dataListenThread.Start(_dataListener);
            if (_logListenThread != null) 
                _logListenThread.Start(_logListener);

            _form.PrintLogEntry("Server start complete\n");
        }
        public void Stop()
        {
            Closing = true;
            _logListener.Server.Close();
            _dataListener.Server.Close();
        }
        private void Listen(object obj) // Thread that listens for incoming connections - when it receives one, it spins up a new thread to ParseClientData
        {
            var listener = (TcpListener)obj;
            listener.Start();
            while (!Closing)
            {
                var client = new TcpClient();
                try
                {
                    client = listener.AcceptTcpClient(); // add a timeout
                }
                catch (SocketException ex)
                {
                    Debug.Print("Socket Exception: " + ex.ErrorCode + " " + ex.Message);
                    continue;
                }
                finally
                {
                    if (client.Connected & !Closing)
                    {

                        var clientThread = new Thread(GetClientData);
                        _form.PrintLogEntry("Client connected: "+client.Client.LocalEndPoint+"\n");
                        clientThread.Start(client);
                    }
                }
                
            }
            listener.Stop();
        }

        private void GetClientData(object client)
        {

            var tcpClient = (TcpClient) client;
            
            while (true)
            {
                if (Closing) break;
                if (tcpClient.Available == 0) continue;

                _form.PrintLogEntry("There appears to be pending data sent from the client. Checking now. \n");

                int dataReceived;
                byte[] rawData;
                try
                {
                    rawData = ReceiveData(tcpClient.Client, out dataReceived);
                }
                catch
                {
                    break;
                }
                if (dataReceived == 0) break;
                
                ProcessClientData(rawData, dataReceived, tcpClient);
            }
            _form.PrintLogEntry("Closing TCP connection with " + tcpClient.Client.LocalEndPoint + "\n");
            tcpClient.Close();
        }      

        public byte[] ReceiveData(Socket socket, out int received, int timeout = 10000, int size = 4096, int offset = 0)
        {
            var message = new byte[size];
            received = 0;
            socket.ReceiveTimeout = timeout;
            do
            {
                try
                {
                    received += socket.Receive(message, offset + received, size - received, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    _form.PrintLogEntry("Receive timeout reached. Error code: " + ex.Message);
                    break;
                }

            } while (received < size & socket.Poll(100, SelectMode.SelectRead));
            socket.ReceiveTimeout = 0;

            return message;
        } 
        private void ProcessClientData(byte[] rawData, int bytesRead, TcpClient client)
        {
            
            var encoder = new UTF8Encoding();
            var decodedData = encoder.GetString(rawData, 0, bytesRead);
            var stream = client.GetStream();

            _form.PrintLogEntry("Received from: " + client.Client.LocalEndPoint + " : " + encoder.GetString(rawData, 0, bytesRead) + "\n");

            if (decodedData == "Hello?")
            {
                _form.PrintLogEntry("Responding to keep-alive from: " + client.Client.LocalEndPoint + "\n");
                var sendBuffer = encoder.GetBytes("Hello!");
                stream.Write(sendBuffer, 0, sendBuffer.Length);
            }

        }


        public void Dispose()
        {
            Stop();
            
        }
    }

    
}