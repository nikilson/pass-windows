using System;
///using System.Collections.Generic;
///using System.Linq;
///using System.Threading.Tasks;
///using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ClearClipboard
{
    static class CleanClipboard
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        /// [DllImport("user32.dll")]
        [DllImport("user32.dll")]
        private static extern int ShowWindow(int Handle, int showState);

        [DllImport("kernel32.dll")]
        public static extern int GetConsoleWindow();

        [STAThread]
        static void Main(string[] args)
        {
            ///Application.SetHighDpiMode(HighDpiMode.SystemAware);
            ///Application.EnableVisualStyles();
            ///Application.SetCompatibleTextRenderingDefault(false);
            ///
            int win = GetConsoleWindow();
            ShowWindow(win, 0);

            string currPrsName = Process.GetCurrentProcess().ProcessName;
            Process[] allProcessWithThisName = Process.GetProcessesByName(currPrsName);
            Process currentProcess = Process.GetCurrentProcess();
            int currentProcessId = currentProcess.Id;
            foreach (Process v in allProcessWithThisName)
            {
                if (currentProcessId != v.Id)
                {
                    v.Kill();
                }
            }
            System.Threading.Thread.Sleep(45*1000);
            System.Windows.Forms.Clipboard.Clear();
        }
    }
}
