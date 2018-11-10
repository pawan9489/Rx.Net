using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subjects
{
    partial class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--------- Simple Subject ------");
            simpleSubject();
            Console.WriteLine();
            Console.WriteLine("--------- Replay Subject ------");
            replaySubject();
            Console.WriteLine();
            Console.WriteLine("--------- Behavior Subject ------");
            behaviourSubject();
            Console.WriteLine();
            Console.WriteLine("--------- Async Subject ------");
            asyncSubject();
            Console.WriteLine();
            Console.WriteLine("--------- Custom Subject ------");
            customSubject();
            Console.WriteLine("-------------------");
            Console.ReadLine();
        }
    }
}
