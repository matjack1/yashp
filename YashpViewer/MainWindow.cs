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

        private ArrayList threads;
        private ArrayList classes;
        private ArrayList inAppClasses;
        private ArrayList objects;

        private float currentZoom = 1.0f;

        public MainWindow()
        {
            InitializeComponent();
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

            redraw();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private ArrayList readXml(string xmlFile)
        {

            classes = new ArrayList();
            objects = new ArrayList();
            threads = new ArrayList();
            inAppClasses = new ArrayList();

            classList.Items.Clear();
            objectList.Items.Clear();
            threadList.Items.Clear();

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
                if (node.GetAttribute("isInApp", String.Empty).Length > 0)
                    m.IsInApp = Convert.ToBoolean(node.GetAttribute("isInApp", String.Empty));
                if (node.GetAttribute("static", String.Empty).Length > 0)
                    m.IsStatic = Convert.ToBoolean(node.GetAttribute("static", String.Empty));
                m.ReturnType = node.GetAttribute("returnType", String.Empty);

                functionInfos[m.FunctionID] = m;

                if (!classes.Contains(m.ClassName)) {
                    classes.Add(m.ClassName);
                    if (m.IsInApp) inAppClasses.Add(classes.Count - 1);
                    classList.Items.Add(m.ClassName);
                    classList.SetItemChecked(classList.Items.Count - 1, true);
                }
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

                    if (m.InstanceObjectID != "0" && !objects.Contains(m.InstanceObjectID))
                    {
                        objects.Add(m.InstanceObjectID);
                        objectList.Items.Add("Instance " + m.InstanceObjectID + " (" + m.MethodInfo.ClassName + ")");
                        objectList.SetItemChecked(objectList.Items.Count - 1, true);
                    }

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

                    if (!threads.Contains(t.ThreadID))
                    {
                        threads.Add(t.ThreadID);
                        threadList.Items.Add("Thread " + t.ThreadID);
                        threadList.SetItemChecked(threadList.Items.Count - 1, true);
                    }
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

        private void redraw()
        {
            ArrayList unwantedObjectsIDs = new ArrayList();
            ArrayList unwantedThreadsIDs = new ArrayList();
            ArrayList unwantedClassesIDs = new ArrayList();

            for (int i = 0; i < threadList.Items.Count; i++)
            {
                if (!threadList.GetItemChecked(i))
                {
                    unwantedThreadsIDs.Add( threads[ i ] );
                }
            }

            for (int i = 0; i < objectList.Items.Count; i++)
            {
                if (!objectList.GetItemChecked(i))
                {
                    unwantedObjectsIDs.Add( objects[ i ] );
                }
            }

            for (int i = 0; i < classList.Items.Count; i++)
            {
                if (!classList.GetItemChecked(i))
                {
                    unwantedClassesIDs.Add( classes[ i ] );
                }
            }
                
            umlGraph.setUnwantedObjectIDs(unwantedObjectsIDs);
            umlGraph.setUnwantedThreadIDs(unwantedThreadsIDs);
            umlGraph.setUnwantedClassesIDs(unwantedClassesIDs);

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

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            currentZoom *= 0.9f;
            umlGraph.setZoom(currentZoom);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            currentZoom *= 1.1f;
            umlGraph.setZoom(currentZoom);
        }

        private void selectThreads_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objectList.Items.Count; i++)
            {
                threadList.SetItemChecked(i, true);
            }
            redraw();
        }

        private void deselectThreads_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objectList.Items.Count; i++)
            {
                threadList.SetItemChecked(i, false);
            }
            redraw();
        }

        private void selectObjects_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < objectList.Items.Count; i++) {
                objectList.SetItemChecked(i, true);
            }
            redraw();
        }

        private void deselectObjects_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < classList.Items.Count; i++)
            {
                objectList.SetItemChecked(i, false);
            }
            redraw();
        }

        private void selectClasses_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < classList.Items.Count; i++)
            {
                classList.SetItemChecked(i, true);
            }
            redraw();
        }

        private void deselectClasses_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < classList.Items.Count; i++)
            {
                classList.SetItemChecked(i, false);
            }
            redraw();
        }

        private void classList_SelectedIndexChanged(object sender, EventArgs e)
        {
            redraw();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < classList.Items.Count; i++)
            {
                classList.SetItemChecked(i, inAppClasses.Contains(i));
            }
            redraw();
        }
    }
}
