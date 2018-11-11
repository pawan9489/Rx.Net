using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading;

namespace FilterOperations
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


        private static void filter()
        {
            var sequence1 = (from number in Enumerable.Range(1, 5) select slow(number, 1000)).ToObservable(NewThreadScheduler.Default);
            sequence1 = sequence1.Where(x => x % 2 == 0);
            sequence1.Subscribe(Console.WriteLine);
        }

        private static void distinct()
        {
            var sequence1 = (from number in randoms(20) select slow(number, 1000))
                            .ToObservable(ThreadPoolScheduler.Instance);
            var sequence2 = sequence1
                            .Do(n => Console.WriteLine("Value Sending to Sequence 2 -> {0}", n))
                            .Distinct();
            //sequence1.Subscribe(n => Console.WriteLine("Sequence 1 -> {0}", n));
            sequence2.Subscribe(n => Console.WriteLine("\t\t\t\tSequence 2 -> {0}", n));
        }

        private static void distinctUntilChanged()
        {
            var sequence1 = (from number in randoms(20) select slow(number, 1000))
                            .ToObservable(ThreadPoolScheduler.Instance);
            var sequence2 = sequence1
                            .Do(n => Console.WriteLine("Value Sending to Sequence 2 -> {0}", n))
                            .DistinctUntilChanged();
            //sequence1.Subscribe(n => Console.WriteLine("Sequence 1 -> {0}", n));
            sequence2.Subscribe(n => Console.WriteLine("\t\t\t\tSequence 2 -> {0}", n));
        }

        private static void elementAt()
        {
            var sequence1 = (from number in Enumerable.Range(1, 5) select slow(number, 1000)).ToObservable(NewThreadScheduler.Default);
            sequence1 = sequence1.ElementAt(2);
            sequence1.Subscribe(Console.WriteLine);
        }

        private static void skip()
        {
            var sequence1 = (from number in Enumerable.Range(1, 5) select slow(number, 1000)).ToObservable(NewThreadScheduler.Default);
            sequence1 = sequence1.Skip(TimeSpan.FromSeconds(3.2));
            //sequence1 = sequence1.Skip(2);
            sequence1.Subscribe(Console.WriteLine);
        }

        private static void take()
        {
            var sequence1 = (from number in Enumerable.Range(1, 5) select slow(number, 1000)).ToObservable(NewThreadScheduler.Default);
            sequence1 = sequence1.Take(TimeSpan.FromSeconds(3.4));
            //sequence1 = sequence1.Take(3);
            sequence1.Subscribe(Console.WriteLine, Console.WriteLine, () => Console.WriteLine("Completed"));
        }
    }
}
