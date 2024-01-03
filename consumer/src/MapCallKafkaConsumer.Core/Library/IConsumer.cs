namespace MapCallKafkaConsumer.Library
{
    public interface IConsumer
    {
        string Identifier { get; }
        void Start();
        void Stop();
    }
}
