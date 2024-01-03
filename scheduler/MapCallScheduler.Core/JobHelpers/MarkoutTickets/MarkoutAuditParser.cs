using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    /// <summary>
    /// Parses OneCall email messages to OneCallMarkoutAudits complete with
    /// ticket numbers.  Email messages should be validated before this class
    /// receives them.
    /// </summary>
    public class MarkoutAuditParser : IMarkoutAuditParser
    {
        #region Constants

        public const string UNKNOWN_MESSAGE_TYPE_FORMAT = "Unrecognized message type abbreviation: {0}";

        public struct Regexes
        {
            #region Constants

            public static readonly Regex DATE_TRANSMITTED = new Regex(@"^Date/Time\s+:\s+([0-9\-]+)\s+at\s+([0-9:]+)",
                RegexOptions.Multiline),
                CDC_CODE = new Regex(@"Receiving\s+Terminal:\s+([A-Z0-9]+)"),
                TICKET_NUMBER = new Regex(@"\s+\d+\s+(\d+)-([A-Z]{4})\|");

            #endregion
        }

        #endregion

        #region Private Members

        private readonly IRepository<OneCallMarkoutMessageType> _repository;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public MarkoutAuditParser(IRepository<OneCallMarkoutMessageType> repository, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        private IEnumerable<OneCallMarkoutAuditTicketNumber> GetTicketNumbers(string message)
        {
            foreach (Match match in Regexes.TICKET_NUMBER.Matches(message))
            {
                var messageType = ParseMessageType(match);
                var query = _repository.Where(
                    t =>
                        t.Description.ToLower()
                            .StartsWith(messageType));

                if (!query.Any())
                {
                    throw new InvalidDataException(String.Format(UNKNOWN_MESSAGE_TYPE_FORMAT, messageType));
                }

                yield return new OneCallMarkoutAuditTicketNumber {
                    RequestNumber = ParseRequestNumber(match),
                    MessageType = query.Single()
                };
            }
        }

        private static string ParseMessageType(Match match)
        {
            return match.Groups[2].Value.ToLower()
                .Replace("updt", "upda")
                // actually "emer retransmission"
                .Replace("ertr", "emer")
                .Replace("retr", "rout");
        }

        private static int ParseRequestNumber(Match match)
        {
            return Convert.ToInt32(match.Groups[1].Value);
        }

        private static DateTime ParseDateTransmitted(string message)
        {
            var match = Regexes.DATE_TRANSMITTED.Match(message);
            var dateString = match.Groups[1].Value + " " + match.Groups[2].Value;

            return Convert.ToDateTime(dateString);
        }

        #endregion

        #region Exposed Methods

        public OneCallMarkoutAudit Parse(IMailMessage message)
        {
            if (!Regexes.CDC_CODE.IsMatch(message.Body))
            {
                throw new InvalidOperationException(String.Format("No cdc code found in audit message:{0}{1}",
                    Environment.NewLine, message.Body));
            }

            var cdcCode = Regexes.CDC_CODE.Match(message.Body).Groups[1].Value.Trim();
            var audit = new OneCallMarkoutAudit {
                DateTransmitted = ParseDateTransmitted(message.Body),
                DateReceived = _dateTimeProvider.GetCurrentDate(),
                FullText = message.Body
            };

            foreach (var ticketNum in GetTicketNumbers(message.Body))
            {
                ticketNum.Audit = audit;
                ticketNum.CDCCode = cdcCode;
                audit.TicketNumbers.Add(ticketNum);
            }

            return audit;
        }

        #endregion
    }
}