using System.Collections;
using System.Configuration;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Serialization.Formatters;

namespace DeleporterCore.Configuration
{
    public static class NetworkConfiguration
    {
        public const int DEFAULT_PORT = 60473;
        public const string DEFAULT_HOST = "localhost";
        public const string DEFAULT_SERVICE_NAME = "Deleporter.rm";

        public static int Port { get; private set; }
        public static string Host { get; private set; }
        public static string ServiceName { get; private set; }

        static NetworkConfiguration()
        {
            var config = (DeleporterConfigurationSection)ConfigurationManager.GetSection("deleporter");
            if (config != null)
            {
                Port = config.Port;
                Host = config.Host;
                ServiceName = config.ServiceName;
            }
            else
            {
                Port = DEFAULT_PORT;
                Host = DEFAULT_HOST;
                ServiceName = DEFAULT_SERVICE_NAME;
            }
        }

        public static IChannel CreateChannel()
        {
            IDictionary props = new Hashtable {{"port", Port}, {"typeFilterLevel", TypeFilterLevel.Full}};

            return new TcpChannel(props, null, new BinaryServerFormatterSinkProvider {
                TypeFilterLevel = TypeFilterLevel.Full
            });
        }

        public static string HostAddress
        {
            get { return string.Format("tcp://{0}:{1}/{2}", Host, Port, ServiceName); }
        }
    }
}
