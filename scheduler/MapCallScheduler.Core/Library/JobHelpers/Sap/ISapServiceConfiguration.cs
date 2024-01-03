using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    /// <summary>
    /// Represents an object containing FTP configuration information for an SAP file processing 
    /// service.  You should inherit from SapServiceConfigurationBase and provide an interface which inherits
    /// from this one.
    /// </summary>
    public interface ISapServiceConfiguration : IFileServiceConfiguration {}
}