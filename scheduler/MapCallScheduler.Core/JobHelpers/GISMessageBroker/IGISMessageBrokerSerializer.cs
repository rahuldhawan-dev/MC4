using MapCall.Common.Model.Entities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GISMessageBroker
{
    public interface IGISMessageBrokerSerializer
    {
        #region Abstract Methods

        string Serialize(SampleSite siteValue, Formatting formatting = Formatting.None);

        string Serialize(SewerMainCleaning sewerMainCleaning, Formatting formatting = Formatting.None);
        
        string Serialize(Service service, Formatting formatting = Formatting.None);

        #endregion
    }
}
