﻿namespace Demo
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.umlGraph = new Demo.UMLGraph();
            this.SuspendLayout();
            // 
            // umlGraph
            // 
            this.umlGraph.AutoSize = true;
            this.umlGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.umlGraph.Location = new System.Drawing.Point(0, 0);
            this.umlGraph.Name = "umlGraph";
            this.umlGraph.Size = new System.Drawing.Size(847, 612);
            this.umlGraph.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 612);
            this.Controls.Add(this.umlGraph);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UMLGraph umlGraph;
    }
}

