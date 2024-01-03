using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MapCallIntranet.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;
namespace MapCallIntranet.Tests
{
    [TestClass]
    public class AssemblyTest
    {
        [TestMethod]
        public void TestRESTfulCRUDMethods()
        {
            // we're skipping our main controller here that we really want to test. 
            // it has a few other oddities, and we have a manual test for its POST atttribute
            // this is here to catch any future controllers that might be added
            var skipControllers = new[] {
                typeof(NearMissController)
            };
            TestLibrary.TestRESTfulCRUDMethods(
                Assembly.GetAssembly(typeof(NearMissController)),
                typeof(Controller),
                t => !skipControllers.Contains(t));
        }
    }
}
