namespace MMSINC.Configuration
{
    /// <summary>
    /// Represents a configuration section that serves as a container for a group of
    /// inner configuration sections.
    /// </summary>
    public interface IGroupedServiceConfiguration
    {
        /// <summary>
        /// All section groups have a name.
        /// </summary>
        string GroupName { get; }
    }
}
