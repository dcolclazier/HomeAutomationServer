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

        public void PrintLogEntry(string text)
        {
            if (Server.Closing) return;
            this.PerformSafely(() => WriteToBox(text));
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
      
    }

    

    
    
}
