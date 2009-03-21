using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;

namespace yashp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Profiler profiler = new Profiler();
            string message;
            bool success = profiler.CheckSetup(out message);
            if (!success)
            {
                Console.WriteLine("Application setup error");
                return;
            }
            profiler.Start();
            return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }
    }


    public class Profiler
    {
        public const string PROFILER_GUID = "5553A90C-3E0E-4D55-833A-CB5128BD7258";
        public static string Version
        {
            get { return "0.10"; }
        }
        public EventHandler completed;
        public bool CheckSetup(out string message)
        {
            message = String.Empty;
            using (RegistryKey rk = Registry.ClassesRoot.OpenSubKey("CLSID\\" + "{" + PROFILER_GUID + "}"))
            {
                if (rk == null)
                {
                    message = "Unable to find the registry key for the profiler hook.  Please register the NProf.Hook.dll file.";
                    return false;
                }
            }

            return true;
        }
        public bool Start()
        {
            process = new Process();
            process.StartInfo = new ProcessStartInfo("C:\\Documents and Settings\\Administrator\\My Documents\\Visual Studio 2008\\Projects\\yashp\\test_program\\bin\\Release\\test_program.exe", "");
            process.StartInfo.EnvironmentVariables["COR_ENABLE_PROFILING"] = "0x1";
            process.StartInfo.EnvironmentVariables["COR_PROFILER"] = "{" + PROFILER_GUID + "}";
            process.StartInfo.UseShellExecute = false;
            process.EnableRaisingEvents = true;
            process.Exited += new EventHandler(OnProcessExited);
            return process.Start();
        }
        private void OnProcessExited(object oSender, EventArgs ea)
        {
            completed(null, null);
        }
        private Process process;
    }
    
}
