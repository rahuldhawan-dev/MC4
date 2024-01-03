using System;

namespace MMSINC.Utilities.ActiveMQ
{
    public interface IMessage
    {
        string Text { get; }
        void Acknowledge();
    }
}
