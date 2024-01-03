using MMSINC.Utilities.Kafka.Configuration;

namespace MMSINC.Utilities.Kafka
{
    public interface IKafkaServiceFactory<out TKafkaService> where TKafkaService : IKafkaService
    {
        TKafkaService Build(IKafkaConfiguration config);
    }
}
