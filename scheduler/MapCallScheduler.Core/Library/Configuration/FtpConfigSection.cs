using System.Configuration;

namespace MapCallScheduler.Library.Configuration
{
    public class FtpConfigSection : ConfigurationSection, IFtpConfigSection
    {
        #region Constants

        public struct Keys
        {
            #region Constants

            public const string HOST = "host",
                USER = "username",
                PASSWORD = "password",
                MAKE_CHANGES = "makeChanges",
                WORKING_DIRECTORY = "workingDirectory";

            #endregion
        }

        #endregion

        #region Properties

        [ConfigurationProperty(Keys.HOST, IsRequired = true)]
        public string Host => this[Keys.HOST].ToString();

        [ConfigurationProperty(Keys.USER, IsRequired = true)]
        public string User => this[Keys.USER].ToString();

        [ConfigurationProperty(Keys.PASSWORD, IsRequired = true)]
        public string Password => this[Keys.PASSWORD].ToString();

        [ConfigurationProperty(Keys.MAKE_CHANGES, IsRequired = false, DefaultValue = false)]
        public bool MakeChanges => (bool)this[Keys.MAKE_CHANGES];

        [ConfigurationProperty(Keys.WORKING_DIRECTORY, DefaultValue = ".")]
        public string WorkingDirectory => this[Keys.WORKING_DIRECTORY].ToString();

        #endregion
    }

    /// <summary>
    /// Contains details necessary for connecting to an FTP server.
    /// </summary>
    public interface IFtpConfigSection
    {
        #region Abstract Properties

        string Host { get; }
        string Password { get; }
        string User { get; }
        bool MakeChanges { get; }
        string WorkingDirectory { get; }

        #endregion
    }
}