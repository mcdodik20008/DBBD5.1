using System;
using System.Windows.Forms;

namespace BD_ITOG
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // надеюсь, что все не очевидное пояснил 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
