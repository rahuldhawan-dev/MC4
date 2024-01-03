using MapCallActiveMQListener.Library;
using MMSINC.Utilities.ActiveMQ;

namespace MapCallActiveMQListener
{
    public class Sender : ActiveMQThingerBase, ISender
    {
        #region Constructors

        public Sender(IActiveMQConfiguration configuration, IActiveMQServiceFactory factory) : base(configuration, factory) {}

        #endregion

        #region Exposed Methods

        public void Send(string topic, string message)
        {
            WithService(service => service.SendMessage(topic, message));
        }

        #endregion
    }
}