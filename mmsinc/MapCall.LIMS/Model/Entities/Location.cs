using Newtonsoft.Json;

namespace MapCall.LIMS.Model.Entities
{
    public class Location
    {
        [JsonProperty("LOCATION_SEQ")]
        public int? LocationSequenceNumber { get; set; }

        [JsonProperty("LOCATION")]
        public string LocationName { get; set; }

        [JsonProperty("LOCATION_DESC")]
        public string LocationDescription { get; set; }

        [JsonProperty("ACTIVE")]
        public string Active { get; set; }

        [JsonProperty("SAMPLESITEID")]
        public string SampleSiteId { get; set; }

        [JsonProperty("CLIENT_ID")]
        public string ClientId { get; set; }

        [JsonProperty("PROFILE_NO")]
        public string ProfileNumber { get; set; }

        [JsonProperty("FACILITY_ID")]
        public string FacilityId { get; set; }

        [JsonProperty("SITE_ID")]
        public string SiteId { get; set; }

        [JsonProperty("PSCODE")]
        public string PrimaryStationCode { get; set; }

        [JsonProperty("ADDRESS")]
        public string Address { get; set; }

        [JsonProperty("CITY")]
        public string City { get; set; }

        [JsonProperty("STATE")]
        public string State { get; set; }

        [JsonProperty("ZIP")]
        public string Zip { get; set; }

        [JsonProperty("LATITUDE")]
        public string Latitude { get; set; }

        [JsonProperty("LONGITUDE")]
        public string Longitude { get; set; }

        [JsonProperty("PWSID")]
        public string PublicWaterSupplyIdentifier { get; set; }
    }
}
