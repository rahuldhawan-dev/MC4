using System.Net.Mail;
using MMSINC.Interface;

namespace MMSINC.Common
{
    public class MailMessageFactory : IMailMessageFactory
    {
        public IMailMessage Build()
        {
            return new MailMessageWrapper(new MailMessage());
        }
    }
}
