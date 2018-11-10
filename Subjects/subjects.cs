using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Subjects;

namespace Subjects
{
    partial class Program
    {
        private static void simpleSubject()
        {
            ISubject<int> s = new Subject<int>();
            Console.WriteLine("Adding values");
            s.OnNext(2);
            Console.WriteLine("Adding next value");
            s.OnNext(4);
            s.Subscribe(Console.WriteLine);
            s.OnNext(6);
        }

        private static void replaySubject()
        {
            ISubject<int> s = new ReplaySubject<int>();
            Console.WriteLine("Adding values");
            s.OnNext(2);
            Console.WriteLine("Adding next value");
            s.OnNext(4);
            s.Subscribe(Console.WriteLine);
            s.OnNext(6);
        }

        private static void behaviourSubject()
        {
            ISubject<int> s = new BehaviorSubject<int>(0);
            Console.WriteLine("Adding values");
            s.OnNext(2);
            Console.WriteLine("Adding next value");
            s.OnNext(4);
            s.Subscribe(Console.WriteLine);
            s.OnNext(6);
        }

        private static void asyncSubject()
        {
            ISubject<int> s = new AsyncSubject<int>();
            Console.WriteLine("Adding values");
            s.OnNext(2);
            Console.WriteLine("Adding next value");
            s.OnNext(4);
            s.Subscribe(Console.WriteLine, Console.WriteLine, 
                () => Console.WriteLine("Completed the Async Subject"));
            s.OnNext(6);
            s.OnCompleted();
        }

        private static void customSubject()
        {
            var receiver = new Subject<int>();
            receiver.Subscribe(x => Console.WriteLine("Receiver {0}", x));

            var sender = new Subject<int>();

            var router = Subject.Create<int>(receiver, sender);

            router.Subscribe(x => Console.WriteLine("Router {0}", x));

            sender.OnNext(2);
            sender.OnNext(20);
        }

    }
}
