using MapCallScheduler.JobHelpers.SapEmployee;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SapEmployee
{
    [TestClass]
    public class SapEmployeeServiceTest :
        SapFileProcessingServiceTestBase<SapEmployeeService, ISapEmployeeFileService, ISapEmployeeUpdaterService> {}
}
