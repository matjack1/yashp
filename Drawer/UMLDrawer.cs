using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Drawer
{
    public partial class UMLDrawer : UserControl
    {
        public UMLDrawer()
        {
            InitializeComponent();
        }
        private void onPaint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Yellow, ClientRectangle);
            g.DrawString("Hello, world", Font, Brushes.Black, 0, 0);
        }
        private void InitializeComponent() {
            this.SuspendLayout();
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.onPaint);
            this.ResumeLayout(false);
        }
    }

}


