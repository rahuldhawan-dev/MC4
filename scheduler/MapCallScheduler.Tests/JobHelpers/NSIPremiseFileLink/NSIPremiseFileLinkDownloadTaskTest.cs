using MapCallScheduler.JobHelpers.NSIPremiseFileLink;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.NSIPremiseFileLink
{
    [TestClass]
    public class NSIPremiseFileDownloadServiceTest : FileDownloadServiceTestBase<INSIPremiseFileLinkServiceConfiguration, 
        NSIPremiseFileDownloadService> { }
}
