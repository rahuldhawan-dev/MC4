using MapCallScheduler.Library.Common;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    /// <summary>
    /// Represents the top level object responsible for getting all relevant files from the file service,
    /// passing those to an updater service for updating, and then deleting each downloaded file from the
    /// remote server.  You should inherit from SapFileProcessingServiceBase and provide an interface which
    /// inherits from this one.
    /// </summary>
    public interface ISapFileProcessingService : IProcessableService {}
}