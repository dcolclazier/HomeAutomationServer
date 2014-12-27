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
        public Server Server { get; set; }
        public Form1()
        {
            InitializeComponent();
        }
        private void richText_Status_TextChanged(object sender, EventArgs e)
        {

        }
        public void WriteToBox(string text)
        {
            if (Server.Closing) return;

            richText_Status.AppendText(text);            
        }

        public void Log(string text)
        {
            if (Server.Closing) return;
            this.PerformSafely(() => WriteToBox(text+"\n"));
        }
        public void ClearTextBox()
        {
            richText_Status.Clear();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.WindowsShutDown) return;

            // Confirm user wants to close
            switch (MessageBox.Show(this, @"Are you sure you want to close?", @"Closing", MessageBoxButtons.YesNo))
            {
                case DialogResult.No:
                    e.Cancel = true;
                    break;
                default:
                    if (Server != null)
                    {
                        Server.Stop();
                        Thread.Sleep(1000);
                    }
                    break;
            }
        }

        private void btn_Clear_Click(object sender, EventArgs e)
        {
            richText_Status.Clear();
        }

        private void btn_SendCommand_Click(object sender, EventArgs e)
        {
            //ClientCommands status;
            //var success = Enum.TryParse(comboBox1.SelectedValue.ToString(), out status);
            var client = Server.ConnectionTable.FirstOrDefault(p => p.Key.Client.RemoteEndPoint == comboBox2.SelectedItem);
            
            //if(client.Key != null & success)
            //    Server.SendCommand(client.Key,(byte)status);

            var selectedCommand = (string)comboBox1.SelectedValue;
            var commandByte = client.Value[selectedCommand];
            //var array = new byte[client.Value.Keys.Count];
            
            //client.Value.Values.CopyTo(array, 0);
            Server.SendCommand(client.Key,commandByte);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.DataSource = Enum.GetValues(typeof(ClientCommands));
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedClient = (IPEndPoint)comboBox2.SelectedValue;
            var client = Server.ConnectionTable.FirstOrDefault(p=>Equals(p.Key.Client.RemoteEndPoint, selectedClient));
            var commandList = (from object key in client.Value.Keys select key.ToString()).ToList();
            
            comboBox1.DataSource = commandList;
        }

        private void btn_RefreshDevices_Click(object sender, EventArgs e)
        {
            comboBox2.DataSource = Server.ConnectionTable.Keys.Select(p=>p.Client.RemoteEndPoint).ToList();

            
            

        }
      
    }

    

    
    
}
