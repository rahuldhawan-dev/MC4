using MMSINC.Interface;

namespace MMSINC.Common
{
    /// <summary>
    /// This factory exists so that the nested containers stop returning
    /// the same ISmtpClient instance every time one is requested.
    /// </summary>
    public class SmtpClientFactory : ISmtpClientFactory
    {
        public ISmtpClient Build()
        {
            // we need to call the constructor here rather than using a
            // container as using a container would just propagate the
            // single instance/disposal issue.
            return new SmtpClientWrapper();
        }
    }
}
