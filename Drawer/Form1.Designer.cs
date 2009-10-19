namespace Drawer
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
            this.umlDrawer1 = new Drawer.UMLDrawer();
            this.SuspendLayout();
            // 
            // umlDrawer1
            // 
            this.umlDrawer1.Location = new System.Drawing.Point(12, 12);
            this.umlDrawer1.Name = "umlDrawer1";
            this.umlDrawer1.Size = new System.Drawing.Size(654, 537);
            this.umlDrawer1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 557);
            this.Controls.Add(this.umlDrawer1);
            this.Name = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private UMLDrawer umlDrawer1;

    }
}

