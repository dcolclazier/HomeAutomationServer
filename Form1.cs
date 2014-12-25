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

            

        }
    }
    
    
}
