using System;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Email;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    public class AuditFailureEmailer : IAuditFailureEmailer
    {
        #region Constants

        public const string SUBJECT = "Markout Ticket Audit Failed";

        public const string HTML_MESSAGE_FORMAT = @"<html>
  <head>
    <title>" + SUBJECT + @"</title>
    <style>
      table#auditTicketsTable {{
        border: 1px solid black;
        border-collapse: collapse;
      }}

      table#auditTicketsTable th, table#auditTicketsTable td {{
        border-top: 1px solid black;
        border-bottom: 1px solid black;
        padding: 4px;
      }}

      table#auditTicketsTable th {{
        border-left: 1px solid black;
        border-right: 1px solid black;
      }}

      table#auditTicketsTable td {{
        border-left: 1px dashed black;
        border-right: 1px dashed black;
      }}

      table#auditTicketsTable tbody tr:nth-child(even) td {{
        background: #e1eef7;
      }}

      table#auditTicketsTable tbody tr:hover td {{
        background: #83b9de;
      }}
    </style>
  </head>
  <body>
    <div>{0}</div>
    <div>
      <h3>Original Audit Message:</h3>
      <pre><code>{1}</code></pre>
    </div>
  </body>
</html>";

        #endregion

        #region Private Members

        private readonly IDeveloperEmailer _innerEmailer;
        private readonly IRepository<OneCallMarkoutTicket> _ticketRepository;

        #endregion

        #region Constructors

        public AuditFailureEmailer(IDeveloperEmailer innerEmailer, IRepository<OneCallMarkoutTicket> ticketRepository)
        {
            _innerEmailer = innerEmailer;
            _ticketRepository = ticketRepository;
        }

        #endregion

        #region Private Methods

        private string FormatMessage(OneCallMarkoutAudit audit)
        {
            return string.Format(HTML_MESSAGE_FORMAT, FormatAudit(audit), audit.FullText);
        }

        private string FormatAudit(OneCallMarkoutAudit audit)
        {
            var sb = new StringBuilder(LinkToAudit(audit));

            sb.AppendLine("<h3>Audit Information:</h3>");
            sb.AppendLine(String.Format("Transmitted At: {0}", audit.DateTransmitted));
            sb.AppendLine("<h4>Expected Tickets:</h4>");
            sb.AppendLine("<table id='auditTicketsTable'>");
            sb.AppendLine("<thead><tr>");
            sb.AppendLine("<th>Number</th><th>Type</th><th>In Database?</th><th>More Than One?</th><th>Type Matches?</th>");
            sb.AppendLine("</tr></thead>");
            sb.AppendLine("<tbody>");

            foreach (var num in audit.TicketNumbers)
            {
                var tickets = _ticketRepository.Where(t => t.RequestNumber == num.RequestNumber);

                sb.AppendLine("<tr>");
                sb.AppendFormat("<td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td>",
                    LinkToTickets(num.RequestNumber, tickets), num.MessageType, tickets.Any(), tickets.Count() > 1,
                    tickets.Any(t => t.MessageType == num.MessageType));
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</tbody>");
            sb.AppendLine("</table>");

            return sb.ToString();
        }

        private string LinkToTickets(int requestNumber, IQueryable<OneCallMarkoutTicket> tickets)
        {
            if (!tickets.Any())
            {
                return requestNumber.ToString();
            }

            var sb = new StringBuilder();

            tickets.EachWithIndex((t, i) => sb.Append(LinkToTicket(t, i)));

            return sb.ToString();
        }

        private static string LinkToTicket(OneCallMarkoutTicket ticket, int count)
        {
            return String.Format("<a href='http://mapcall.amwater.com/modules/mvc/FieldOperations/OneCallMarkoutTicket/Show/{0}'>{1}</a> ", ticket.Id,
                count == 0 ? ticket.RequestNumber.ToString() : "(" + count + ")");
        }

        private static string LinkToAudit(OneCallMarkoutAudit audit)
        {
            return
                String.Format(
                    "<a href='http://mapcall.amwater.com/modules/mvc/FieldOperations/OneCallMarkoutAudit/Show/{0}'>Link</a>",
                    audit.Id);
        }

        #endregion

        #region Exposed Methods

        public void SendMessage(OneCallMarkoutAudit audit)
        {
            _innerEmailer.SendMessage(SUBJECT, FormatMessage(audit), true);
        }

        #endregion

        public void Dispose()
        {
            _innerEmailer.Dispose();
        }
    }

    public interface IAuditFailureEmailer : IDisposable
    {
        #region Abstract Methods

        void SendMessage(OneCallMarkoutAudit audit);

        #endregion
    }
}
