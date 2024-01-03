using System;
using System.Collections.Generic;

namespace MMSINC.Utilities.ActiveMQ
{
    public interface IActiveMQService
    {
        void SendMessage(string topic, string message);
        void SendMessage(string topic, string message, Dictionary<string, object> properties);

        Action ReceiveMessages(string topic, Action<IMessage> onMessageReceived);
    }
}
