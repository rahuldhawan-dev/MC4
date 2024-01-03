using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class MapCallSyncMessage
    {
        #region Constants

        public const string SCHEMA_VERSION = "1.0.5",
                            SOURCE_SYSTEM = "MAPCALL";

        public struct DataTypes
        {
            #region Constants

            public const string HYDRANT = "HYDRANT_DATA",
                                VALVE = "VALVE_DATA",
                                SEWER_OPENING = "SEWER_OPENING_DATA",
                                SERVICE = "SERVICE_DATA",
                                AS_BUILT_IMAGE = "AS_BUILT_IMAGE_DATA";
            #endregion
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
        public IEnumerable<Hydrant> Hydrants { get; internal set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Valve> Valves { get; internal set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<SewerOpening> SewerOpenings { get; internal set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<Service> Services { get; internal set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public IEnumerable<AsBuiltImage> AsBuiltImages { get; internal set; }

        #endregion

        #region Constructors

        public MapCallSyncMessage(IEnumerable<Hydrant> hydrants)
        {
            DataType = DataTypes.HYDRANT;
            Hydrants = hydrants;
        }

        public MapCallSyncMessage(IEnumerable<Valve> valves)
        {
            DataType = DataTypes.VALVE;
            Valves = valves;
        }

        public MapCallSyncMessage(IEnumerable<SewerOpening> openings)
        {
            DataType = DataTypes.SEWER_OPENING;
            SewerOpenings = openings;
        }

        public MapCallSyncMessage(IEnumerable<Service> services)
        {
            DataType = DataTypes.SERVICE;
            Services = services;
        }

        public MapCallSyncMessage(IEnumerable<AsBuiltImage> asBuiltImages)
        {
            DataType = DataTypes.AS_BUILT_IMAGE;
            AsBuiltImages = asBuiltImages;
        }
        
        #endregion
    }
}
