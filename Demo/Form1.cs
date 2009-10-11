using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            MethodInfo m1 = new MethodInfo();

            MethodEvent e1 = new MethodEvent();
            e1.EventType = MethodEvent.EventTypeEnum.EnterEvent;
            e1.InstanceObjectID = 0;
            e1.MethodInfo = m1;

            ArrayList events = new ArrayList();
            events.Add(e1);
        }

    }
}
