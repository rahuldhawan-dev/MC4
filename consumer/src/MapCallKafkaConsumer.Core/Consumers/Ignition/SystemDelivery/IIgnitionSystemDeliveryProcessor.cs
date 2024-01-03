using MapCall.Common.Model.Entities;
using MapCallKafkaConsumer.Library;

namespace MapCallKafkaConsumer.Consumers.Ignition.SystemDelivery
{
    public interface IIgnitionSystemDeliveryProcessor : IMessageToEntityProcessor<Model.SystemDelivery, SystemDeliveryIgnitionEntry> { }
}
