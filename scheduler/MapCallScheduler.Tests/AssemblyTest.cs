using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCallScheduler.JobHelpers.AssetUploadProcessor;
using MapCallScheduler.JobHelpers.GIS.DumpTasks;
using MapCallScheduler.JobHelpers.GISMessageBroker.Tasks;
using MapCallScheduler.JobHelpers.LeakAlert;
using MapCallScheduler.JobHelpers.LIMSSynchronization;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.JobHelpers.MeterChangeOutStatusUpdate;
using MapCallScheduler.JobHelpers.NonRevenueWaterEntryCreator;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using MapCallScheduler.JobHelpers.SapEmployee;
using MapCallScheduler.JobHelpers.SapPremise;
using MapCallScheduler.JobHelpers.SapProductionWorkOrder;
using MapCallScheduler.JobHelpers.SapWaterQualityComplaint;
using MapCallScheduler.JobHelpers.ServicePremiseLink;
using MapCallScheduler.JobHelpers.SmartCoverAlert;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using MapCallScheduler.JobHelpers.W1V.ImportTasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IListExtensions;
using StructureMap;

namespace MapCallScheduler.Tests
{
    [TestClass]
    public class AssemblyTest
    {
        [TestMethod]
        public void TestNoClassesHaveMoreThanThreeDependencies()
        {
            var assemblies = new[] {
                typeof(MapCallSchedulerService).Assembly, // core
                typeof(Options).Assembly, // console
                typeof(MapCallScheduler).Assembly // service
            };
            var ignoreDependencies = new[] {typeof(ILog), typeof(IContainer)};
            var badTypes = new List<Type>();
            // please keep this list alphabetized
            var exceptions = new[] {
                typeof(AsBuiltImageTask),
                typeof(AssetUploadProcessorService),
                typeof(CustomerMaterialTask),
                typeof(DailyGISFileDumpTaskBase<>),
                typeof(GISMessageBrokerTaskBase<>),
                typeof(HydrantInspectionTask),
                typeof(HydrantTask),
                typeof(LeakAlertUpdaterService),
                typeof(MeterChangeOutStatusUpdateService),
                typeof(NonRevenueWaterEntryCreatorService),
                typeof(NonRevenueWaterEntryFileDumpTask),
                typeof(OneCallMessageHeartbeatService),
                typeof(SampleSiteTask),
                typeof(SampleSiteProfileSyncService),
                typeof(SapEmployeeUpdaterService),
                typeof(SapPremiseUpdaterService),
                typeof(SapScheduledProductionWorkOrderService),
                typeof(SapWaterQualityComplaintService),
                typeof(ServiceTask),
                typeof(ServicePremiseLinkService),
                typeof(SewerMainCleaningTask),
                typeof(SewerOpeningTask),
                typeof(SmartCoverAlertLinkService),
                typeof(SystemDeliveryEntryFileDumpTask),
                typeof(ValveTask),
                typeof(WorkOrderTask),
                typeof(W1VServiceTask)
            };

            foreach (var type in assemblies.SelectMany(assembly => assembly.GetTypes().Where(x => !exceptions.Contains(x) && !x.IsInterface && !x.Name.EndsWith("Args"))))
            {
                badTypes.AddRange(from constructor in type.GetConstructors()
                    select
                        constructor.GetParameters()
                            .Count(
                                parameter =>
                                    parameter.ParameterType.IsInterface &&
                                    !ignoreDependencies.Contains(parameter.ParameterType))
                    into deps
                    where deps > 3
                    select type);
            }

            Assert.IsTrue(badTypes.Count == 0,
                "No classes should have more than 3 dependencies.  The following types have violated this rule:{0}{1}",
                Environment.NewLine, String.Join(Environment.NewLine, badTypes.Map(t => t.Name)));
        }
    }
}
