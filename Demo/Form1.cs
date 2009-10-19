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
            m1.ClassName = "TestClass";
            m1.MethodName = "TestMethod";
            m1.FunctionID = 2123;

            MethodInfo m2 = new MethodInfo();
            m2.ClassName = "StartClass";
            m2.MethodName = "Main";
            m2.FunctionID = 2133;

            DateTime now = DateTime.Now;

            MethodEvent e1 = new MethodEvent();
            e1.EventType = MethodEvent.EventTypeEnum.EnterEvent;
            e1.InstanceObjectID = 3;
            e1.MethodInfo = m1;
            e1.timestamp = new DateTime(now.Ticks + 200);

            MethodEvent e2 = new MethodEvent();
            e2.EventType = MethodEvent.EventTypeEnum.EnterEvent;
            e2.InstanceObjectID = 4;
            e2.MethodInfo = m1;
            e2.timestamp = new DateTime(now.Ticks + 800);

            MethodEvent e3 = new MethodEvent();
            e3.EventType = MethodEvent.EventTypeEnum.EnterEvent;
            e3.InstanceObjectID = 5;
            e3.MethodInfo = m1;
            e3.timestamp = new DateTime(now.Ticks + 1200);

            MethodEvent e0 = new MethodEvent();
            e0.EventType = MethodEvent.EventTypeEnum.EnterEvent;
            e0.InstanceObjectID = 6;
            e0.MethodInfo = m2;
            e0.timestamp = new DateTime(now.Ticks);

            MethodEvent l1 = new MethodEvent();
            l1.EventType = MethodEvent.EventTypeEnum.LeaveEvent;
            l1.InstanceObjectID = 3;
            l1.MethodInfo = m1;
            l1.timestamp = new DateTime(now.Ticks + 500);

            MethodEvent l2 = new MethodEvent();
            l2.EventType = MethodEvent.EventTypeEnum.LeaveEvent;
            l2.InstanceObjectID = 4;
            l2.MethodInfo = m1;
            l2.timestamp = new DateTime(now.Ticks + 2500);

            MethodEvent l3 = new MethodEvent();
            l3.EventType = MethodEvent.EventTypeEnum.LeaveEvent;
            l3.InstanceObjectID = 5;
            l3.MethodInfo = m1;
            l3.timestamp = new DateTime(now.Ticks + 1500);

            MethodEvent l0 = new MethodEvent();
            l0.EventType = MethodEvent.EventTypeEnum.LeaveEvent;
            l0.InstanceObjectID = 6;
            l0.MethodInfo = m2;
            l0.timestamp = new DateTime(now.Ticks + 3500);

            ArrayList events = new ArrayList();
            events.Add(e0);
            events.Add(e1);
            events.Add(l1);
            events.Add(e2);
            events.Add(e3);
            events.Add(l3);
            events.Add(l2);
            events.Add(l0);

            this.umlGraph.setEvents(events);
        }

    }
}
