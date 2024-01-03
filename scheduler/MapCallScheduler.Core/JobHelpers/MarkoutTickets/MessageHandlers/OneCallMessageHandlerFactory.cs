using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Email;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using StructureMap;

namespace MapCallScheduler.JobHelpers.MarkoutTickets.MessageHandlers
{
    /// <summary>
    /// Finds a handler for the given message which can handle the message
    /// based on subject.  Invalid email addresses are passed off to the
    /// JunkMessageHandler here.
    /// </summary>
    public class OneCallMessageHandlerFactory : IncomingEmailMessageHandlerWithExpectedSenderFactoryBase<IOneCallMessageHandler>, IOneCallMessageHandlerFactory
    {
        #region Constants

        public const string EXPECTED_SENDER = "nj@occinc.com";

        #endregion

        #region Properties

        public override Type JunkMessageHandlerType => typeof(JunkMessageHandler);

        public override string ExpectedSender => EXPECTED_SENDER;

        #endregion

        #region Constructors

        public OneCallMessageHandlerFactory(IContainer container) : base(container) {}

        #endregion

        protected override bool ExtraTypeFilter(Type type)
        {
            return type.HasAttribute<HandlesSubjectAttribute>();
        }

        protected override IOrderedEnumerable<Type> GetHandlerTypeOrder(IEnumerable<Type> filtered)
        {
            return filtered.OrderByDescending(h => h.GetCustomAttribute<HandlesSubjectAttribute>().Regex.Length);
        }
    }
}