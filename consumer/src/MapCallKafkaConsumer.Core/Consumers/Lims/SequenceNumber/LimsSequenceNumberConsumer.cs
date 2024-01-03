using log4net;
using MapCall.Common.Model.Entities;
using MapCall.LIMS.Model.Entities;
using MapCallKafkaConsumer.Library;
using MMSINC.Utilities.Kafka;
using MMSINC.Utilities.Kafka.Consumer;

namespace MapCallKafkaConsumer.Consumers.Lims.SequenceNumber
{
    public class LimsSequenceNumberConsumer : ConsumerBase<ILimsSequenceNumberProcessor, Location, SampleSite>
    {
        #region Properties

        public override string Topic => "com.amwater.lims.location.topic";

        public override string Identifier => nameof(LimsSequenceNumberConsumer);

        #endregion

        #region Constructors

        public LimsSequenceNumberConsumer(
            ILog logger,
            ILimsConfiguration configuration,
            IKafkaServiceFactory<IKafkaConsumer> serviceFactory,
            ILimsSequenceNumberProcessor processor)
            : base(
                logger,
                configuration,
                serviceFactory,
                processor) { }

        #endregion
    }
}
