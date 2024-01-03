using System.Net;
using FluentFTP;
using MapCallScheduler.Library.Configuration;

namespace MapCallScheduler.Library.Ftp
{
    public class FtpClientFactory : IFtpClientFactory
    {
        #region Exposed Methods

        public IFtpClient FromConfig(IFtpConfigSection config)
        {
            return new FtpClient {
                Host = config.Host,
                Credentials = new NetworkCredential(config.User, config.Password)
            };
        }

        #endregion
    }

    public interface IFtpClientFactory
    {
        #region Abstract Methods

        IFtpClient FromConfig(IFtpConfigSection config);

        #endregion
    }
}