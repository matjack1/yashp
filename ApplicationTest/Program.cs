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
            Test2 fantasy = new Test2();

            for (int i=0; i<10; i++)
                t.setX(10);

            Test t2 = new Test();

            for (int i = 0; i < 10; i++)
            {
                t2.setX(10);
            }

            Test2.sumInt(2, 4);
            fantasy.almostSum(2, 4);
            try
            {
                throw new Exception("TEST");
            }
            catch
            {

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

    class Test2
    {
        public static int sumInt(int x, int y)
        {
            return x + y;
        }

        public int almostSum(int x, int y)
        {
            Test2.sumInt(3, 5);
            return x + y + 4;
        }
    }
}
