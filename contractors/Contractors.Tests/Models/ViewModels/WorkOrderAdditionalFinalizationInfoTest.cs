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
using MMSINC.Data;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.NHibernate;
using StructureMap;

namespace Contractors.Tests.Models.ViewModels
{
    [TestClass]
    public class WorkOrderAdditionalFinalizationInfoTest : MapCallMvcInMemoryDatabaseTestBase<WorkOrder>
    {
        #region Fields

        private ViewModelTester<WorkOrderAdditionalFinalizationInfo, WorkOrder> _vmTester;
        private WorkOrderAdditionalFinalizationInfo _viewModel;
        private WorkOrder _entity;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<WorkOrderFactory>().Create();
            _viewModel = _viewModelFactory.Build<WorkOrderAdditionalFinalizationInfo, WorkOrder>(_entity);
            _vmTester = _container.With(_entity).With(_viewModel)
                                  .GetInstance<ViewModelTester<WorkOrderAdditionalFinalizationInfo, WorkOrder>>();
        }

        #endregion

        #region Tests

        #region Mapping

        [TestMethod]
        public void TestMapToEntitySetsSendSewerOverflowChangedNotification()
        {
            _viewModel.WorkDescription = (int)WorkDescription.Indices.SEWER_MAIN_OVERFLOW;
            
            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendSewerOverflowChangedNotification);

            _entity.WorkDescription = null;

            _vmTester.MapToEntity();

            Assert.IsTrue(_viewModel.SendSewerOverflowChangedNotification);
        }

        #endregion

        #endregion
    }
}
