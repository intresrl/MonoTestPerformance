using System;
using System.Collections.Generic;

using System.Threading;
using System.Windows.Forms;

namespace TestPerformance
{
    class MainClass
    {

        public static void Main(string[] args)
        {

            if (args.Length != 1)
            {
                usage();
                Application.Exit();
                return;
            }

            if (args[0].StartsWith("cpu"))
            {
                int threads = 2;
                if (args[0].Split(':').Length > 1 )
                {
                    if (!int.TryParse(args[0].Split(':')[1], out threads)) threads = 2;
                }
                Console.WriteLine("Starting CPU test with {0}: ", threads);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                testPerformance(threads);
                var elapsed = watch.ElapsedMilliseconds;
                Console.WriteLine("Elapsed: " + elapsed);
            } else if (args[0].StartsWith("form"))
            {
                int forms = 100;
                Console.WriteLine("Starting form test with {0}: ", forms);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                testWindowsForm(forms);
                var elapsed = watch.ElapsedMilliseconds;
                Console.WriteLine("Elapsed: " + elapsed);
            }
            
            
        }

        private static void usage()
        {
            Console.WriteLine("Usage: ");
            Console.WriteLine("");
            Console.WriteLine("program.exe cpu");
            Console.WriteLine("program.exe cpu:num_threads");
            Console.WriteLine("program.exe form");
        }

        private static Random rand = new Random();
        static List<Thread> threads = new List<Thread>();
        static List<WaitHandle> waitHandles = new List<WaitHandle>();
        private static int f = 0;
        private static void testWindowsForm(int forms)
        {
            Application.Run(new Form1(forms));
        }


        private static void testPerformance(int threads_num)
        {
            int i = 0;
            while (i < threads_num)
            {
                var handle = new EventWaitHandle(false, EventResetMode.ManualReset);
                var thread = new Thread(new ParameterizedThreadStart(Run));
                thread.Start(handle);
                threads.Add(thread);
                waitHandles.Add(handle);
                i++;
            }
            
            WaitHandle.WaitAll(waitHandles.ToArray());
        }

        private static void Run(object handler)
        {
            long num = 0;
            string r = "";
            while (num < 100000)
            {
                var rnd = rand.Next(100, 1000);
                r += rnd.ToString();
                num++;
                if (num % 10000 == 0)
                {
                    r = "";
                    Console.WriteLine("Progress: " + num);
                    
                }
            }

            ((EventWaitHandle) handler).Set();
        }

    }
}
