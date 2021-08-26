using System;
using System.Diagnostics;

namespace ThreadHelperExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Press any key to run test");
            Console.ReadKey();
            Console.Clear();

            ThreadTest();

            Console.ReadKey();
        }

        static async void ThreadTest()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Foo foo1 = new Foo();
            Foo foo2 = new Foo();
            Foo foo3 = new Foo();

            await foo1.Bar();
            Console.WriteLine("Bar1 has finished");
            await foo2.Bar();
            Console.WriteLine("Bar2 has finished");
            await foo3.Bar();
            Console.WriteLine("Bar3 has finished");

            stopWatch.Stop();

            Console.WriteLine("Test took " + stopWatch.Elapsed + " to complete");
        }
    }
}
