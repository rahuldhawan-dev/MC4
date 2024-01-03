using log4net;
using MapCall.Common.Model.Entities;
using MapCallKafkaConsumer.Library;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Configuration;
using MMSINC.Utilities.Kafka.Consumer;

namespace MapCallKafkaConsumer.Consumers.Ignition.SystemDelivery
{
    public class IgnitionSystemDeliveryConsumer : ConsumerBase<IIgnitionSystemDeliveryProcessor, Model.SystemDelivery, SystemDeliveryIgnitionEntry>
    {
        #region Properties

        // TODO: We need this topic name
        public override string Topic => "ignition_sys_delivery";

        public override string Identifier => nameof(IgnitionSystemDeliveryConsumer);

        #endregion

        #region Constructors

        public IgnitionSystemDeliveryConsumer(
            ILog logger, 
            IIgnitionConfiguration configuration,
            IKafkaServiceFactory<IKafkaConsumer> serviceFactory, 
            IIgnitionSystemDeliveryProcessor processor) : 
            base(logger, 
                configuration, 
                serviceFactory, 
                processor) { }

        #endregion
    }
}
