using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml.XPath;


namespace Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ArrayList m = readFromXML();

            this.umlGraph.setEvents(m);
        }

        ArrayList readFromXML()
        {
            XPathDocument doc = new XPathDocument("C:\\Documents and Settings\\Administrator\\My Documents\\Visual Studio 2008\\Projects\\yashp\\YashpViewer\\bin\\Debug\\YashpOutput.xml");
            XPathNavigator nav = doc.CreateNavigator();

            XPathExpression functionInfosExpression = nav.Compile("infos/functionInfos/functionInfo");
            XPathNodeIterator functionInfosIterator = nav.Select(functionInfosExpression);

            XPathExpression eventsExpression = nav.Compile("infos/events/*");
            XPathNodeIterator eventsIterator = nav.Select(eventsExpression);

            Hashtable functionInfos = new Hashtable();

            while (functionInfosIterator.MoveNext())
            {
                XPathNavigator node = functionInfosIterator.Current.Clone();

                MethodInfo m = new MethodInfo();
                m.FunctionID = node.GetAttribute("functionId", String.Empty);
                m.ClassName = node.GetAttribute("className", String.Empty);
                m.MethodName = node.GetAttribute("methodName", String.Empty);
                if (node.GetAttribute("static", String.Empty).Length > 0)
                m.IsStatic = Convert.ToBoolean(node.GetAttribute("static", String.Empty));
                m.ReturnType = node.GetAttribute("returnType", String.Empty);

                functionInfos[m.FunctionID] = m;
            }

            ArrayList events = new ArrayList();

            while (eventsIterator.MoveNext())
            {
                XPathNavigator node = eventsIterator.Current.Clone();

                if (node.Name == "methodEvent")
                {
                    MethodEvent m = new MethodEvent();
                    m.InstanceObjectID = node.GetAttribute("objectId", String.Empty);
                    m.MethodInfo = (MethodInfo)functionInfos[node.GetAttribute("functionId", String.Empty)];
                    m.ThreadID = node.GetAttribute("threadId", String.Empty);
                    m.timestamp = Convert.ToDouble(node.GetAttribute("timestamp", String.Empty));

                    String type = node.GetAttribute("type", String.Empty);

                    if (type == "Enter")
                        m.EventType = MethodEvent.EventTypeEnum.EnterEvent;
                    else if (type == "Leave")
                        m.EventType = MethodEvent.EventTypeEnum.LeaveEvent;

                    events.Add(m);
                }
                if (node.Name == "threadEvent")
                {
                    ThreadEvent t = new ThreadEvent();
                    t.timestamp = Convert.ToDouble(node.GetAttribute("timestamp", String.Empty));
                    t.ThreadID = node.GetAttribute("threadId", String.Empty);

                    String type = node.GetAttribute("type", String.Empty);
                    if (type == "Create")
                        t.EventType = ThreadEvent.EventTypeEnum.CreateEvent;
                    else if (type == "Destroy")
                        t.EventType = ThreadEvent.EventTypeEnum.DestroyEvent;

                    events.Add(t);
                }
                if (node.Name == "exceptionEvent")
                {
                    ExceptionEvent t = new ExceptionEvent();
                    t.timestamp = Convert.ToDouble(node.GetAttribute("timestamp", String.Empty));
                    events.Add(t);
                }
            }

            return events;
        }



    }

    
}
