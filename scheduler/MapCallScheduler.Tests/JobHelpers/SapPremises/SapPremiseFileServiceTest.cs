using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCallScheduler.JobHelpers.SapPremise;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using MapCallScheduler.Tests.Library.JobHelpers.Sap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.SapPremises
{
    [TestClass]
    public class SapPremiseFileServiceTest : FileDownloadServiceTestBase<ISapPremiseServiceConfiguration, SapPremiseFileService>
    {
    }
}
