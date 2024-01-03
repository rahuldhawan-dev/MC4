namespace MapCall.Common.Utility
{
    // I am futuring proofing this now on the off chance
    // that some day we have to use this with a third site.
    public enum Site
    {
        MapCall,
        WorkOrders
    }

    /// <summary>
    /// Class to be used by each site for configuring site-specific stuff
    /// that may be needed. Right now this is just needed for the Menu control.
    /// But I'm sure we're gonna come across other needs for this.
    /// </summary>
    public class ResourceConfiguration : IResourceConfiguration
    {
        #region Properties

        public bool IsDevMachine { get; set; }

        /// <summary>
        /// Is it MapCall or 271?
        /// </summary>
        public Site Site { get; set; }

        public string ConfigurationResourceName { get; set; }

        #endregion
    }
}
