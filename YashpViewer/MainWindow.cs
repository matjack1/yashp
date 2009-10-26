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


namespace YashpViewer
{
    public partial class MainWindow : Form
    {
        private ArrayList events;
        private SortedList objects;
        private SortedList threads;

        public MainWindow()
        {
            InitializeComponent();
            objects = new SortedList();
            threads = new SortedList();
        }

        private void attachToProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExecuteForm form = new ExecuteForm();
            form.ShowDialog();
        }

        private void loadXMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = "C:\\Documents and Settings\\Administrator\\My Documents\\Visual Studio 2008\\Projects\\yashp\\ApplicationTest\\bin\\Release";
            openFileDialog1.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                events = readXml(openFileDialog1.FileName);
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private ArrayList readXml(string xmlFile)
        {
            XPathDocument doc = new XPathDocument(xmlFile);
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

                if (!objects.ContainsKey(m.ClassName))
                {
                    objects.Add(m.ClassName, m.FunctionID);
                }
            }

            // filling objects checked list
            foreach (DictionaryEntry d in objects)
            {
                objectList.Items.Add(d.Key);
                objectList.SetItemChecked(objectList.Items.Count - 1, true);
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

                    if (!threads.ContainsKey(t.ThreadID))
                    {
                        threads.Add(t.ThreadID, t.ThreadID);
                    }
                }
                if (node.Name == "exceptionEvent")
                {
                    ExceptionEvent t = new ExceptionEvent();
                    t.timestamp = Convert.ToDouble(node.GetAttribute("timestamp", String.Empty));
                    events.Add(t);
                }
            }

            // filling threads checked list
            foreach (DictionaryEntry d in threads)
            {
                threadList.Items.Add(d.Key);
                threadList.SetItemChecked(threadList.Items.Count - 1, true);
            }

            return events;
        }

        private void redraw()
        {
            ArrayList unwantedObjectsIDs = new ArrayList();
            ArrayList unwantedThreadsIDs = new ArrayList();

            for (int i = 0; i < threadList.Items.Count; i++)
            {
                if (!threadList.GetItemChecked(i))
                {
                    unwantedThreadsIDs.Add( threads[ threadList.Items[i].ToString() ] );
                }
            }

            for (int i = 0; i < objectList.Items.Count; i++)
            {
                if (!objectList.GetItemChecked(i))
                {
                    unwantedObjectsIDs.Add( objects[ objectList.Items[i].ToString() ] );
                }
            }
                
            umlGraph.setUnwantedObjectIDs(unwantedObjectsIDs);
            umlGraph.setUnwantedThreadIDs(unwantedThreadsIDs);
            this.umlGraph.setEvents(events);

        }

        private void objectList_SelectedIndexChanged(object sender, EventArgs e)
        {
            redraw();
        }

        private void threadList_SelectedIndexChanged(object sender, EventArgs e)
        {
            redraw();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {

        }
    }
}
