using System;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCall.Common.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.CommonTest.Validation
{
    [TestClass]
    public class BacterialWaterSampleValidationHelperTest : MapCallMvcSecuredRepositoryTestBase<BacterialWaterSample,
        BacterialWaterSampleRepository, User>
    {
        #region Fields

        private BacterialWaterSampleValidationHelper _target;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<ISampleSiteRepository>().Use<SampleSiteRepository>();
            i.For<IPublicWaterSupplyRepository>().Use<PublicWaterSupplyRepository>();
            i.For<IDateTimeProvider>().Use<DateTimeProvider>();
            i.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _target = _container.GetInstance<BacterialWaterSampleValidationHelper>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestValidateTotalChlorineForSampleSite()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Func<decimal?, int?, bool> getResult = (chlorine, sampleSiteId) => {
                return _target.ValidateTotalChlorineForSampleSite(chlorine, sampleSiteId);
            };

            // Must always return true if SampleSite is null
            Assert.IsTrue(getResult(null, null));
            Assert.IsTrue(getResult(0m, null));

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must always return true if PWSID is not null and TotalChlorineReported is false
            pwsid.TotalChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must return false if PWSID.TotalChlorineReported is true and the chlorine value is null
            pwsid.TotalChlorineReported = true;
            Assert.IsFalse(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));
        }

        [TestMethod]
        public void TestValidateFreeChlorineForSampleSite()
        {
            var pwsid = GetFactory<PublicWaterSupplyFactory>().Create();
            var sampleSite = GetFactory<SampleSiteFactory>().Create();

            Func<decimal?, int?, bool> getResult = (chlorine, sampleSiteId) => {
                return _target.ValidateFreeChlorineForSampleSite(chlorine, sampleSiteId);
            };

            // Must always return true if SampleSite is null
            Assert.IsTrue(getResult(null, null));
            Assert.IsTrue(getResult(0m, null));

            // Must always return true if PWSID is null
            sampleSite.PublicWaterSupply = null;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must always return true if PWSID is not null and FreeChlorineReported is false
            pwsid.FreeChlorineReported = false;
            sampleSite.PublicWaterSupply = pwsid;
            Assert.IsTrue(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));

            // Must return false if PWSID.FreeChlorineReported is true and the chlorine value is null
            pwsid.FreeChlorineReported = true;
            Assert.IsFalse(getResult(null, sampleSite.Id));
            Assert.IsTrue(getResult(0m, sampleSite.Id));
        }

        #endregion
    }
}
