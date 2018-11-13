using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace SequenceCreation
{
    partial class Program
    {
        private static int slow(int x)
        {
            Thread.Sleep(1000);
            return x;
        }

        private static int slow(int x, int time)
        {
            Thread.Sleep(time);
            return x;
        }

        private static int slowX(int x)
        {
            Thread.Sleep(1000 * x);
            return x;
        }

        private static Random r = new Random();
        private static IEnumerable<int> randoms(int n)
        {
            if (n < 0)
                throw new Exception("Please provide +ve Values.");
            while (n > 0)
            {
                n--;
                yield return r.Next(1, 5);
            }
        }


        private static void create()
        {
            var observable = Observable.Create<DateTime>(observer =>
            {
                Console.WriteLine("Registering subscriber {0} of type {1}, {2}",
                        observer.GetHashCode(), observer, Thread.CurrentThread.ManagedThreadId);
                Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("{0}", Thread.CurrentThread.ManagedThreadId);
                    for (int i = 0; i < 5; i++)
                    {
                        observer.OnNext(DateTime.Now);
                        Thread.Sleep(1000);
                    }
                    observer.OnCompleted();
                });
                return () => Console.WriteLine("OnCompleted {0} of type {1}, {2}",
                        observer.GetHashCode(), observer, Thread.CurrentThread.ManagedThreadId);
            });
            observable.Subscribe(d => Console.WriteLine("On Next : {0}, {1}",
                d.ToLocalTime(), Thread.CurrentThread.ManagedThreadId), Console.WriteLine,
                () => Console.WriteLine("Original Completed set by the subscriber"));
        }

        private static void test()
        {
            var stream = (from n in Enumerable.Range(1, 5) select slow(n, 1000)).ToObservable(NewThreadScheduler.Default);

            for(int i = 0; i < 10000; i++)
            {
                int copy = i;
                stream.Subscribe(a =>
                {
                    Thread.Sleep(1000);
                    Console.WriteLine("Subscriber {0} {1}", copy, a);
                });
            }

            //stream.Subscribe(a =>
            //{
            //    Thread.Sleep(1000);
            //    Console.WriteLine("Subscriber 2 {0}", a);
            //});
        }

        private static void interval()
        {
            Observable.Interval(TimeSpan.FromSeconds(1))
                .Subscribe(a => Console.WriteLine("{0} - {1}", a, DateTime.Now));
        }

        private static void timer()
        {
            Observable.Timer(TimeSpan.FromSeconds(1))
                .Subscribe(a => Console.WriteLine("{0} - {1}", a, DateTime.Now));
        }

        private static void timeOut()
        {
            var a = new int[] { 2, 4, 2, 5, 2 };

            (from n in a select slowX(n)).ToObservable()
                .Timeout(TimeSpan.FromSeconds(4.5))
                .Subscribe(Console.WriteLine, 
                    err => Console.WriteLine("OOPS Error Occured: {0}", err.Message));
        }
    }
}
