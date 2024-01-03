using MMSINC.Interface;

namespace MMSINC.Common
{
    public interface ISmtpClientFactory
    {
        ISmtpClient Build();
    }
}
