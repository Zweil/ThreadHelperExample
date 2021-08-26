using System;
using System.Diagnostics;
using FuelStar.Helpers;

namespace ThreadHelperExampleComplete
{
    class Program
    {
        public const string FooTask2 = "foo2";

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

            ThreadHelper.Instance.AddTask(foo1.Bar1());
            ThreadHelper.Instance.AddTask(FooTask2, foo2.Bar());
            ThreadHelper.Instance.AddTask("foo3", foo3.Bar());

            //Uncomment to test task cancellation
            //ThreadHelper.Instance.CancelTask("foo3");

            if(await ThreadHelper.Instance.CheckTaskComplete<bool>(FooTask2))
                Console.WriteLine("Bar2 has finished");
            if (await ThreadHelper.Instance.CheckTaskComplete<bool>("foo3"))
                Console.WriteLine("Bar3 has finished");

            stopWatch.Stop();

            Console.WriteLine("Test took " + stopWatch.Elapsed + " to complete");
        }
    }
}