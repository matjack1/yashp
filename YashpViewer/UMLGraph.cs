using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace YashpViewer
{
    class UMLGraph : UserControl
    {

        private Color[] threadColorLibrary = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Pink };
        private Color backgroundColor = Color.White;
        private int nextFreeThreadColor = 0;

        static private int padding = 30;
        static private int colWidth = 150;
        static private int methodBodyWidth = 20;
        static private int colInterspace = 20;
        static private int timestampWidth = 60;

        static private int nextFreeColumn = 0;
        static private float zoomScale = 1.0f;

        private Hashtable classData = new Hashtable();
        private Hashtable lastMethodData = new Hashtable();
        private Hashtable threadColor = new Hashtable();
        private ArrayList events = new ArrayList();

        private ArrayList unwantedObjectIDs = new ArrayList();
        private ArrayList unwantedThreadIDs = new ArrayList();
        private ArrayList unwantedClassesIDs = new ArrayList(); 

        class ClassData
        {
            public int columnNumber;
            public MethodInfo methodInfo;
            public String instanceObjectID;

            public ArrayList methodData = new ArrayList();

            public float getColumnLeftX()
            {
                return columnNumber * (colWidth + colInterspace) * zoomScale;
            }
        }

        class MethodData
        {
            public int methodColumnNumber;
            public MethodEvent methodEvent;
            public MethodData callerMethodData;
            public ClassData classData;
            public float leaveY;

            public float getColumnLeftX()
            {
                return (colWidth - methodBodyWidth) * 0.5f * zoomScale + classData.getColumnLeftX() + (methodBodyWidth * 0.5f * zoomScale) * methodColumnNumber;
            }

            public float getColumnRightX()
            {
                return getColumnLeftX() + methodBodyWidth * zoomScale;
            }
        }


        public UMLGraph()
        {
        }

        public void setUnwantedThreadIDs(ArrayList ids) {
            unwantedThreadIDs = ids;
            this.Invalidate();
        }


        public void setUnwantedClassesIDs(ArrayList ids) {
            unwantedClassesIDs = ids;
            this.Invalidate();
        }

        public void setUnwantedObjectIDs(ArrayList ids)
        {
            unwantedObjectIDs = ids;
            this.Invalidate();
        }

        public void setZoom(float zoom) {
            zoomScale = Math.Min(Math.Max(zoom, 0.5f), 2.0f);
            this.Invalidate();
        }

        public void setEvents(ArrayList eventsLog)
        {
            events = eventsLog;

            classData = new Hashtable();
            lastMethodData = new Hashtable();
            threadColor = new Hashtable();
            nextFreeThreadColor = 0;
            nextFreeColumn = 0;

            double lastTimestamp = ((LogEvent)events[0]).timestamp;
            float lastY = 0;
            double mediumDeltaTime = getMediumDeltaTime();

            foreach (LogEvent logEvent in events)
            {
                double deltaTime = logEvent.timestamp - lastTimestamp;
                float deltaY = (float) Math.Max(20, Math.Min(40 * (deltaTime / mediumDeltaTime), 80));
                lastY += deltaY;
                lastTimestamp = logEvent.timestamp;

                if (logEvent is MethodEvent)
                {
                    MethodEvent e = (MethodEvent) logEvent;
                    ClassData currentClassData = getOrCreateClassData(e);
                }
            }

            this.MinimumSize = new Size((int) (timestampWidth + 3.0f * padding + classData.Count * (colWidth + colInterspace)), (int) (2.0f * padding + lastY + 40));
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs eva)
        {
            base.OnPaint(eva);

            classData = new Hashtable();
            lastMethodData = new Hashtable();
            nextFreeColumn = 0;

            Graphics g = eva.Graphics;

            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, Width, Height));

            if (events.Count == 0) {
                return;
            }
            
            g.TranslateTransform(padding + padding + timestampWidth, padding);

            double lastTimestamp = ((LogEvent)events[0]).timestamp;
            double firstTimeStamp = lastTimestamp;
            float lastY = 0;
            double mediumDeltaTime = getMediumDeltaTime();
            String lastThreadId = null;

            Hashtable threadCreationY = new Hashtable();
            Hashtable threadDestroyY = new Hashtable();

            foreach (LogEvent logEvent in events)
            {
                if (logEvent is MethodEvent)
                {
                    MethodEvent e = (MethodEvent)logEvent;
                    if (unwantedObjectIDs.Contains(e.InstanceObjectID)) continue;
                    if (unwantedThreadIDs.Contains(e.ThreadID)) continue;
                    if (unwantedClassesIDs.Contains(e.MethodInfo.ClassName)) continue;
                }

                if (logEvent is ThreadEvent)
                {
                    ThreadEvent e = (ThreadEvent)logEvent;
                    if (unwantedThreadIDs.Contains(e.ThreadID)) continue;
                }

                double deltaTime = logEvent.timestamp - lastTimestamp;
                float deltaY = (float) Math.Max(20, Math.Min(40 * (deltaTime / mediumDeltaTime), 80)) * zoomScale;

                float topY = padding + lastY - 20;
                float bottomY = topY + deltaY + 40;

                bool hasToDraw = !((topY <= eva.ClipRectangle.Top && bottomY <= eva.ClipRectangle.Top) || (topY >= eva.ClipRectangle.Bottom && bottomY >= eva.ClipRectangle.Bottom));

                if (hasToDraw)
                {

                    String time = (lastTimestamp + deltaTime - firstTimeStamp) + " us";
                    g.DrawString(time, new Font("Tahoma", 7), Brushes.Black, -padding - timestampWidth, deltaY + lastY - 7);

                    foreach (DictionaryEntry de in classData)
                    {
                        ClassData d = (ClassData)de.Value;

                        foreach (MethodData m in d.methodData)
                        {
                            drawMethodData(g, m, lastY, lastY + deltaY);
                        }
                    }
                }

                if (logEvent is MethodEvent)
                {
                    MethodEvent e = (MethodEvent) logEvent;

                    String currentThreadID = e.ThreadID;

                    bool mustDrawHeader = !classData.ContainsKey(e.getHash());
                    ClassData currentClassData = getOrCreateClassData(e);

                    if (mustDrawHeader)
                    {
                        drawHeader(g, e, currentClassData.getColumnLeftX(), lastY + deltaY);
                    }
                    
                    if (e.EventType == MethodEvent.EventTypeEnum.EnterEvent) {

                        MethodData currentMethodData = new MethodData();
                        

                        if (lastMethodData.ContainsKey(currentThreadID))
                            currentMethodData.callerMethodData = (MethodData) lastMethodData[currentThreadID];
                        
                        currentMethodData.classData = currentClassData;
                        currentMethodData.methodColumnNumber = currentClassData.methodData.Count;
                        currentMethodData.methodEvent = e;

                        currentClassData.methodData.Add(currentMethodData);

                        lastMethodData[currentThreadID] = currentMethodData;

                        if (hasToDraw)
                            drawMethodEnter(g, currentMethodData, lastY + deltaY);

                        if (threadCreationY.ContainsKey(currentThreadID)) {
                            float startY = (float)threadCreationY[currentThreadID];
                            drawThreadCreate(g, currentMethodData, startY, lastY + deltaY);
                            threadCreationY.Remove(currentThreadID);
                        }

                    }
                    else if (e.EventType == MethodEvent.EventTypeEnum.LeaveEvent) {

                        MethodData currentMethodData = (MethodData) currentClassData.methodData[currentClassData.methodData.Count - 1];
                        lastMethodData[currentThreadID] = currentMethodData.callerMethodData;
                        currentClassData.methodData.RemoveAt(currentClassData.methodData.Count - 1);

                        threadDestroyY[currentThreadID] = currentMethodData;
                        currentMethodData.leaveY = lastY + deltaY;

                        if (hasToDraw)
                         drawMethodLeave(g, currentMethodData, lastY + deltaY);

                    }

                    lastThreadId = currentThreadID;
                }

                if (logEvent is ExceptionEvent)
                {
                    if (hasToDraw && lastThreadId != null)
                        drawException(g, (MethodData) lastMethodData[lastThreadId], lastY + deltaY);
                }

                if (logEvent is ThreadEvent)
                {
                    ThreadEvent threadEvent = (ThreadEvent)logEvent;

                    if (threadEvent.EventType == ThreadEvent.EventTypeEnum.CreateEvent)
                    {
                        threadCreationY[threadEvent.ThreadID] = lastY + deltaY;
                    }

                    if (threadEvent.EventType == ThreadEvent.EventTypeEnum.DestroyEvent)
                    {
                        MethodData md = (MethodData)threadDestroyY[threadEvent.ThreadID];
                        drawThreadDestroy(g, md, md.leaveY, lastY + deltaY);
                    }
                }

                lastY += deltaY;
                lastTimestamp = logEvent.timestamp;

            }
            
        }

        ClassData getOrCreateClassData(MethodEvent mev)
        {
            String hash = mev.getHash();

            if (!classData.ContainsKey(hash))
            {
                ClassData c = new ClassData();
                c.instanceObjectID = mev.InstanceObjectID;
                c.columnNumber = nextFreeColumn++;
                this.classData[hash] = c;
            }

            return (ClassData) this.classData[hash];
        }

        double getMediumDeltaTime()
        {

            if (events.Count == 0) return 0;

            double totDelta = 0;
            double deltaCount = 0;
            double startTime = ((LogEvent)events[0]).timestamp;
            for (int i = 1; i < events.Count; i++)
            {
                double t1 = ((LogEvent)events[i - 1]).timestamp;
                double t2 = ((LogEvent)events[i]).timestamp;
                double delta = t2 - t1;
                totDelta += delta;
                deltaCount++;
            }

            return Math.Max(totDelta / deltaCount, 1);
        }

        void drawHeader(Graphics g, MethodEvent e, float x, float y)
        {
            String title = e.MethodInfo.ClassName;
            Pen blackBorderPen = new Pen(Color.Gray);
            Font textFont = new Font("Tahoma", 8 * zoomScale);

            if (!e.MethodInfo.IsStatic)
                title += "\nInst. " + e.InstanceObjectID.ToString();

            StringFormat f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Center;

            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString(title, textFont, new SizeF(colWidth * zoomScale, 50));

            int boxHeight = (int) (stringSize.Height + 20);

            if (e.MethodInfo.IsStatic)
            {
                g.DrawRectangle(blackBorderPen, x, y - boxHeight - 20, colWidth * zoomScale, boxHeight);
            }
            else
            {
                RoundedCornerRectangle(g, blackBorderPen, x, y - boxHeight - 20, colWidth * zoomScale, boxHeight, boxHeight * 0.3f);
            }

            g.DrawString(title, textFont, Brushes.Black, x + colWidth * 0.5f * zoomScale, y + 10 + stringSize.Height * 0.5f - boxHeight - 20, f); 
        }

        Color getThreadColor(String ThreadID) {

            if (!threadColor.ContainsKey(ThreadID)) {
                Console.WriteLine("Creo per " + ThreadID);
                threadColor[ThreadID] = threadColorLibrary[nextFreeThreadColor++];
                if (nextFreeThreadColor == threadColorLibrary.Length) nextFreeThreadColor = 0;
            }

            Console.WriteLine(ThreadID + " -> " + threadColor[ThreadID]);

            return (Color) threadColor[ThreadID];
        }

        void drawMethodData(Graphics g, MethodData d, float startY, float endY)
        {
            Pen pen = new Pen(getThreadColor(d.methodEvent.ThreadID));

            float leftX = d.getColumnLeftX();
            float rightX = d.getColumnRightX();

            //g.DrawRectangle(pen, leftX, startY, rightX - leftX, endY - startY);
            g.DrawLine(pen, leftX, startY, leftX, endY);
            g.DrawLine(pen, rightX, startY, rightX, endY);
        }

        void drawThreadCreate(Graphics g, MethodData d, float startY, float endY)
        {
            String threadID = d.methodEvent.ThreadID;

            float x = d.getColumnLeftX() + methodBodyWidth * zoomScale * 0.25f;

            Pen pen = new Pen(getThreadColor(threadID), 2 * zoomScale);
            Brush brush = new SolidBrush(getThreadColor(threadID));

            float dotWidth = 5 * zoomScale;

            g.FillEllipse(brush, 0, startY - dotWidth * 0.5f, dotWidth, dotWidth);
            g.DrawLine(pen, 0, startY, x, startY);
            g.DrawLine(pen, x, startY, x, endY);
        }

        void drawThreadDestroy(Graphics g, MethodData d, float startY, float endY)
        {
            String threadID = d.methodEvent.ThreadID;

            float x = d.getColumnLeftX() + methodBodyWidth * zoomScale * 0.25f;

            Pen pen = new Pen(getThreadColor(threadID), 2 * zoomScale);
            Brush brush = new SolidBrush(getThreadColor(threadID));

            float[] dashValues = { 4, 1 };
            pen.DashPattern = dashValues;

            float arrowWidth = 5 * zoomScale;

            g.DrawLine(pen, x, startY, x, endY);
            g.DrawLine(pen, 0, endY, x, endY);

            g.DrawLine(pen, 0, endY, arrowWidth, endY - arrowWidth);
            g.DrawLine(pen, 0, endY, arrowWidth, endY + arrowWidth);
            
        }

        void drawException(Graphics g, MethodData d, float y)
        {
            Brush pen = new SolidBrush(getThreadColor(d.methodEvent.ThreadID));

            float x = d.getColumnRightX() + 5;

            g.TranslateTransform(x, y);

            GraphicsState s = g.Save();

            g.ScaleTransform(zoomScale, zoomScale);

            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddLine( 7, 0, 15, 0);
            gfxPath.AddLine(15, 0,  8, 7);
            gfxPath.AddLine( 7, 8, 12, 8);
            gfxPath.AddLine(12, 8,  0, 16);
            gfxPath.AddLine( 0, 16,  4, 9);
            gfxPath.AddLine( 4, 9,  0, 9);

            gfxPath.CloseFigure();
            g.FillPath(pen, gfxPath);
            gfxPath.Dispose();

            g.Restore(s);

        }

        void drawMethodEnter(Graphics g, MethodData d, float startY)
        {
            Pen pen = new Pen(getThreadColor(d.methodEvent.ThreadID));

            float leftX = d.getColumnLeftX();
            float rightX = d.getColumnRightX();

            g.DrawLine(pen, leftX, startY, rightX, startY);

            if (d.callerMethodData != null) {
                drawStartArrow(g, d, d.callerMethodData, startY, d.methodEvent.MethodInfo.MethodName + "()");
            }
        }

        void drawMethodLeave(Graphics g, MethodData d, float startY)
        {

            Pen pen = new Pen(getThreadColor(d.methodEvent.ThreadID));

            float leftX = d.getColumnLeftX();
            float rightX = d.getColumnRightX();

            g.DrawLine(pen, leftX, startY, rightX, startY);

            if (d.callerMethodData != null)
            {
                drawReturnArrow(g, d, d.callerMethodData, startY);
            }
        }


        void drawStartArrow(Graphics g, MethodData d1, MethodData d2, float y, String title)
        {
            Pen p = new Pen(getThreadColor(d1.methodEvent.ThreadID), 2 * zoomScale);


            if (d1.classData == d2.classData)
            {
                float arrowWidth = 5 * zoomScale;

                g.DrawLine(p, d1.getColumnRightX(), y + 8 * zoomScale, d2.getColumnRightX() + 20 * zoomScale, y + 8 * zoomScale);
                g.DrawLine(p, d2.getColumnRightX() + 20 * zoomScale, y + 8 * zoomScale, d2.getColumnRightX() + 20 * zoomScale, y - 8 * zoomScale);
                g.DrawLine(p, d2.getColumnRightX() + 20 * zoomScale, y - 8 * zoomScale, d2.getColumnRightX(), y - 8 * zoomScale);

                g.DrawLine(p, d1.getColumnRightX() + arrowWidth, y + 3 * zoomScale, d1.getColumnRightX(), y + 8 * zoomScale);
                g.DrawLine(p, d1.getColumnRightX() + arrowWidth, y + 13 * zoomScale, d1.getColumnRightX(), y + 8 * zoomScale);

                g.DrawString(title, new Font("Tahoma", 7 * zoomScale), Brushes.Black, d2.getColumnRightX() + 5 * zoomScale, y - 23 * zoomScale);

                return;
            }
            else
            {
                y += 5 * zoomScale;

                float x2 = d1.getColumnLeftX();
                float x1 = d2.getColumnRightX();

                g.DrawLine(p, x1, y, x2, y);

                float arrowWidth = 5 * zoomScale;

                if (x1 < x2)
                {
                    g.DrawLine(p, x2 - arrowWidth, y - arrowWidth, x2, y);
                    g.DrawLine(p, x2 - arrowWidth, y + arrowWidth, x2, y);
                    g.DrawString(title, new Font("Tahoma", 7 * zoomScale), Brushes.Black, x1 + arrowWidth, y - 15 * zoomScale);
                }
                else
                {
                    g.DrawLine(p, x2 + arrowWidth, y - arrowWidth, x2, y);
                    g.DrawLine(p, x2 + arrowWidth, y + arrowWidth, x2, y);
                    g.DrawString(title, new Font("Tahoma", 7 * zoomScale), Brushes.Black, x1 - arrowWidth, y - 15 * zoomScale);
                }
            }
            
        }


        void drawReturnArrow(Graphics g, MethodData d1, MethodData d2, float y)
        {

            if (d1.classData == d2.classData) {
                return;
            }


            y -= 5 * zoomScale;
            Pen p = new Pen(getThreadColor(d1.methodEvent.ThreadID), 2 * zoomScale);

            float x1 = d1.getColumnLeftX();
            float x2 = d2.getColumnRightX();

            float[] dashValues = { 4, 1 };
            p.DashPattern = dashValues;
            
            g.DrawLine(p, x1, y, x2, y);

            float arrowWidth = 5 * zoomScale;

            if (x1 < x2)
            {
                g.DrawLine(p, x2 - arrowWidth, y - arrowWidth, x2, y);
                g.DrawLine(p, x2 - arrowWidth, y + arrowWidth, x2, y);
            }
            else
            {
                g.DrawLine(p, x2 + arrowWidth, y - arrowWidth, x2, y);
                g.DrawLine(p, x2 + arrowWidth, y + arrowWidth, x2, y);
            }
        }
        
        
        public void RoundedCornerRectangle(Graphics gfxObj, Pen penObj, float X, float Y, float RectWidth, float RectHeight, float CornerRadius)
        {
            GraphicsPath gfxPath = new GraphicsPath();
            gfxPath.AddLine(X + CornerRadius, Y, X + RectWidth - (CornerRadius * 2), Y);
            gfxPath.AddArc(X + RectWidth - (CornerRadius * 2), Y, CornerRadius * 2, CornerRadius * 2, 270, 90);
            gfxPath.AddLine(X + RectWidth, Y + CornerRadius, X + RectWidth, Y + RectHeight - (CornerRadius * 2));
            gfxPath.AddArc(X + RectWidth - (CornerRadius * 2), Y + RectHeight - (CornerRadius * 2), CornerRadius * 2, CornerRadius * 2, 0, 90);
            gfxPath.AddLine(X + RectWidth - (CornerRadius * 2), Y + RectHeight, X + CornerRadius, Y + RectHeight);
            gfxPath.AddArc(X, Y + RectHeight - (CornerRadius * 2), CornerRadius * 2, CornerRadius * 2, 90, 90);
            gfxPath.AddLine(X, Y + RectHeight - (CornerRadius * 2), X, Y + CornerRadius);
            gfxPath.AddArc(X, Y, CornerRadius * 2, CornerRadius * 2, 180, 90);
            gfxPath.CloseFigure();
            gfxObj.DrawPath(penObj, gfxPath);
            gfxPath.Dispose();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // UMLGraph
            // 
            this.Name = "UMLGraph";
            this.ResumeLayout(false);

        }

    }

}
