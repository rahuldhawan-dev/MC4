using System.Configuration;

namespace DeleporterCore.Configuration
{
    public class DeleporterConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("Port", DefaultValue = NetworkConfiguration.DEFAULT_PORT)]
        public int Port
        {
            get { return (int)this["Port"]; }
            set { this["Port"] = value; }
        }

        [ConfigurationProperty("Host", DefaultValue = NetworkConfiguration.DEFAULT_HOST)]
        public string Host
        {
            get { return (string)this["Host"]; }
            set { this["Host"] = value; }
        }

        [ConfigurationProperty("ServiceName", DefaultValue = NetworkConfiguration.DEFAULT_SERVICE_NAME)]
        public string ServiceName
        {
            get { return (string)this["ServiceName"]; }
            set { this["ServiceName"] = value; }
        }
    }
}
