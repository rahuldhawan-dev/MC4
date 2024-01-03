using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.Library.JobHelpers.Sap
{
    /// <summary>
    /// See ISapServiceConfiguration.cs
    /// </summary>
    public abstract class SapServiceConfigurationBase : ISapServiceConfiguration
    {
        #region Properties

        public IFileConfigSection FileConfig => this.GetFileConfig();

        #endregion

        #region Abstract Properties

        public abstract string GroupName { get; }

        #endregion
    }
}