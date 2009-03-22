using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace YashpViewer
{
    public partial class MainWindow : Form
    {
        private const string PROFILER_GUID = "{64E1DC78-A253-4DD0-8A9E-53AF1FDDAB95}";

        public MainWindow()
        {
            InitializeComponent();
            start.Enabled = false;
        }

        private void browse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "EXE files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path.Text = openFileDialog1.FileName;
                start.Enabled = true;
            }

        }

        private void start_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi;

            // make sure the executable exists
            if (File.Exists(path.Text) == false)
            {
                MessageBox.Show("The executable '" + path.Text + "' does not exist.");
                return;
            }

            // create a process executor
            psi = new ProcessStartInfo(path.Text);

            // ----- SET THE CLR ENVIRONMENT VARIABLES -------------------

            // set the Cor_Enable_Profiling env var. This indicates to the CLR whether or
            // not we are using the profiler at all.  1 = yes, 0 = no.
            if (psi.EnvironmentVariables.ContainsKey("COR_ENABLE_PROFILING") == true)
                psi.EnvironmentVariables["COR_ENABLE_PROFILING"] = "1";
            else
                psi.EnvironmentVariables.Add("COR_ENABLE_PROFILING", "1");

            // set the COR_PROFILER env var. This indicates to the CLR which COM object to
            // instantiate for profiling.
            if (psi.EnvironmentVariables.ContainsKey("COR_PROFILER") == true)
                psi.EnvironmentVariables["COR_PROFILER"] = PROFILER_GUID;
            else
                psi.EnvironmentVariables.Add("COR_PROFILER", PROFILER_GUID);

            // ----- SET THE PROFILER ENVIRONMENT VARIABLES -------------------

            // set the LOG_FILENAME env var. This indicates to our profiler where the log 
            // file should be written to.
            if (psi.EnvironmentVariables.ContainsKey("LOG_FILENAME") == true)
                psi.EnvironmentVariables["LOG_FILENAME"] = @"ICorProfilerCallback Log.log";
            else
                psi.EnvironmentVariables.Add("LOG_FILENAME", @"ICorProfilerCallback Log.log");

            // ----- RUN THE PROCESS -------------------

            psi.UseShellExecute = false;
            Process p = Process.Start(psi);

        }
    }
}
