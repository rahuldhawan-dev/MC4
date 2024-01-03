using System;
using System.Net.Mail;
using System.Text;
using System.Web;
using MMSINC.Common;
using MMSINC.Interface;

namespace MMSINC.Utilities.ErrorHandling
{
    public class ErrorMessageGenerator : IErrorMessageGenerator
    {
        #region Constants

        public struct FormatStrings
        {
            public const string FIRST_EXCEPTION =
                                    "Exception: {0}<br/><br/>Message:<br/>{1}<br/><br/>Stacktrace:<br/>{2}",
                                FIRST_EXCEPTION_HTTP =
                                    "Current User: {0}<br/><br/>Exception: {1}<br/><br/>Message:<br/>{2}<br/><br/>Page:<br/>{3}<br/><br/>Parameters:<br/>{4}<br/><br/>Stacktrace:<br/>{5}",
                                INNER_EXCEPTION =
                                    "<br/><br/>Inner Exception: {0}<br/><br/>Message:<br/>{1}<br/><br/>Stacktrace:<br/>{2}",
                                SENDER = "noreply@{0}",
                                SUBJECT = "Error: {0}";
        }

        public const string INDENT_STRING = "&nbsp;&nbsp;";
        public const string LINE_SEPARATOR = "<br/>";
        public const string RECIPIENT = "mapcall_exterminator@amwater.com";

        public static readonly string[] KEYS_TO_SKIP = new[] {
            "__VIEWSTATE"
        };

        #endregion

        #region Private Members

        protected IParameterCollectionFormatter _parameterFormatter;

        #endregion

        #region Constructors

        public ErrorMessageGenerator()
        {
            _parameterFormatter = new ParameterCollectionFormatter();
        }

        #endregion

        #region Public Methods

        public string GenerateMessageBody(IHttpContext ctx, Exception ex = null)
        {
            ex = ex ?? ctx.Server.GetLastError();

            while (ex is HttpUnhandledException)
            {
                ex = ex.InnerException;
            }

            var message =
                new StringBuilder(String.Format(FormatStrings.FIRST_EXCEPTION_HTTP,
                    ctx.User.Name,
                    ex.GetType(),
                    ex.Message, ctx.Request.RawUrl,
                    _parameterFormatter.FormatParameters(ctx.Request,
                        INDENT_STRING, LINE_SEPARATOR, KEYS_TO_SKIP),
                    (ex.StackTrace ?? String.Empty).Replace("\n", "<br/>")));

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                message.AppendFormat(FormatStrings.INNER_EXCEPTION, ex.GetType(),
                    ex.Message,
                    (ex.StackTrace ?? String.Empty).Replace("\n", "<br/>"));
            }

            return message.ToString();
        }

        public string GenerateMessageBody(Exception ex)
        {
            var message =
                new StringBuilder(String.Format(FormatStrings.FIRST_EXCEPTION,
                    ex.GetType(),
                    ex.Message,
                    (ex.StackTrace ?? String.Empty).Replace("\n", "<br/>")));

            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                message.AppendFormat(FormatStrings.INNER_EXCEPTION, ex.GetType(),
                    ex.Message,
                    (ex.StackTrace ?? String.Empty).Replace("\n", "<br/>"));
            }

            return message.ToString();
        }

        public IMailMessage GenerateMailMessage(IHttpContext ctx, Exception e = null)
        {
            var serverName =
                ctx.Request.ServerVariables["SERVER_NAME"].Replace("www.", "");
            return
                new MailMessageWrapper(
                    String.Format(FormatStrings.SENDER, serverName), RECIPIENT) {
                    IsBodyHtml = true,
                    Subject =
                        String.Format(FormatStrings.SUBJECT, serverName),
                    Priority = MailPriority.High,
                    Body = GenerateMessageBody(ctx, e)
                };
        }

        #endregion
    }

    public interface IErrorMessageGenerator
    {
        #region Methods

        IMailMessage GenerateMailMessage(IHttpContext ctx, Exception e = null);
        string GenerateMessageBody(IHttpContext ctx, Exception e = null);
        string GenerateMessageBody(Exception e);

        #endregion
    }
}
