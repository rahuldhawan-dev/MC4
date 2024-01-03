using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories.Users;
using MMSINC.Data.NHibernate;
using MMSINC.Interface;
using MMSINC.Utilities;
using IContainer = StructureMap.IContainer;

namespace MapCallScheduler.JobHelpers.MarkoutTickets
{
    public class MessageParser
    {
        internal readonly DependencyResolver _dependencyResolver;
        
        #region Constructors

        public MessageParser(DependencyResolver dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        #endregion
        
        #region Nested Type: DependencyResolver

        public class DependencyResolver
        {
            #region Private Members

            private readonly IContainer _container;

            #endregion

            #region Constructors

            public DependencyResolver(IContainer container)
            {
                _container = container;
            }

            #endregion

            #region Exposed Methods

            public TEntity Resolve<TEntity>(Expression<Func<TEntity, bool>> where)
            {
                return _container.GetInstance<IRepository<TEntity>>().Where(where).Single();
            }

            #endregion
        }

        #endregion
    }

    /// <summary>
    /// Parses OneCallMarkoutTickets from OneCall email messages. Email
    /// messages should be validated before this class receives them.
    /// </summary>
    public class MarkoutTicketParser : MessageParser, IMarkoutTicketParser
    {
        #region Constants

        public struct Regexes
        {
            #region Constants

            public static readonly Regex NUMBER = new Regex(@"\*\*\* Request No.: (\d+)"),
                TYPE = new Regex(@"\*\*\*\s+(U P D A T E|R O U T I N E|E M E R G E N C Y)"),
                COUNTY = new Regex(@"County:\s+([^\r\n]+)\s+Municipality", RegexOptions.Multiline),
                MUNICIPALITY = new Regex(@"\s+Municipality:\s+([^\r\n]+)\s*", RegexOptions.Multiline),
                STREET = new Regex(@"^Street:\s+([^\r\n]+)\s*", RegexOptions.Multiline),
                CROSS_STREET = new Regex(@"^Nearest Intersection:\s+([^\r\n]+)\s*", RegexOptions.Multiline),
                DATE_RECEIVED = new Regex(@"^Transmit:\s+Date:\s+([0-9/]+)\s+At:\s+([0-9:]+)", RegexOptions.Multiline),
                OPERATING_CENTER = new Regex(@"\s+CDC\s+=\s+(?:SC)?(.{3})"),
                RELATED_REQUEST_NUMBER = new Regex(@"Of\s+Request\s+No.:\s+(\d+)"),
                TYPE_OF_WORK = new Regex(@"^Type\s+of\s+Work:\s+([^\r\n]+)\s*$", RegexOptions.Multiline),
                WORKING_FOR = new Regex(@"^Working\s+For:\s+([^\r\n]+)\s*$", RegexOptions.Multiline),
                EXCAVATOR = new Regex(@"^Excavator:\s+([^\r\n]+)\s*$", RegexOptions.Multiline),
                CDC_CODE = new Regex(@"CDC\s+=\s+([^\s]+)"),
                COMPANY_IS_EXCAVATOR = new Regex("(?:N ?J|NEW JERSEY)(?:AM| AMERI?CAN) (?:WATER|WATRE|WTR)(?: CO(?:.|MP(?:ANY)?)?)?");

            #endregion
        }

        public const string CANCELLATION = "*** Cancellation Of Request No.:";

        #endregion

        #region Private Members

        private readonly IUserRepository _userRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public MarkoutTicketParser(DependencyResolver dependencyResolver, IUserRepository userRepository, IDateTimeProvider dateTimeProvider) : base(dependencyResolver)
        {
            _userRepository = userRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Static Methods

        public static string ParseSingleString(string message, Regex regex)
        {
            var match = regex.Match(message);
            return match.Success ? match.Groups[1].Value.Trim() : null;
        }

        public static TReturn ParseSingleString<TReturn>(string message, Regex regex)
        {
            var result = ParseSingleString(message, regex);
            return string.IsNullOrEmpty(result)
                ? default(TReturn)
                : (TReturn)TypeDescriptor.GetConverter(typeof(TReturn)).ConvertFromString(result);
        }

        public static DateTime ParseDateReceived(string message)
        {
            var match = Regexes.DATE_RECEIVED.Match(message);
            var dateString = match.Groups[1].Value + " " + match.Groups[2].Value;

            return Convert.ToDateTime(dateString);
        }

        #endregion

        #region Exposed Methods

        public OneCallMarkoutTicket Parse(IMailMessage message)
        {
            var typeDesc = message.Body.Contains(CANCELLATION)
                ? "CANCELLED"
                : ParseSingleString(message.Body, Regexes.TYPE).Replace(" ", "");

            var ticket = new OneCallMarkoutTicket {
                MessageType = _dependencyResolver.Resolve<OneCallMarkoutMessageType>(t => t.Description == typeDesc),
                RequestNumber =  ParseSingleString<int>(message.Body, Regexes.NUMBER),
                RelatedRequestNumber = ParseSingleString<int?>(message.Body, Regexes.RELATED_REQUEST_NUMBER),
                CountyText = ParseSingleString(message.Body, Regexes.COUNTY),
                TownText = ParseSingleString(message.Body, Regexes.MUNICIPALITY),
                StreetText = ParseSingleString(message.Body, Regexes.STREET),
                NearestCrossStreetText = ParseSingleString(message.Body, Regexes.CROSS_STREET),
                DateTransmitted = ParseDateReceived(message.Body),
                DateReceived = _dateTimeProvider.GetCurrentDate(),
                OperatingCenter = _dependencyResolver.Resolve<OperatingCenter>(x => x.OperatingCenterCode == FixOpCode(ParseSingleString(message.Body, Regexes.OPERATING_CENTER))),
                TypeOfWork = ParseSingleString(message.Body, Regexes.TYPE_OF_WORK),
                WorkingFor = ParseSingleString(message.Body, Regexes.WORKING_FOR),
                Excavator = ParseSingleString(message.Body, Regexes.EXCAVATOR),
                CDCCode = ParseSingleString(message.Body, Regexes.CDC_CODE),
                FullText = message.Body
            };

            if (Regexes.COMPANY_IS_EXCAVATOR.IsMatch(ticket.Excavator))
            {
                ticket.Responses.Add(new OneCallMarkoutResponse {
                    CompletedAt = _dateTimeProvider.GetCurrentDate(),
                    CompletedBy = _userRepository.GetUserByUserName("mcadmin"),
                    OneCallMarkoutTicket = ticket
                });
            }

            return ticket;
        }

        private string FixOpCode(string opCode)
        {
            return opCode.Replace("SHW", "NJ7");
        }

        #endregion
    }
}