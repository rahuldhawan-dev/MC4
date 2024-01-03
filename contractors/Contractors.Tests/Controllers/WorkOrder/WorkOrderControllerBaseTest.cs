using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers.WorkOrder;
using Contractors.Data.Models.Repositories;
using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using Moq;
using StructureMap;

namespace Contractors.Tests.Controllers.WorkOrder
{
    [TestClass]
    public class WorkOrderControllerBaseTest : ControllerTestBase<TestWorkOrderController, IWorkOrderRepository>
    {
        #region Private Members

        private Mock<IOperatingCenterRepository> _mockOperatingCenterRepository;
        private Mock<ITownRepository> _mockTownRepository;
        private Mock<IRepository<WorkOrderPriority>> _mockPrioritiesRepository;
        private Mock<IAssetTypeRepository> _mockAssetTypeRepository;
        private Mock<IRepository<MarkoutRequirement>> _mockMarkoutRequirementRepository;
        private Mock<IRepository<WorkOrderPurpose>> _mockWorkOrderPurposeRepository;
        private Mock<IRepository<WorkOrderRequester>> _mockWorkOrderRequesterRepository;

        #endregion

        #region Setup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _mockAssetTypeRepository = e.For<IAssetTypeRepository>().Mock();
            _mockPrioritiesRepository =
                e.For<IRepository<WorkOrderPriority>>().Mock();
            _mockTownRepository = e.For<ITownRepository>().Mock();
            _mockOperatingCenterRepository =
                e.For<IOperatingCenterRepository>().Mock();
            _mockMarkoutRequirementRepository =
                e.For<IRepository<MarkoutRequirement>>().Mock();
            _mockWorkOrderPurposeRepository =
                e.For<IRepository<WorkOrderPurpose>>().Mock();
            _mockWorkOrderRequesterRepository =
                e.For<IRepository<WorkOrderRequester>>().Mock();
            e.For<IViewModelFactory>().Use<ViewModelFactory>();
        }

        [TestInitialize]
        public void TestWorkOrderControllerTestInitialize()
        {
            BaseInitialize();
        }

        #endregion

        [TestMethod]
        public void TestWhyDoIUseThisBaseClass()
        {
            Assert.Inconclusive("TODO: I should be using the regular ControllerBaseTest shouldn't I?");
        }

        [TestMethod]
        public void TestSearchShouldRenderView()
        {
            var contractor = new Contractor();
            _mockAuthenticationService
                .Setup(x => x.CurrentUser.Contractor)
                .Returns(contractor);
            _mockAssetTypeRepository
                .Setup(x => x.GetAll())
                .Returns(new List<AssetType>().AsQueryable());
            _mockPrioritiesRepository
                .Setup(x => x.GetAll())
                .Returns(new List<WorkOrderPriority>().AsQueryable());

            _mockMarkoutRequirementRepository
                .Setup(x => x.GetAll())
                .Returns(new List<MarkoutRequirement>().AsQueryable());
            _mockWorkOrderPurposeRepository
                .Setup(x => x.GetAll())
                .Returns(new List<WorkOrderPurpose>().AsQueryable());
            _mockWorkOrderRequesterRepository
                .Setup(x => x.GetAll())
                .Returns(new List<WorkOrderRequester>().AsQueryable());

            _mockOperatingCenterRepository
                .Setup(x => x.GetAll())
                .Returns(new List<OperatingCenter>().AsQueryable());
            _mockTownRepository
                .Setup(x => x.GetAll())
                .Returns(new List<Town>().AsQueryable());

            var result = _target.Search() as ViewResult;

            Assert.IsNotNull(result);
        }
    }

    public class TestWorkOrderController : WorkOrderControllerBase<TestWorkOrderSearch>
    {
        public TestWorkOrderController(ControllerBaseWithPersistenceArguments<IWorkOrderRepository, MapCall.Common.Model.Entities.WorkOrder, ContractorUser> args) : base(args) {}
    }

    public class TestWorkOrderSearch : SearchSet<MapCall.Common.Model.Entities.WorkOrder>, IWorkOrderSearch
    {
        #region Private Members

        private bool _queryIsNull;

        #endregion

        #region Constructors

        public TestWorkOrderSearch(bool queryIsNull)
        {
            _queryIsNull = queryIsNull;
        }

        #endregion

        #region Exposed Methods

        public int? Id
        {
            get { return null; }
        }

        public int[] DocumentType { get; set; }

        public bool QueryIsNull()
        {
            return _queryIsNull;
        }

        public bool NonWorkOrderNumberFieldsAreNull()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
