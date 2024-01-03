using System;

namespace MMSINC.Utilities.Kafka
{
    /// <summary>
    /// Specifies that the implementing class intends to act as a Kafka
    /// consumer or producer. 
    /// </summary>
    public interface IKafkaService : IDisposable { }
}
