using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace TransformationOperators
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

        private static void delay()
        {
            var sequence = from number in Enumerable.Range(1, 5) select slow(number);
            Console.WriteLine("Starting");
            sequence
                .ToObservable()
                .SubscribeOn(NewThreadScheduler.Default)
                .ObserveOn(Scheduler.CurrentThread)
                .Delay(TimeSpan.FromSeconds(2))
                .Subscribe(Console.WriteLine);
            Console.WriteLine("Ending");
        }

        private static void map()
        {
            var sequence = from number in Enumerable.Range(1, 5) select slow(number);
            Console.WriteLine("Starting");
            sequence
                .ToObservable()
                .SubscribeOn(NewThreadScheduler.Default)
                .ObserveOn(Scheduler.CurrentThread)
                .Select(x => x * 10)
                .Subscribe(Console.WriteLine);
            Console.WriteLine("Ending");
        }

        private static void scan()
        {
            var sequence = from number in Enumerable.Range(1, 5) select slow(number);
            Console.WriteLine("Starting");
            sequence
                .ToObservable()
                .SubscribeOn(NewThreadScheduler.Default)
                //.ObserveOn(Scheduler.CurrentThread)
                .Scan((acc, value) => acc + value)
                .Subscribe(Console.WriteLine);
            Console.WriteLine("Ending");
        }

        private static void throttle()
        {
            var sequence = from number in Enumerable.Range(1, 10) select slow(number);
            Console.WriteLine("Starting");
            sequence
                .ToObservable()
                .SubscribeOn(ThreadPoolScheduler.Instance)
                .ObserveOn(Scheduler.CurrentThread)
                //.Throttle(TimeSpan.FromMilliseconds(200))
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Subscribe(Console.WriteLine, Console.WriteLine,
                    () => Console.WriteLine("Stream Completed"));

            Console.WriteLine("Ending");
        }

        private static void amb()
        {
            //var sequence1 = (from number in Enumerable.Range(1, 5) select slow(number, 1000)).ToObservable(Scheduler.NewThread);
            //var sequence2 = (from number in Enumerable.Range(10, 5) select slow(number, 2000)).ToObservable(Scheduler.NewThread);
            //var sequence3 = (from number in Enumerable.Range(100, 5) select slow(number, 200)).ToObservable(Scheduler.NewThread);

            var sequence1 = (from number in Enumerable.Range(1, 5) select slow(number, 1000)).ToObservable(NewThreadScheduler.Default);
            var sequence2 = (from number in Enumerable.Range(10, 5) select slow(number, 2000)).ToObservable(NewThreadScheduler.Default);
            var sequence3 = (from number in Enumerable.Range(100, 5) select slow(number, 200)).ToObservable(NewThreadScheduler.Default);

            //var sequence1 = (from number in Enumerable.Range(1, 5) select slow(number, 1000)).ToObservable(ThreadPoolScheduler.Instance);
            //var sequence2 = (from number in Enumerable.Range(10, 5) select slow(number, 2000)).ToObservable(ThreadPoolScheduler.Instance);
            //var sequence3 = (from number in Enumerable.Range(100, 5) select slow(number, 200)).ToObservable(ThreadPoolScheduler.Instance);
            Console.WriteLine("Starting");

            var racer = sequence2.Amb(sequence1).Amb(sequence3);

            racer.Subscribe(x => Console.WriteLine("{0} -> Thread ID - {1} -> Is Background - {2} -> Is ThreadPoolThread - {3}", 
                x, Thread.CurrentThread.ManagedThreadId, Thread.CurrentThread.IsBackground, Thread.CurrentThread.IsThreadPoolThread));

            Console.WriteLine("Ending");
        }
    }
}
