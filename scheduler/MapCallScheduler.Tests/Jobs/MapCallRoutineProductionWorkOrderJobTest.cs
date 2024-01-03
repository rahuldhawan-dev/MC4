using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCallScheduler.JobHelpers.MapCallRoutineProductionWorkOrder;
using MapCallScheduler.JobHelpers.MarkoutTickets;
using MapCallScheduler.Jobs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallScheduler.Tests.Jobs
{
    [TestClass]
    public class MapCallRoutineProductionWorkOrderJobTest 
        : MapCallJobWithProcessableServiceJobTestBase<MapCallRoutineProductionWorkOrderJob, IMapCallRoutineProductionWorkOrderService> { }
}
