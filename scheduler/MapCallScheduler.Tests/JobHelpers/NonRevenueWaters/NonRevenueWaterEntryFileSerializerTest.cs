using System;
using System.Linq;
using MapCall.Common.Model.ViewModels;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.NonRevenueWaters
{
    [TestClass]
    public class NonRevenueWaterEntryFileSerializerTest
    {
        #region Constants

        private static DateTime _now = new DateTime(2023, 6, 6);

        private static readonly NonRevenueWaterEntryFileDumpViewModel[] NON_REVENUE_WATER_ENTRY = {
            new NonRevenueWaterEntryFileDumpViewModel {
                Year = _now.Year.ToString(),
                Month = _now.Month.ToString(),
                BusinessUnit = "0000123456",
                OperatingCenter = "PA65 - Coatesville",
                Value = "8675309"
            }
        };

        private const string ENTRY = "2023,6,0000123456,PA65 - Coatesville,8675309\r\n";

        #endregion

        #region Private Members

        private IContainer _container;
        private NonRevenueWaterEntryFileSerializer _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new NonRevenueWaterEntryFileSerializer();
            _container = new Container(InitializeContainer);
        }

        #endregion

        #region Private Methods

        private static void InitializeContainer(ConfigurationExpression e)
        {
            e.For<IDateTimeProvider>().Mock();
        }

        #endregion

        [TestMethod]
        public void Test_Serializer_Serializes()
        {
            var result = _target.Serialize(NON_REVENUE_WATER_ENTRY.AsQueryable());
            MyAssert.StringsAreEqual(ENTRY, result);
        }
    }
}
