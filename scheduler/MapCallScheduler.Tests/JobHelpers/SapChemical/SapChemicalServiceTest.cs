using MapCallScheduler.JobHelpers.SapChemical;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SapChemical
{
    [TestClass]
    public class SapChemicalServiceTest :
        SapFileProcessingServiceTestBase<SapChemicalService, ISapChemicalFileService, ISapChemicalUpdaterService> {}
}
