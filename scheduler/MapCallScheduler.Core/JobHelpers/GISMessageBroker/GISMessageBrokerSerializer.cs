using MapCallScheduler.JobHelpers.GISMessageBroker.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Entity = MapCall.Common.Model.Entities;

namespace MapCallScheduler.JobHelpers.GISMessageBroker
{
    public class GISMessageBrokerSerializer : IGISMessageBrokerSerializer
    {
        public string Serialize(Entity.SampleSite site, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(
                new MapCallSyncMessage(SampleSite.FromDbRecord(site)),
                new JsonSerializerSettings {
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }

        public string Serialize(Entity.SewerMainCleaning sewerMainCleaning, Formatting formatting = Formatting.None)
        {
            return JsonConvert.SerializeObject(
                new MapCallSyncMessage(SewerMainCleaning.FromDbRecord(sewerMainCleaning)),
                new JsonSerializerSettings {
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }

        public string Serialize(Entity.Service service, Formatting formatting = Formatting.None)
        {
            var contractResolver = new DefaultContractResolver {
                NamingStrategy = new CamelCaseNamingStrategy()
            };
            
            return JsonConvert.SerializeObject(
                new MapCallSyncMessage(W1VServiceRecord.FromDbRecord(service)),
                new JsonSerializerSettings {
                    ContractResolver = contractResolver,
                    Formatting = formatting,
                    DateFormatString = "o"
                });
        }
    }
}
