using System;
using System.Collections.Generic;

namespace MMSINC.Utilities.ActiveMQ
{
    public abstract class ActiveMQServiceBase : IActiveMQService
    {
        #region Private Members

        protected readonly IActiveMQConfiguration _config;

        #endregion

        public ActiveMQServiceBase(IActiveMQConfiguration config)
        {
            _config = config;
        }

        public abstract void SendMessage(string topic, string message);
        public abstract void SendMessage(string topic, string message, Dictionary<string, object> properties);

        public abstract Action ReceiveMessages(string topic, Action<IMessage> onMessageReceived);
    }
}
