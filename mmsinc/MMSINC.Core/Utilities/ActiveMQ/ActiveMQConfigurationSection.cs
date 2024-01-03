using System;
using System.Configuration;
using System.Linq;

namespace MMSINC.Utilities.ActiveMQ
{
    public class ActiveMQConfigurationSection : ConfigurationSection
    {
        #region Constants

        public const string SECTION_NAME = "activeMQ";

        public struct Keys
        {
            #region Constants

            public const string SCHEME = "scheme",
                                HOST = "host",
                                PORT = "port",
                                TOPIC = "topic",
                                USER = "user",
                                PASSWORD = "password";

            #endregion
        }

        public static readonly string[] ALLOWED_SCHEMES = {
            "tcp", "amqp", "amqps"
        };

        #endregion

        #region Properties

        [ConfigurationProperty(Keys.SCHEME, IsRequired = true)]
        public string Scheme
        {
            get
            {
                var value = this[Keys.SCHEME].ToString();
                if (!ALLOWED_SCHEMES.Contains(value))
                {
                    throw new InvalidOperationException(
                        $"'{value}' is not a recognized scheme.  Only the following values are allowed: {string.Join(", ", ALLOWED_SCHEMES)}");
                }

                return value;
            }
        }

        [ConfigurationProperty(Keys.HOST, IsRequired = true)]
        public string Host => this[Keys.HOST].ToString();

        [ConfigurationProperty(Keys.PORT, IsRequired = true)]
        public int Port => (int)this[Keys.PORT];

        [ConfigurationProperty(Keys.USER)]
        public string User => this[Keys.USER].ToString();

        [ConfigurationProperty(Keys.PASSWORD)]
        public string Password => this[Keys.PASSWORD].ToString();

        #endregion
    }
}
