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

        class ColumnData {
            public int column;
            public float openedAtY;
        }

        private int nextFreeColumn = 0;
        private ArrayList events = new ArrayList();
        private Hashtable columns = new Hashtable();
        private int padding = 30;
        private int colWidth = 90;
        private int methodBodyWidth = 20;
        private int colInterspace = 40;
        private Pen currentThreadPen = new Pen(Color.Red);
        private int headerHeight = 40;
        private int headerInterspaceHeight = 20;

        public UMLGraph()
        {

        }

        public void setEvents(ArrayList eventsLog)
        {
            events = eventsLog;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(0, 0, Width, Height));

            for (int i = 0; i < 30; i++) {
                e.Graphics.DrawLine(new Pen(Color.Gray), padding + i * (colWidth + colInterspace), padding, padding + i * (colWidth + colInterspace), 1000);
                e.Graphics.DrawLine(new Pen(Color.Gray), padding + i * (colWidth + colInterspace) + colWidth, padding, padding + i * (colWidth + colInterspace) + colWidth, 1000);
            }

            if (events.Count == 0) return;

            long minDelta = -1;
            long startTime = ((LogEvent)events[0]).timestamp.Ticks;

            for (int i=1; i<events.Count; i++) {
                long t1 = ((LogEvent)events[i-1]).timestamp.Ticks;
                long t2 = ((LogEvent)events[i]).timestamp.Ticks;
                long delta = t2 - t1;
                if (minDelta == -1) minDelta = delta;
                minDelta = Math.Min(minDelta, delta);
            }

            float scaleY = (float) colInterspace / minDelta;

            foreach (LogEvent ev in events) {
                if (ev is MethodEvent) {
                    MethodEvent mev = (MethodEvent) ev;
                    String hash;

                    if (mev.InstanceObjectID != 0)
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

                    float eventY = padding + headerHeight + headerInterspaceHeight + (mev.timestamp.Ticks - startTime) * scaleY;
                    float eventX = padding + (colWidth + colInterspace) * col.column;

                    if (drawHeader) {
                        if (mev.InstanceObjectID != 0)
                        {
                            this.drawInstanceHeader(e.Graphics, eventX, eventY - headerHeight - headerInterspaceHeight, mev.MethodInfo.ClassName + " (" + mev.InstanceObjectID.ToString() + ")", false);
                        }
                        else {
                            this.drawInstanceHeader(e.Graphics, eventX, eventY - headerHeight - headerInterspaceHeight, mev.MethodInfo.ClassName, true);
                        }
                    }
                    
                    if (mev.EventType == MethodEvent.EventTypeEnum.EnterEvent) 
                    {
                        col.openedAtY = eventY;
                    }
                    else if (mev.EventType == MethodEvent.EventTypeEnum.LeaveEvent)
                    {
                        this.drawMethod(e.Graphics, eventX + (colWidth - methodBodyWidth) * 0.5f, col.openedAtY, methodBodyWidth, eventY - col.openedAtY);
                    }

                }
            }
        }

        void drawInstanceHeader(Graphics g, float x, float y, String title, bool isStatic) {
            if (isStatic) {
                g.DrawRectangle(new Pen(Color.Black), x, y, colWidth, headerHeight);
            } else {
                RoundedCornerRectangle(g, new Pen(Color.Black), x, y, colWidth, headerHeight, 10);
            }

            StringFormat f = new StringFormat();
            f.Alignment = StringAlignment.Center;
            f.LineAlignment = StringAlignment.Center;

            g.DrawString(title, new Font("Tahoma", 8), Brushes.Black, x + colWidth * 0.5f, y + (headerHeight - 8) * 0.5f, f); 
        }

        void drawMethod(Graphics g, float x, float y, float width, float height)
        {

            g.DrawRectangle(currentThreadPen, x, y, width, height);

            for (int i = 0; i < height + width; i += 3) {
                float fx = x, fy = y + i;
                float lx, ly;
                if (i < width)
                {
                    lx = x + i;
                    ly = y;
                }
                else {
                    lx = x + width;
                    ly = y + i - width;
                }

                if (fy > y + height) {
                    fx = fx + (fy - y - height);
                    fy = y + height;
                }

                g.DrawLine(currentThreadPen, fx, fy, lx, ly);
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
