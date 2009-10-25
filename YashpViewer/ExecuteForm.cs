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
    public partial class ExecuteForm : Form
    {
        private const string PROFILER_GUID = "{64E1DC78-A253-4DD0-8A9E-53AF1FDDAB95}";

        public ExecuteForm()
        {
            InitializeComponent();
            button1.Enabled = false;
            //textBox2.Text = "C:\\";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Documents and Settings\\Administrator\\My Documents\\Visual Studio 2008\\Projects\\yashp\\ApplicationTest\\bin\\Release";
            openFileDialog1.Filter = "EXE files (*.exe)|*.exe|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = openFileDialog1.FileName;
                button1.Enabled = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = "C:\\Documents and Settings\\Administrator\\My Documents\\Visual Studio 2008\\Projects\\yashp\\ApplicationTest\\bin\\Release";
            saveFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = saveFileDialog1.FileName;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProcessStartInfo psi;

            // make sure the executable exists
            if (File.Exists(textBox1.Text) == false)
            {
                MessageBox.Show("The executable '" + textBox1.Text + "' does not exist.");
                return;
            }

            // make sure the XML exists
            /*if (File.Exists(textBox2.Text) == false)
            {
                MessageBox.Show("The XML '" + textBox1.Text + "' does not exist.");
                return;
            }*/

            // create a process executor
            psi = new ProcessStartInfo(textBox1.Text);

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

            // set the XML_FILENAME env var. This indicates to our profiler where the XML 
            // file should be written to.
            if (psi.EnvironmentVariables.ContainsKey("XML_FILENAME") == true)
                psi.EnvironmentVariables["XML_FILENAME"] = textBox2.Text;
            else
                psi.EnvironmentVariables.Add("XML_FILENAME", textBox2.Text);


            // ----- RUN THE PROCESS -------------------

            psi.UseShellExecute = false;
            Process p = Process.Start(psi);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
