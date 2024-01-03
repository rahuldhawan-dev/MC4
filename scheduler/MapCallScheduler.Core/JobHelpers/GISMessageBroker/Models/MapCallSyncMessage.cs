using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class MapCallSyncMessage
    {
        #region Constants

        public const string SCHEMA_VERSION = "1.1.0",
                            SOURCE_SYSTEM = "MAPCALL";

        public struct DataTypes
        {
            public const string SAMPLE_SITE = "SAMPLE_SITE_DATA",
                                SEWER_MAIN_CLEANING = "SEWER_MAIN_CLEANING_DATA",
                                SERVICE = "W1V_SERVICE_DATA";
        }

        #endregion

        #region Properties

        [Required]
        public string SchemaVersion => SCHEMA_VERSION;

        [Required]
        public string DataType { get; }

        [Required]
        public string SourceSystem => SOURCE_SYSTEM;

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SampleSite SampleSite { get; internal set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public SewerMainCleaning SewerMainCleaning { get; internal set; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public W1VServiceRecord W1VServiceRecord { get; internal set; }

        #endregion

        public MapCallSyncMessage(SampleSite sampleSite)
        {
            DataType = DataTypes.SAMPLE_SITE;
            SampleSite = sampleSite;
        }

        public MapCallSyncMessage(SewerMainCleaning sewerMainCleaning)
        {
            DataType = DataTypes.SEWER_MAIN_CLEANING;
            SewerMainCleaning = sewerMainCleaning;
        }

        public MapCallSyncMessage(W1VServiceRecord serviceRecord)
        {
            DataType = DataTypes.SERVICE;
            W1VServiceRecord = serviceRecord;
        }
    }
}
