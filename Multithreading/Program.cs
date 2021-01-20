using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Multithreading
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"main thread id is {Thread.CurrentThread.ManagedThreadId}");
            CancellationTokenSource cts = new CancellationTokenSource();
            //ThreadPool.QueueUserWorkItem(CallBackWorkItem);
            ThreadPool.QueueUserWorkItem(CallBackWorkItem, cts.Token);
            Console.WriteLine("press enter to cancel operation");
            Console.Read();
            cts.Cancel();

            Console.WriteLine("detect press enter");
            Console.ReadKey();
        }

        private static void CallBackWorkItem(object state)
        {
            CancellationToken token = (CancellationToken)token;
            Console.WriteLine("start to count");
            Count(token, 1000);
        }

        private static void Count(CancellationToken token, int countto)
        {
            for (int i = 0; i < countto; i++)
            {
                if (token.IsCancellationRequested)
                {
                    Console.WriteLine("end count");
                    return;
                }
                else
                {
                    Console.WriteLine("count number is " + i);
                    Thread.Sleep(300);
                }
            }
            Console.WriteLine("finish count");
        }
    }
}
