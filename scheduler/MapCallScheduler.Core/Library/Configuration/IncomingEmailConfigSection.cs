using System;
using System.Configuration;

namespace MapCallScheduler.Library.Configuration
{
    public class IncomingEmailConfigSection : ConfigurationSection, IIncomingEmailConfigSection
    {
        #region Constants

        public struct Keys
        {
            #region Constants

            public const string SERVER = "server",
                PORT = "port",
                USERNAME = "username",
                PASSWORD = "password",
                GAP_INTERVAL = "gapInterval",
                MAKE_CHANGES = "makeChanges";

            #endregion
        }

        #endregion

        #region Properties

        [ConfigurationProperty(Keys.SERVER, IsRequired = true)]
        public string Server => this[Keys.SERVER].ToString();

        [ConfigurationProperty(Keys.PORT, IsRequired = true)]
        public int Port => (int)this[Keys.PORT];

        [ConfigurationProperty(Keys.USERNAME, IsRequired = true)]
        public string Username => this[Keys.USERNAME].ToString();

        [ConfigurationProperty(Keys.PASSWORD, IsRequired = true)]
        public string Password => this[Keys.PASSWORD].ToString();

        [ConfigurationProperty(Keys.GAP_INTERVAL, DefaultValue = null)]
        public int? GapInterval
        {
            get
            {
                var interval = this[Keys.GAP_INTERVAL];
                return interval == null ? (int?)null : Convert.ToInt32(interval);
            }
        }

        /// <summary>
        /// Whether or not changes should be made to the messages on the (Imap) server.
        /// </summary>
        [ConfigurationProperty(Keys.MAKE_CHANGES, IsRequired = false, DefaultValue = false)]
        public bool MakeChanges => (bool)this[Keys.MAKE_CHANGES];

        #endregion
    }

    /// <summary>
    /// Contains details necessary for connecting to an Imap server.
    /// </summary>
    public interface IIncomingEmailConfigSection
    {
        #region Abstract Properties

        string Server { get; }
        int Port { get; }
        string Username { get; }
        string Password { get; }
        int? GapInterval { get; }
        /// <summary>
        /// If false, no changes should be made to any messages on the server.
        /// </summary>
        bool MakeChanges { get; }

        #endregion
    }
}