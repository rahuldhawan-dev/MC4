using log4net;
using MapCallKafkaConsumer.Library;
using MapCall.Common.Model.Entities;
using MapCall.LIMS.Model.Entities;
using MMSINC.Data;

namespace MapCallKafkaConsumer.Consumers.Lims.SequenceNumber
{
    public interface ILimsSequenceNumberProcessor : IMessageToEntityProcessor<Location, SampleSite> { }
}
