using System;
using MMSINC.Utilities.ActiveMQ;

namespace MapCallActiveMQListener.Library
{
    public abstract class ActiveMQThingerBase
    {
        #region Private Members

        protected readonly IActiveMQConfiguration _configuration;
        protected readonly IActiveMQServiceFactory _amqFactory;

        #endregion

        #region Constructors

        public ActiveMQThingerBase(IActiveMQConfiguration configuration, IActiveMQServiceFactory amqFactory)
        {
            _configuration = configuration;
            _amqFactory = amqFactory;
        }

        #endregion

        #region Private Methods

        protected virtual void WithService(Action<IActiveMQService> fn)
        {
            fn(_amqFactory.Build(_configuration));
        }

        #endregion
    }
}