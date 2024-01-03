using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EventSubcategory : EntityLookup
    {
        public struct Indices
        {
            public const int BACKFLOW_EVENT = 1,
                             BUSINESS_CONTINUITY_PLAN = 2,
                             CHEMICAL_RELEASE_GAS = 3,
                             CHEMICAL_RELEASE_LIQUID = 4,
                             CONTAMINATION = 5,
                             CYANOTOXIN_RESPONSE_PLAN = 6,
                             DISCHARGE = 7,
                             EQUIPMENT_FAILURE = 8,
                             ERP_EXERCISE = 9,
                             FLOODING = 10,
                             MAIN_BREAK = 11,
                             MEDIA_ARTICLE_PFAS = 12,
                             NETWORK_PIPING_LIMITATIONS = 13,
                             POTENTIAL_NOV_MCL_TIER_I = 14,
                             POTENTIAL_NOV_MCL_TIER_II = 15,
                             POTENTIAL_NOV_REPORTING = 16,
                             PROCESS_FAILURE = 17,
                             REPAIR_MAIN = 18,
                             REPAIR_SERVICE = 19,
                             RESPONSE_MITIGATION = 20,
                             SECURITY = 21,
                             SOURCE_WATER_CONTAMINATION = 22,
                             SOURCE_WATER_ALGAL_BLOOM = 23,
                             SYSTEM_DEMANDS = 24,
                             THREAT = 25,
                             TREATMENT = 26,
                             WASTEWATER_OVERFLOW = 27,
                             WEATHER = 28,
                             WQ_LEGIONELLA = 29;
        }

        [Required]
        [StringLength(50)]
        public override string Description { get; set; }
    }
}
