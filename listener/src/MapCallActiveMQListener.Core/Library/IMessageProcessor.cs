using MMSINC.Utilities.ActiveMQ;

namespace MapCallActiveMQListener.Library
{
    public interface IMessageProcessor
    {
        void Process(IActiveMQService service, IMessage message, string topic);
    }
}
