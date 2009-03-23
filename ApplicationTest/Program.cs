using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ApplicationTest
{
    class ThreadTest
    {
        static void Main()
        {
            ThreadTest tr = new ThreadTest();

            Thread t = new Thread(WriteY);
            t.Start();                          // Run WriteY on the new thread
            for (int i = 0; i < 100; i++) tr.WriteX();   // Write 'x' forever
        }

        void WriteX() {
            Console.Write("x");
        }

        static void WriteY()
        {
            for (int i = 0; i < 100; i++) Console.Write("y");   // Write 'y' forever
        }
    }
}
