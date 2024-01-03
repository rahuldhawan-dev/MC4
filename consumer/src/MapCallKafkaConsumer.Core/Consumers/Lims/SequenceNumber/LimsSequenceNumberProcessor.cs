using log4net;
using MapCallKafkaConsumer.Library;
using MapCall.Common.Model.Entities;
using MapCall.LIMS.Model.Entities;
using MMSINC.Data;

namespace MapCallKafkaConsumer.Consumers.Lims.SequenceNumber
{
    public class LimsSequenceNumberProcessor : MessageToEntityProcessorBase<Location, SampleSite>, ILimsSequenceNumberProcessor
    {
        #region Constructors

        public LimsSequenceNumberProcessor(ILog logger, IUnitOfWorkFactory unitOfWorkFactory) : base(logger, unitOfWorkFactory) { }

        #endregion

        #region Exposed Methods

        public override SampleSite RetrieveEntity(IUnitOfWork unitOfWork, Location location)
        {
            return int.TryParse(location.SampleSiteId, out var sampleSiteId)
                ? unitOfWork.GetRepository<SampleSite>().Find(sampleSiteId)
                : null;
        }

        public override SampleSite MapMessageToEntity(Location location, SampleSite sampleSite)
        {
            if (location.LocationSequenceNumber.HasValue &&
                location.LocationSequenceNumber.Value != sampleSite.LimsSequenceNumber)
            {
                sampleSite.LimsSequenceNumber = location.LocationSequenceNumber;
            }
            
            return sampleSite;
        }

        #endregion  
    }
}
