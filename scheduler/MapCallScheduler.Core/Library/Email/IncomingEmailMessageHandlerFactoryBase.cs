using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapCallScheduler.Metadata;
using MMSINC.ClassExtensions.MemberInfoExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Interface;
using StructureMap;

namespace MapCallScheduler.Library.Email
{
    public abstract class IncomingEmailMessageHandlerFactoryBase<THandlerBase> : IIncomingEmailMessageHandlerFactory where THandlerBase : IIncomingEmailMessageHandler
    {
        #region Private Members

        protected IContainer _container;

        #endregion

        #region Abstract Properties

        public abstract Type JunkMessageHandlerType { get; }

        #endregion

        #region Constructors

        protected IncomingEmailMessageHandlerFactoryBase(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        protected virtual IIncomingEmailMessageHandler ConstructHandler(IWrappedImapClient imapClient, Type handler)
        {
            var argType = handler.GetGreediestConstructor().GetParameters().First().ParameterType;

            return
                (IIncomingEmailMessageHandler)
                _container
                    .With("args")
                    .EqualTo(_container
                        .With("imapClient")
                        .EqualTo(imapClient)
                        .GetInstance(argType))
                    .GetInstance(handler);
        }

        protected virtual bool ExtraTypeFilter(Type type)
        {
            return true;
        }

        protected virtual IOrderedEnumerable<Type> GetHandlerTypeOrder(IEnumerable<Type> filtered)
        {
            return filtered.OrderBy(_ => 1);
        }

        // TODO: fix this so it returns the first handler in order that matches, rather than
        // matching the whole set of them and then ordering and getting the first
        protected Type GetHandlerType(IMailMessage message)
        {
            var handlerBaseType = typeof(THandlerBase);
            var filtered = handlerBaseType.Assembly.GetTypes().Where(h => TypeMatches(h, message, handlerBaseType));
            return filtered.Any() ? GetHandlerTypeOrder(filtered).First() : JunkMessageHandlerType;
        }

        private bool TypeMatches(Type testType, IMailMessage message, Type handlerBaseType)
        {
            return !testType.IsAbstract && testType.Namespace == handlerBaseType.Namespace &&
                        testType.HasAttribute<HandlesMessageAttribute>() &&
                        testType.GetCustomAttribute<HandlesMessageAttribute>().Predicate(message) &&
                        ExtraTypeFilter(testType);
        }

        #endregion

        #region Exposed Methods

        public virtual IIncomingEmailMessageHandler GetHandler(IWrappedImapClient imapClient, IMailMessage message)
        {
            return ConstructHandler(imapClient, GetHandlerType(message));
        }

        #endregion
    }
}