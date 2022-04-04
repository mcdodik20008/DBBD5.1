using System;
using System.Windows.Forms;

namespace DBBD51
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
