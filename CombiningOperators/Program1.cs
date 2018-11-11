using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reactive.Linq;
using System.Reactive.Concurrency;

namespace CombiningOperators
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

        private static void combineLatest()
        {
            var s1 = (from n in Enumerable.Range(1, 5) select slow(n, 1000)).ToObservable(NewThreadScheduler.Default);
            var s2 = (from n in Enumerable.Range(11, 5) select slow(n, 550)).ToObservable(NewThreadScheduler.Default);
            var s3 = (from n in Enumerable.Range(101, 5) select slow(n)).ToObservable(NewThreadScheduler.Default);

            var cLatest = s1.CombineLatest(s2, 
                (left, right) => string.Format("Left - {0}, Right - {1}", left, right))
                //.CombineLatest(s3, (left, right) => string.Format("Left - {0}, Right - {1}", left, right))
                ;

            cLatest.Subscribe(Console.WriteLine);
        }

        private static void concat()
        {
            var s1 = (from n in Enumerable.Range(1, 5) select slow(n, 1000)).ToObservable(NewThreadScheduler.Default);
            var s2 = (from n in Enumerable.Range(11, 5) select slow(n, 550)).ToObservable(NewThreadScheduler.Default);
            var s3 = (from n in Enumerable.Range(101, 5) select slow(n, 200)).ToObservable(NewThreadScheduler.Default);

            var concat = s1.Concat(s2).Concat(s3)
                ;

            concat.Subscribe(Console.WriteLine);
        }

        private static void merge()
        {
            var s1 = (from n in Enumerable.Range(1, 5) select slow(n, 1000)).ToObservable(NewThreadScheduler.Default);
            var s2 = (from n in Enumerable.Range(11, 5) select slow(n, 550)).ToObservable(NewThreadScheduler.Default);
            var s3 = (from n in Enumerable.Range(101, 5) select slow(n, 200)).ToObservable(NewThreadScheduler.Default);

            var concat = s1.Merge(s2).Merge(s3)
                ;

            concat.Subscribe(Console.WriteLine);
        }

        private static void sample()
        {
            var s1 = (from n in Enumerable.Range(1, 5) select slow(n, 1000)).ToObservable(NewThreadScheduler.Default);
            var s2 = (from n in Enumerable.Range(11, 7) select slow(n, 550)).ToObservable(NewThreadScheduler.Default);
            var s3 = (from n in Enumerable.Range(101, 10) select slow(n, 200)).ToObservable(NewThreadScheduler.Default);

            var concat = s3.Sample(s1);
                ;

            concat.Subscribe(Console.WriteLine);
        }

        private static void startsWith()
        {
            var s1 = (from n in Enumerable.Range(1, 5) select slow(n, 1000)).ToObservable(NewThreadScheduler.Default);
            var s2 = (from n in Enumerable.Range(11, 7) select slow(n, 550)).ToObservable(NewThreadScheduler.Default);
            var s3 = (from n in Enumerable.Range(101, 10) select slow(n, 200)).ToObservable(NewThreadScheduler.Default);

            var arr = new int[2] { -1, 0};
            var concat = s1.StartWith(arr)
            ;

            concat.Subscribe(Console.WriteLine);
        }

        private static void zip()
        {
            var s1 = (from n in Enumerable.Range(1, 5) select slow(n, 1000)).ToObservable(NewThreadScheduler.Default);
            var s2 = (from n in Enumerable.Range(11, 7) select slow(n, 550)).ToObservable(NewThreadScheduler.Default);
            var s3 = (from n in Enumerable.Range(101, 10) select slow(n, 200)).ToObservable(NewThreadScheduler.Default);

            var concat = s1.Zip(s2, (left, right) => string.Format("Left - {0}, Right - {1}", left, right))
            ;

            concat.Subscribe(Console.WriteLine);
        }
    }
}
