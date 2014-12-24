using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomeAutomationServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void richText_Status_TextChanged(object sender, EventArgs e)
        {

        }
       
        

        

        public void WriteToBox(string text)
        {
            richText_Status.AppendText(text);
        }

        public void ClearTextBox()
        {
            richText_Status.Clear();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            var server = new Server(this, 420);

        }
    }
    public static class CrossThreadExtensions
        {
            public static void PerformSafely(this Control target, Action action)
            {
                if (target.InvokeRequired)
                    target.Invoke(action);
                else
                    action();
            }
            public static void PerformSafely<T1>(this Control target, Action<T1> action, T1 parameter)
            {
                if (target.InvokeRequired)
                    target.Invoke(action, parameter);
                else
                    action(parameter);
            }
            public static void PerformSafely<T1, T2>(this Control target, Action<T1, T2> action, T1 p1, T2 p2)
            {
                if (target.InvokeRequired)
                    target.Invoke(action, p1, p2);
                else
                    action(p1, p2);
            }
        }
    public class Server
        {
            private readonly TcpListener _tcpListener;
            private readonly Form1 _form;
            private readonly TcpListener _helloListener;

            public Server(Form1 form, int listeningPort)
            {
                _form = form;
                _tcpListener = new TcpListener(IPAddress.Parse("192.168.168.101"), listeningPort);
                _helloListener = new TcpListener(IPAddress.Parse("192.168.168.101"), 59400);

                var listenThread = new Thread(ListenForConnection);
                var helloThread = new Thread(ListenForConnection);
                _form.WriteToBox("It's working!\n");
                listenThread.Start(_tcpListener);
                helloThread.Start(_helloListener);
            }

            private void ListenForConnection(object obj)
            {
                var listener = (TcpListener)obj;
                listener.Start();
                while (true)
                {
                    var client = listener.AcceptTcpClient();
                    var clientThread = new Thread(HandleClientComm);
                    clientThread.Start(client);
                }
            }
            private void ListenForClients()
            {
                _tcpListener.Start();
                while (true)
                {
                    var client = _tcpListener.AcceptTcpClient();
                    var clientThread = new Thread(HandleClientComm);
                    clientThread.Start(client);
                }
            }

            private void HandleClientComm(object client) //bug - refactor the shit out of this... i hate it - its ugly.
            {

                var tcpClient = (TcpClient)client;
                var encoder = new UTF8Encoding();
                var clientStream = tcpClient.GetStream();


                var buffer = encoder.GetBytes("Hello!");
                clientStream.Write(buffer, 0, buffer.Length);
                clientStream.Flush();


                var message = new byte[4096];
                int bytesRead;
                while (true)
                {
                    bytesRead = 0;
                    try
                    {
                        bytesRead = clientStream.Read(message, 0, 4096);
                    }
                    catch
                    {
                        break;
                    }
                    if (bytesRead == 0)
                    {
                        break;
                    }
                    ProcessMessage(message, bytesRead, clientStream);
                    //_form.PerformSafely(()=>_form.ClearTextBox());
                    var read = bytesRead;
                    _form.PerformSafely(() => _form.WriteToBox(encoder.GetString(message, 0, read) + "\n"));
                }


                tcpClient.Close();
            }

            private void ProcessMessage(byte[] buffer, int bytesRead, Stream stream)
            {
                var encoder = new UTF8Encoding();
                var message = encoder.GetString(buffer, 0, bytesRead);

                if (message == "Hello?")
                {
                    Debug.Print("Got a 'Hello?'   !!!");
                    var sendBuffer = encoder.GetBytes("Hello!");
                    stream.Write(sendBuffer, 0, sendBuffer.Length);
                }

            }




        }
    
}
