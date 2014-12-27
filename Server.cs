using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

using Newtonsoft.Json.Serialization;


namespace HomeAutomationServer
{
    public enum ClientCommands : byte
    {
        KeepAliveResponse = 0x00,
        GetCurrentTemp = 0x02,
        SwitchToCelcius = 0x03,
        SwitchToFahrenheit = 0x04,
        StartMotionNotifications = 0x05,
        StopMotionNotifications = 0x06

    }
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
       
        public readonly Dictionary<TcpClient,Dictionary<string,byte>> ConnectionTable;    

        public Server(Form1 form, string ipAddress, int dataPort, int logPort = 59400)
        {
            _form = form;
            _ipAddress = IPAddress.Parse(ipAddress);
            _dataPort = dataPort;
            _logPort = logPort;

            Closing = false;
            ConnectionTable = new Dictionary<TcpClient, Dictionary<string,byte>>();
        }
        private void Init()
        {
            _dataListener = new TcpListener(_ipAddress, _dataPort);
            _logListener = new TcpListener(_ipAddress, _logPort);

            _dataListenThread = new Thread(Listen);
            _logListenThread = new Thread(Listen);

            _form.Log("Server init complete\n");
        }
        public void Start()
        {
            if (_dataListener == null || _logListener == null) 
                Init();
            if (_dataListenThread != null) 
                _dataListenThread.Start(_dataListener);
            if (_logListenThread != null) 
                _logListenThread.Start(_logListener);

            _form.Log("Server start complete\n");
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
                        _form.Log("Client connected: " + client.Client.RemoteEndPoint + "\n");
                        var helloThread = new Thread(SayHello);
                        helloThread.Start(client);

                        var clientThread = new Thread(PersistentConnection);
                        clientThread.Start(client);
                    }
                }               
            }
            listener.Stop();
        }

        private void SayHello(object client)
        {
            
            var tcpClient = (TcpClient) client;
            
            SendCommand(tcpClient, 0x00);
            Thread.Sleep(200);

            var bytesRead = 0;
            var receivedData = new byte[] {};
                     
            try { receivedData = ReceiveData(tcpClient.Client, out bytesRead, size: tcpClient.Available); }
            catch { _form.Log("Something went wrong... bummer."); }
            
            var encoder = new UTF8Encoding();
            var commandTable = JsonConvert.DeserializeObject<Dictionary<string,byte>>(encoder.GetString(receivedData, 0, bytesRead));
            
            ConnectionTable.Add(tcpClient,commandTable);
        }

        private void PersistentConnection(object client)
        {
            var tcpClient = (TcpClient) client;
            while (true)
            {
                if (Closing) break;
                if (tcpClient.Available == 0) continue;
                
                Thread.Sleep(200);

                int bytesRead;
                byte[] receivedData;

                try { receivedData = ReceiveData(tcpClient.Client, out bytesRead, size: tcpClient.Available); }
                catch { break; }
                
                var encoder = new UTF8Encoding();
                var decodedData = encoder.GetString(receivedData, 0, bytesRead);

                if(decodedData.Length>0) _form.Log("Received from: " + tcpClient.Client.RemoteEndPoint + " : " + decodedData + "\n");
                
            }
            _form.Log("Closing TCP connection with " + tcpClient.Client.RemoteEndPoint + "\n");
            ConnectionTable.Remove(tcpClient);
            tcpClient.Close();
        }
        public byte[] ReceiveData(Socket socket, out int bytesRead, int timeout = 10000, int size = 4096, int offset = 0)
        {
            var message = new byte[size];
            bytesRead = 0;
            socket.ReceiveTimeout = timeout;
            do
            {
                try
                {
                    bytesRead += socket.Receive(message, offset + bytesRead, size - bytesRead, SocketFlags.None);
                }
                catch (SocketException ex)
                {
                    _form.Log("Receive timeout reached. Error code: " + ex.Message);
                    break;
                }

            } while (bytesRead < size & socket.Poll(100, SelectMode.SelectRead));
            
            socket.ReceiveTimeout = 0;

            return message;
        }
        private byte[] Combine(byte[] a, byte[] b)
        {
            var c = new byte[a.Length + b.Length];
            Buffer.BlockCopy(a, 0, c, 0, a.Length);
            Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }       
        private void ProcessClientData(byte[] rawData, int bytesRead, TcpClient client)
        {
            
            var encoder = new UTF8Encoding();
            var decodedData = encoder.GetString(rawData, 0, bytesRead);
            

            _form.Log("Received from: " + client.Client.LocalEndPoint + " : " + encoder.GetString(rawData, 0, bytesRead) + "\n");

            if (decodedData == "Hello?")
            {
                _form.Log("Responding to keep-alive from: " + client.Client.LocalEndPoint + " : " + bytesRead+"\n");
                SendCommand(client,0x00);
            }

        }
        public void SendCommand(TcpClient client, byte byteToRyte)
        {
            var stream = client.GetStream();
            try
            {
                stream.Write(new[] {byteToRyte}, 0, 1);
            }
            catch (Exception e)
            {
                ConnectionTable.Remove(client);
                _form.Log(client.Client.RemoteEndPoint + "did not receive command. Removing from connected devices. ");
            }

        }
        public void Dispose()
        {
            Stop();
            
        }
    }

    
}