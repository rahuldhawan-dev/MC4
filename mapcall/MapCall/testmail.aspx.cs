using System;
using System.Configuration;
using System.Net.Mail;
using System.Web.UI;
using MapCall.Common.Utility.Notifications;

namespace MapCall
{
    public partial class testmail : Page
    {
        protected void btnGo_Click(object sender, EventArgs e)
        {
            using (var mm = new MailMessage())
            {
                mm.Subject = "Mail Tester";
                mm.To.Add(txtEmail.Text);
                mm.Sender = new MailAddress(ConfigurationManager.AppSettings[NotifierBase.FROM_ADDRESS_KEY]);
                mm.Body = "Mail Tester";
                using (var sc = new SmtpClient())
                {
                    sc.Send(mm);
                }
                Response.Write("Sent"); 
            }
        }
    }
}
