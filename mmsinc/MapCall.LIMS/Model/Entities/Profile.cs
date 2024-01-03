using Newtonsoft.Json;

namespace MapCall.LIMS.Model.Entities
{
    public class Profile
    {
        [JsonProperty("PROFILE_NO")]
        public int Number { get; set; }

        [JsonProperty("PROFILE_NAME")]
        public string Name { get; set; }

        [JsonProperty("PWSID")]
        public string PublicWaterSupplyIdentifier { get; set; }

        [JsonProperty("TYPE")]
        public string AnalysisType { get; set; }
    }
}
