using MapCallScheduler.JobHelpers.SapPremise;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SapPremises
{
    [TestClass]
    public class SapPremiseServiceTest : SapFileProcessingServiceTestBase<SapPremiseService, ISapPremiseFileService, ISapPremiseUpdaterService>
    {
    }
}
