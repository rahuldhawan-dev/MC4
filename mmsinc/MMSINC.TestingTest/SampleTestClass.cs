using System;
using MMSINC.Testing.Linq;
using MMSINCTestImplementation.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.TestingTest
{
    [TestClass]
    // This class exists in order to setup the Tests for the
    // LinqUnitTestClass
    // see: TestTestAllStringPropertiesThrowExceptionWhenSetTooLongFailsWhenNotSet
    public class SampleTestClass : LinqUnitTestClass<Shipper>
    {
        protected override void DeleteObject(Shipper entity)
        {
            throw new NotImplementedException();
        }

        protected override Shipper GetValidObjectFromDatabase()
        {
            throw new NotImplementedException();
        }
    }
}
