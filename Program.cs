using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var form1 = new Form1();
            var server = new Server(form1, "192.168.168.101", 420);
            server.Start();
            Application.Run(form1);
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

    public class NewServer
    {
        //public List<> ClientConnections { get; private set; }


    }
    public class Server
    {
        //private readonly TcpListener _tcpListener;
        private readonly Form1 _form;
        private readonly string _ipAddress;
        private readonly int _listeningPort;
        private readonly int _keepAlivePort;
        private TcpListener _tcpListener;
        private TcpListener _keepAliveListener;
        private Thread _listenThread;
        private Thread _helloThread;

        //public List<ClientConnection> Connections;    

        public Server(Form1 form, string ipAddress, int listeningPort, int keepAlivePort = 59400)
        {
            _form = form;
            _ipAddress = ipAddress;
            _listeningPort = listeningPort;
            _keepAlivePort = keepAlivePort;          
            _form.PerformSafely(() => _form.WriteToBox("It's working!\n"));          
        }

        private void Init()
        {
            _tcpListener = new TcpListener(IPAddress.Parse(_ipAddress), _listeningPort);
            _keepAliveListener = new TcpListener(IPAddress.Parse(_ipAddress), _keepAlivePort);
            _listenThread = new Thread(ListenForConnection);
            _helloThread = new Thread(ListenForConnection);
            
        }

        public void Start()
        {
            if (_tcpListener == null || _keepAliveListener == null) Init();

            if (_listenThread != null) _listenThread.Start(_tcpListener);
            if (_helloThread != null) _helloThread.Start(_keepAliveListener);
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
