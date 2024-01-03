using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SystemDeliveryEntry
{
    [TestClass]
    public class SystemDeliveryEntryFileSerializerTest
    {
        #region Private Members

        private SystemDeliveryEntryFileSerializer _target;
        private IContainer _container;
        private static DateTime _sampledate = new DateTime(2021, 3, 4);

        public static readonly SystemDeliveryEntryFileDumpViewModel[] SYSTEM_DELIVERY_ENTRY_SAMPLE_INPUT = {
            new SystemDeliveryEntryFileDumpViewModel {
                Month = _sampledate.Month.ToString(),
                Year = _sampledate.Year.ToString(),
                AsPostedDescription = "AS POSTED",
                BusinessUnit = "000010101",
                EntryDescription = "SYS_NORMAL",
                FacilityName = "Facility lolz",
                TotalValue = "100.0000",
                SystemDeliveryDescription = "SYSTEM DELIVERY"
            }
        };

        private string sampleOutput = "2021,3,000010101,Facility lolz,SYS_NORMAL,AS POSTED,SYSTEM DELIVERY,100.0000\r\n";

        private void InitializeContainer(ConfigurationExpression e)
        {
            e.For<IDateTimeProvider>().Mock();
        }
        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _target = new SystemDeliveryEntryFileSerializer();
            _container = new Container(InitializeContainer);
        }

        #endregion

        [TestMethod]
        public void TestSerializerSerializesCorrectly()
        {
            var result = _target.Serialize(SYSTEM_DELIVERY_ENTRY_SAMPLE_INPUT.AsQueryable());
            MyAssert.StringsAreEqual(sampleOutput, result);
        }
    }
}
