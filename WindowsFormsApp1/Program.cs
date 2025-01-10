// Main Program
using System;
using System.Windows.Forms;
using InvoiceApp.UI;

namespace InvoiceApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}