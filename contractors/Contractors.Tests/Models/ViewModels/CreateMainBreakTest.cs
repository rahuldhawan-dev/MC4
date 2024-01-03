using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using Moq;
using StructureMap;
using IWorkOrderRepository = Contractors.Data.Models.Repositories.IWorkOrderRepository;
using WorkOrderRepository = Contractors.Data.Models.Repositories.WorkOrderRepository;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass()]
    public class CreateMainBreakTest : MapCallMvcInMemoryDatabaseTestBase<MainBreak>
    {
        #region Fields

        private ViewModelTester<CreateMainBreak, MainBreak> _vmTester;
        private CreateMainBreak _viewModel;
        private MainBreak _entity;
        private Mock<IAuthenticationService<ContractorUser>> _authServ;
        private ContractorUser _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<ContractorUser>>().Mock();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<ContractorUserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);

            _entity = GetFactory<MainBreakFactory>().Create();
            _viewModel = _container.GetInstance<CreateMainBreak>();
            _viewModel.Map(_entity);
            _vmTester = new ViewModelTester<CreateMainBreak, MainBreak>(_viewModel, _entity, _container.GetInstance<ITestDataFactoryService>());

            // Needs to be done or else any querying related to the WorkOrder will fail.
            _entity.WorkOrder.AssignedContractor = _user.Contractor;
            Session.Save(_entity.WorkOrder);
            Session.Flush();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMappingSimplePropertiesBothWays()
        {
            _vmTester.CanMapBothWays(x => x.FootageReplaced);
            _vmTester.CanMapBothWays(x => x.ReplacedWith);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestFootageReplacedIsRequiredWhenMainBreakReplace()
        {
            _entity.WorkOrder.WorkDescription.Id = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR;
            _viewModel.FootageReplaced = null;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FootageReplaced);

            _entity.WorkOrder.WorkDescription.Id = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.FootageReplaced);

            _viewModel.FootageReplaced = 5;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.FootageReplaced);
        }

        [TestMethod]
        public void TestReplacedWithIsRequiredWhenMainBreakReplace()
        {
            _entity.WorkOrder.WorkDescription.Id = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR;
            _viewModel.ReplacedWith = null;

            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ReplacedWith);

            _entity.WorkOrder.WorkDescription.Id = (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE;
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ReplacedWith);

            _viewModel.ReplacedWith = GetFactory<MainBreakMaterialFactory>().Create().Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.ReplacedWith);
        }

        #endregion

        #endregion
    }
}
