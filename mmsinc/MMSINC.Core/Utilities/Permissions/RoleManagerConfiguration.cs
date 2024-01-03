using System.Configuration;
using System.Web.Configuration;

namespace MMSINC.Utilities.Permissions
{
    /// <summary>
    /// Class needed so we can edit these settings through the web.config. 
    /// </summary>
    public class RoleManagerConfiguration : ConfigurationSection
    {
        #region Constants

        public const string CONFIG_SECTION_NAME = "roleManager";
        public const string CONFIG_CONNECTION_STRING_NAME = "connectionStringName";

        #endregion

        #region Properties

        [ConfigurationProperty(CONFIG_CONNECTION_STRING_NAME, IsRequired = true)]
        public string ConnectionStringName
        {
            get { return (string)this[CONFIG_CONNECTION_STRING_NAME]; }
            set { this[CONFIG_CONNECTION_STRING_NAME] = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets the roleManager config element from web.config. This is only ever get accessed
        /// once per application, and that's just to get the connection string, so there's no 
        /// need to cache it. 
        /// </summary>
        /// <returns></returns>
        public static RoleManagerConfiguration GetRoleManagerConfiguration()
        {
            // No need for throwing exception shere. The ConfigurationManager will do plenty of that
            // for us if the section is missing. 
            return (RoleManagerConfiguration)WebConfigurationManager.GetSection(CONFIG_SECTION_NAME);
        }

        #endregion
    }
}
