using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class WorkDescriptionRepositoryTest : InMemoryDatabaseTest<WorkDescription, WorkDescriptionRepository>
    {
        #region Fields

        private TestDateTimeProvider _dateTimeProvider;
        private DateTime _now;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression i)
        {
            base.InitializeObjectFactory(i);
            i.For<IDateTimeProvider>().Use(_dateTimeProvider = new TestDateTimeProvider(_now = DateTime.Now));
            i.For<IIconSetRepository>().Use<IconSetRepository>();
            i.For<IServiceRepository>().Use<ServiceRepository>();
            i.For<ITapImageRepository>().Use<TapImageRepository>();
            i.For<IImageToPdfConverter>().Use(() => new ImageToPdfConverter());
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestGetActiveByAssetTypeIdReturnsActiveWorkDescriptionsByAssetType()
        {
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var goodDescription = GetFactory<ValveRepairWorkDescriptionFactory>().Create(new
                {AssetType = valveAssetType, Description = "This is a great description!"});
            var badDescription = GetFactory<HydrantRepairWorkDescriptionFactory>().Create(new
                {AssetType = hydrantAssetType, Description = "This is a terrible description!"});
            Session.Flush();

            var result = Repository.GetActiveByAssetTypeId(valveAssetType.Id);
            Assert.AreSame(goodDescription, result.Single());
        }

        [TestMethod]
        public void TestGetActiveByAssetTypeIdDoesNotReturnActiveWorkDescriptionsWhatAreNotActive()
        {
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var goodDescription = GetFactory<ValveRepairWorkDescriptionFactory>().Create(new
                {AssetType = valveAssetType, Description = "This is a great description!"});
            var badDescription = GetFactory<HydrantRepairWorkDescriptionFactory>().Create(new
                {AssetType = valveAssetType, Description = "This is a terrible description!", IsActive = false});
            Session.Flush();

            var result = Repository.GetActiveByAssetTypeId(valveAssetType.Id);
            Assert.AreSame(goodDescription, result.Single());
        }

        [TestMethod]
        public void TestGetActiveByAssetTypeIdForCreateGetsInitial()
        {
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var initialWorkDescription = GetFactory<HydrantInstallationWorkDescriptionFactory>()
               .Create(new {AssetType = hydrantAssetType});
            var revisitWorkDescription = GetFactory<HydrantLandscapingWorkDescriptionFactory>()
               .Create(new {AssetType = hydrantAssetType});
            Session.Flush();

            var result = Repository.GetActiveByAssetTypeIdForCreate(hydrantAssetType.Id, false);

            Assert.AreSame(initialWorkDescription, result.Single());
        }

        [TestMethod]
        public void TestGetActiveByAssetTypeIdForCreateGetsRevisit()
        {
            var initialWorkDescription1 = GetFactory<HydrantInstallationWorkDescriptionFactory>().Create();
            var revisitWorkDescription1 = GetFactory<HydrantLandscapingWorkDescriptionFactory>().Create();
            Session.Flush();

            var result = Repository.GetActiveByAssetTypeIdForCreate(initialWorkDescription1.AssetType.Id, true);

            Assert.AreSame(revisitWorkDescription1, result.Single());
        }

        [TestMethod]
        public void TestGetUsedByAssetTypeIdsReturnsUsedWorkDescriptionsByAssetType()
        {
            var valveAssetType = GetFactory<ValveAssetTypeFactory>().Create();
            var hydrantAssetType = GetFactory<HydrantAssetTypeFactory>().Create();
            var goodDescription = GetFactory<ValveRepairWorkDescriptionFactory>().Create(new {
                AssetType = valveAssetType, Description = "This is a great description!"
            });
            GetFactory<WorkOrderFactory>()
               .Create(new {WorkDescription = goodDescription});
            var badDescription = GetFactory<HydrantRepairWorkDescriptionFactory>().Create(new {
                AssetType = valveAssetType, Description = "This description is not used by any WorkOrder!"
            });
            Session.Flush();

            var result = Repository.GetUsedByAssetTypeIds(new[] { valveAssetType.Id });
            Assert.AreSame(goodDescription, result.Single());
        }
        #endregion
    }
}
