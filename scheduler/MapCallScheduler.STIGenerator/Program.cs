using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StructureMap;

namespace MapCallScheduler.STIGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(new DependencyRegistry());
            var runner = container.GetInstance<Runner>();

            runner.OverrideTypeRegistrations();
            runner.Run();
        }
    }
}
