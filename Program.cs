using System;
using System.Collections.Generic;
using System.Linq;
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
            form1.Server = new Server(form1, "192.168.168.101", 420);
            form1.Server.Start();

            Application.Run(form1);
            
            
        }
    }
    
}
