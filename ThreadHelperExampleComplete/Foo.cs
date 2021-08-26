using System;
using System.Threading.Tasks;
using FuelStar.Helpers;

namespace ThreadHelperExampleComplete
{
    class Foo
    {
        public async Task<bool> Bar()
        {
            try
            {
                await Task.Delay(5000);

                ThreadHelper.Instance.GetCancellationToken(Program.FooTask2).ThrowIfCancellationRequested();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Bar1()
        {
            try
            {
                await Task.Delay(5000);

                ThreadHelper.Instance.GetCancellationToken(Program.FooTask2).ThrowIfCancellationRequested();

                Console.WriteLine("Bar1 has finished");

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
