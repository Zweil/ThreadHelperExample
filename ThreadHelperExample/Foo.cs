using System.Threading.Tasks;

namespace ThreadHelperExample
{
    class Foo
    {
        public async Task<bool> Bar()
        {
            try
            {
                await Task.Delay(5000);

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
