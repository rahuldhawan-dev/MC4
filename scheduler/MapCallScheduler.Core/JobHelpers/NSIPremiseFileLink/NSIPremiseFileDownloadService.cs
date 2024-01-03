using log4net;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.Library.Filesystem;
using MapCallScheduler.Library.JobHelpers.Common;

namespace MapCallScheduler.JobHelpers.NSIPremiseFileLink
{
    public class NSIPremiseFileDownloadService : FileDownloadServiceBase<INSIPremiseFileLinkServiceConfiguration>
    {
        public override string FileType => null;
        //File name will be structured in the following way
        //OperatingCenterID.PremiseNumber.InstallationNumber.DeviceLocation.Connection.pdf
        public override string FilePattern => "*.*.*.*.*.pdf";

        public NSIPremiseFileDownloadService(INSIPremiseFileLinkServiceConfiguration config, IFileClientFactory fileClientFactory, ILog log) : base(config, fileClientFactory, log) { }
    }
}
