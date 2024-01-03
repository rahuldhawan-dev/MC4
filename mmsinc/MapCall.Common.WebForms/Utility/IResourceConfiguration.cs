namespace MapCall.Common.Utility
{
    public interface IResourceConfiguration
    {
        #region Properties

        bool IsDevMachine { get; }
        Site Site { get; }
        string ConfigurationResourceName { get; set; }

        #endregion
    }
}
