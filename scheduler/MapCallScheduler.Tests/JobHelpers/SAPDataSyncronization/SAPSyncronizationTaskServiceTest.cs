using System;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.SAP.Model;
using MapCall.SAP.Model.Repositories;
using MapCallScheduler.JobHelpers.SAPDataSyncronization;
using MapCallScheduler.JobHelpers.SAPDataSyncronization.Tasks;
using MapCallScheduler.Tests.Library.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.ClassExtensions;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SAPDataSyncronization
{
    [TestClass]
    public class SAPSyncronizationTaskServiceTest : TaskServiceTestBase<SAPSyncronizationTaskService, SAPSyncronizationTaskBase>
    {
        #region Properties

        protected override Type[] ExpectedTaskTypes => new[] {
            typeof(SAPBlowOffInspectionTask),
            typeof(SAPEquipmentTask),
            typeof(SAPHydrantInspectionTask),
            typeof(SAPHydrantTask),
            typeof(SAPSewerMainCleaningTask),
            typeof(SAPSewerOpeningTask),
            typeof(SAPValveInspectionTask),
            typeof(SAPValveTask),
            typeof(SAPWorkOrderTask)
        };

        #endregion

        #region Private Methods

        protected override void InitializeContainer(ConfigurationExpression e)
        {
            e.For<ISAPEquipmentRepository>().Mock();
            e.For<ISAPInspectionRepository>().Mock();
            e.For<ISAPWorkOrderStatusUpdateRepository>().Mock();
            e.For<IRepository<Hydrant>>().Mock();
            e.For<IRepository<SewerOpening>>().Mock();
            e.For<IRepository<Valve>>().Mock();
            e.For<IRepository<HydrantInspection>>().Mock();
            e.For<IRepository<ValveInspection>>().Mock();
            e.For<IRepository<BlowOffInspection>>().Mock();
            e.For<IRepository<SewerMainCleaning>>().Mock();
            e.For<IRepository<WorkOrder>>().Mock();
            e.For<IRepository<Equipment>>().Mock();
            e.For<ISAPNewServiceInstallationRepository>().Mock();
            e.For<ISAPWorkOrderRepository>().Mock();
            e.For<ILog>().Mock();
        }

        #endregion
    }
}
