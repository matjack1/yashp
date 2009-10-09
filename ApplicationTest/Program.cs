using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ApplicationTest
{
    class Test
    {
        static void Main()
        {
            Test t = new Test();

            for (int i=0; i<10; i++)
                t.setX(10);

            Test t2 = new Test();

            for (int i = 0; i < 10; i++)
            {
                t2.setX(10);
            }
        }

        void setX(int x) {
            this.x = x;
        }

        int getX() {
            return this.x;
        }

        private int x;
    }
}
