using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Demo
{
    class UMLGraph : UserControl
    {

        public UMLGraph()
        {

        }

        public void setEvents(ArrayList eventsLog)
        {
            events = eventsLog;
            this.Invalidate();
        }
        /*
        protected override void OnPaint1(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.ScaleTransform(0.1f, 0.01f);
            e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, Width, Height));
            
            for (int i = 0; i < 30; i++) {
                e.Graphics.DrawLine(new Pen(Color.Gray), padding + i * (colWidth + colInterspace), padding, padding + i * (colWidth + colInterspace), 1000);
                e.Graphics.DrawLine(new Pen(Color.Gray), padding + i * (colWidth + colInterspace) + colWidth, padding, padding + i * (colWidth + colInterspace) + colWidth, 1000);
            }
            
            if (events.Count == 0) return;

            double minDelta = -1;
            double startTime = ((LogEvent)events[0]).timestamp;

            for (int i=1; i<events.Count; i++) {
                double t1 = ((LogEvent) events[i - 1]).timestamp;
                double t2 = ((LogEvent) events[i]).timestamp;
                double delta = t2 - t1;
                if (minDelta == -1) minDelta = delta;
                minDelta = Math.Min(minDelta, delta);
            }

            float scaleY = 2.0f * minimumMethodHeight / (float)minDelta;

            foreach (LogEvent ev in events) {
                if (ev is MethodEvent) {
                    MethodEvent mev = (MethodEvent) ev;
                    String hash;

                    if (!mev.MethodInfo.IsStatic)
                        hash = mev.InstanceObjectID.ToString();
                    else
                        hash = mev.MethodInfo.ClassName;


                    bool drawHeader = false;
                    if (!columns.ContainsKey(hash))
                    {
                        // XXX Creo una nuova colonna
                        ColumnData c = new ColumnData();
                        c.column = nextFreeColumn++;
                        this.columns[hash] = c;
                        drawHeader = true;
                    }

                    ColumnData col = (ColumnData)this.columns[hash];

                    float eventY = padding + headerHeight + headerInterspaceHeight + (float) (mev.timestamp - startTime) * scaleY;
                    float eventX = padding + (colWidth + colInterspace) * col.column;

                    if (drawHeader) {
                        if (!mev.MethodInfo.IsStatic)
                        {
                            this.drawInstanceHeader(e.Graphics, eventX, eventY - headerHeight - headerInterspaceHeight, mev.MethodInfo.ClassName + " (" + mev.InstanceObjectID.ToString() + ")", false);
                        }
                        else {
                            this.drawInstanceHeader(e.Graphics, eventX, eventY - headerHeight - headerInterspaceHeight, mev.MethodInfo.ClassName, true);
                        }
                    }
                    
                    if (mev.EventType == MethodEvent.EventTypeEnum.EnterEvent) 
                    {
                        col.calledBy = this.lastColumnData;
                        this.lastColumnData = col;
                        col.openedAtY = eventY;
                    }
                    else if (mev.EventType == MethodEvent.EventTypeEnum.LeaveEvent)
                    {
                        this.lastColumnData = col.calledBy;

                        this.drawMethod(e.Graphics, eventX + (colWidth - methodBodyWidth) * 0.5f, col.openedAtY, methodBodyWidth, eventY - col.openedAtY);

                        if (col.calledBy != null)
                        {
                            float arrowStartX = padding + (colWidth + colInterspace) * col.calledBy.column + (colWidth - methodBodyWidth) * 0.5f + methodBodyWidth;
                            float arrowEndX = eventX + (colWidth - methodBodyWidth) * 0.5f;
                            this.drawStartArrow(e.Graphics, arrowStartX, arrowEndX, col.openedAtY + colInterspace * 0.5f, mev.MethodInfo.MethodName);
                            this.drawReturnArrow(e.Graphics, arrowEndX, arrowStartX, eventY - colInterspace * 0.5f);
                        }

                    }
                    
                }
            }
        }
        
        
        
        private Hashtable columns = new Hashtable();
        
        private int minimumMethodHeight = 60;
        private int maximumStaticHeight = 200;
        private Pen currentThreadPen = new Pen(Color.Red);
        private int headerHeight = 40;
        
        private ColumnData lastColumnData = null;

        */





        private Color[] threadColorLibrary = { Color.Red, Color.Blue, Color.Green, Color.Orange, Color.Pink };
        private Color backgroundColor = Color.White;
        private int nextFreeThreadColor = 0;

        static private int padding = 30;
        static private int colWidth = 150;
        static private int methodBodyWidth = 20;
        static private int colInterspace = 20;
        static private int headerInterspaceHeight = 20;

        private int nextFreeColumn = 0;
        private float zoomScale = 1.0f;

        private Hashtable classData = new Hashtable();
        private Hashtable lastMethodData = new Hashtable();
        private Hashtable threadColor = new Hashtable();
        private ArrayList events = new ArrayList();

        class ClassData {
            public int columnNumber;
            public MethodInfo methodInfo;
            public String instanceObjectID;

            public ArrayList methodData = new ArrayList();

            public float getColumnLeftX()
            {
                return columnNumber * (colWidth + colInterspace);
            }
        }

        class MethodData
        {
            public int methodColumnNumber;
            public MethodEvent methodEvent;
            public MethodData callerMethodData;
            public ClassData classData;

            public float getColumnLeftX() {
                return (colWidth - methodBodyWidth) * 0.5f + classData.getColumnLeftX() + (methodBodyWidth * 0.5f) * methodColumnNumber;
            }

            public float getColumnRightX()
            {
                return getColumnLeftX() + methodBodyWidth;
            }
        }


        protected override void OnPaint(PaintEventArgs eva)
        {
            base.OnPaint(eva);

            classData = new Hashtable();
            lastMethodData = new Hashtable();
            threadColor = new Hashtable();
            nextFreeThreadColor = 0;
            nextFreeColumn = 0;

            Graphics g = eva.Graphics;

            g.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, Width, Height));

            if (events.Count == 0) {
                return;
            }
            
            g.ScaleTransform(zoomScale, zoomScale);
            g.TranslateTransform(padding, padding + 40);

            double lastTimestamp = ((LogEvent)events[0]).timestamp;
            float lastY = 0;
            double minimumDeltaTime = getMinimumDeltaTime();

            foreach (LogEvent logEvent in events)
            {

                double deltaTime = logEvent.timestamp - lastTimestamp;
                float deltaY = (float) Math.Min( 20 * (deltaTime / minimumDeltaTime), 30);

                Console.WriteLine("Timestamp: " + logEvent.timestamp + " Delta: " + deltaTime);

                foreach (DictionaryEntry de in classData) {
                    ClassData d = (ClassData) de.Value;

                    foreach (MethodData m in d.methodData)
                    {
                        Console.WriteLine("Disegno " + m.methodEvent.getHash() + " da " + lastY + " a " + (lastY + deltaY));
                        drawMethodData(g, m, lastY, lastY + deltaY);
                    }
                }

                if (logEvent is MethodEvent)
                {
                    MethodEvent e = (MethodEvent) logEvent;

                    String currentThreadID = e.ThreadID;
                    bool mustDrawHeader = !classData.ContainsKey(e.getHash());
                    ClassData currentClassData = getOrCreateClassData(e);

                    if (mustDrawHeader) {
                        Console.WriteLine("Disegno header " + lastY);
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

                        drawMethodEnter(g, currentMethodData, lastY + deltaY);

                        Console.WriteLine("Aggiungo metodo " + e.getHash());
                    }
                    else if (e.EventType == MethodEvent.EventTypeEnum.LeaveEvent) {

                        MethodData currentMethodData = (MethodData) currentClassData.methodData[currentClassData.methodData.Count - 1];
                        lastMethodData[currentThreadID] = currentMethodData.callerMethodData;
                        currentClassData.methodData.RemoveAt(currentClassData.methodData.Count - 1);

                        drawMethodLeave(g, currentMethodData, lastY + deltaY);

                        Console.WriteLine("Rimuovo metodo " + e.getHash());
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

        double getMinimumDeltaTime()
        {

            if (events.Count == 0) return 0;

            double minDelta = -1;
            double startTime = ((LogEvent)events[0]).timestamp;
            for (int i = 1; i < events.Count; i++)
            {
                double t1 = ((LogEvent)events[i - 1]).timestamp;
                double t2 = ((LogEvent)events[i]).timestamp;
                double delta = t2 - t1;
                if (minDelta == -1) minDelta = delta;
                minDelta = Math.Min(minDelta, delta);
            }

            return minDelta;
        }

        void drawHeader(Graphics g, MethodEvent e, float x, float y)
        {
            String title = e.MethodInfo.ClassName;
            Pen blackBorderPen = new Pen(Color.Black);
            Font textFont = new Font("Tahoma", 8);

            if (!e.MethodInfo.IsStatic)
                title += "\nInst. " + e.InstanceObjectID.ToString();

            StringFormat f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Center;

            SizeF stringSize = new SizeF();
            stringSize = g.MeasureString(title, textFont, new SizeF(colWidth, 50));

            int boxHeight = (int) (stringSize.Height + 20);

            if (e.MethodInfo.IsStatic)
            {
                g.DrawRectangle(blackBorderPen, x, y - boxHeight - 20, colWidth, boxHeight);
            }
            else
            {
                RoundedCornerRectangle(g, blackBorderPen, x, y - boxHeight - 20, colWidth, boxHeight, 10);
            }

            g.DrawString(title, textFont, Brushes.Black, x + colWidth * 0.5f, y + 10 + stringSize.Height * 0.5f - boxHeight - 20, f); 
        }

        Color getThreadColor(MethodEvent e) {
            if (!threadColor.ContainsKey(e.ThreadID)) {
                threadColor[e.ThreadID] = threadColorLibrary[nextFreeThreadColor++];
                if (nextFreeThreadColor == threadColorLibrary.Length) nextFreeThreadColor = 0;
            }

            return (Color) threadColor[e.ThreadID];
        }

        void drawMethodData(Graphics g, MethodData d, float startY, float endY)
        {
            Pen pen = new Pen(getThreadColor(d.methodEvent));

            float leftX = d.getColumnLeftX();
            float rightX = d.getColumnRightX();

            //g.DrawRectangle(pen, leftX, startY, rightX - leftX, endY - startY);
            g.DrawLine(pen, leftX, startY, leftX, endY);
            g.DrawLine(pen, rightX, startY, rightX, endY);
        }

        void drawMethodEnter(Graphics g, MethodData d, float startY)
        {
            Pen pen = new Pen(getThreadColor(d.methodEvent));

            float leftX = d.getColumnLeftX();
            float rightX = d.getColumnRightX();

            g.DrawLine(pen, leftX, startY, rightX, startY);

            if (d.callerMethodData != null) {
                drawStartArrow(g, d, d.callerMethodData, startY, d.methodEvent.MethodInfo.MethodName + "()");
            }
        }

        void drawMethodLeave(Graphics g, MethodData d, float startY)
        {

            Pen pen = new Pen(getThreadColor(d.methodEvent));

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
            Pen p = new Pen(getThreadColor(d1.methodEvent), 2);

            if (d1.classData == d2.classData)
            {

                g.DrawLine(p, d1.getColumnRightX(), y + 8, d2.getColumnRightX() + 20, y + 8);
                g.DrawLine(p, d2.getColumnRightX() + 20, y + 8, d2.getColumnRightX() + 20, y - 8);
                g.DrawLine(p, d2.getColumnRightX() + 20, y - 8, d2.getColumnRightX(), y - 8);

                g.DrawLine(p, d1.getColumnRightX() + 5, y + 3, d1.getColumnRightX(), y + 8);
                g.DrawLine(p, d1.getColumnRightX() + 5, y + 13, d1.getColumnRightX(), y + 8);

                g.DrawString(title, new Font("Tahoma", 7), Brushes.Black, d2.getColumnRightX() + 5, y - 23);

                return;
            }
            else
            {
                y += 5;

                float x2 = d1.getColumnLeftX();
                float x1 = d2.getColumnRightX();

                g.DrawLine(p, x1, y, x2, y);

                if (x1 < x2)
                {
                    g.DrawLine(p, x2 - 5, y - 5, x2, y);
                    g.DrawLine(p, x2 - 5, y + 5, x2, y);
                    g.DrawString(title, new Font("Tahoma", 7), Brushes.Black, x1 + 5, y - 15);
                }
                else
                {
                    g.DrawLine(p, x2 + 5, y - 5, x2, y);
                    g.DrawLine(p, x2 + 5, y + 5, x2, y);
                    g.DrawString(title, new Font("Tahoma", 7), Brushes.Black, x1 - 5, y - 15);
                }
            }
            
        }


        void drawReturnArrow(Graphics g, MethodData d1, MethodData d2, float y)
        {

            if (d1.classData == d2.classData) {
                return;
            }

            y -= 5;
            Pen p = new Pen(getThreadColor(d1.methodEvent), 2);

            float x1 = d1.getColumnLeftX();
            float x2 = d2.getColumnRightX();

            float[] dashValues = { 4, 1 };
            p.DashPattern = dashValues;

            g.DrawLine(p, x1, y, x2, y);
            if (x1 < x2)
            {
                g.DrawLine(p, x2 - 5, y - 5, x2, y);
                g.DrawLine(p, x2 - 5, y + 5, x2, y);
            }
            else
            {
                g.DrawLine(p, x2 + 5, y - 5, x2, y);
                g.DrawLine(p, x2 + 5, y + 5, x2, y);
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

    }

}
