using System.Configuration;

namespace MMSINC.Configuration
{
    public static class GroupedServiceConfigurationExtensions
    {
        /// <summary>
        /// Gets the configuration section for the given group name and section key.
        /// </summary>
        /// <typeparam name="TConfigSection">The type of the configuration section to get.</typeparam>
        /// <param name="that">The <see cref="IGroupedServiceConfiguration"/> instance this extension method is invoked upon.</param>
        /// <param name="sectionKey">The key of the section to get.</param>
        public static TConfigSection GetConfigSection<TConfigSection>(this IGroupedServiceConfiguration that, string sectionKey)
        {
            return (TConfigSection)ConfigurationManager.GetSection($"{that.GroupName}/{sectionKey}");
        }
    }
}
