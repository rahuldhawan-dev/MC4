using System;
using System.Collections;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkOrderPurpose : ReadOnlyEntityLookup
    {
        #region Consts

        public new struct StringLengths
        {
            public const int CODE = 5, CODE_GROUP = 10;
        }

        #endregion

        // TODO: Either name this better or change this from an enum.
        public enum Indices
        {
            CUSTOMER = 1,
            COMPLIANCE = 3,
            SAFETY = 4,
            LEAK_DETECTION = 5,
            REVENUE_150_TO_500 = 6,
            REVENUE_500_TO_1000 = 7,
            REVENUE_ABOVE_1000 = 8,
            DAMAGED_BILLABLE = 9,
            ESTIMATES = 10,
            WATER_QUALITY = 11,
            ASSET_RECORD_CONTROL = 13,
            SEASONAL = 14,
            DEMOLITION = 15,
            BPU = 16,
            HURRICANE_SANDY = 18,
            CONSTRUCTION_PROJECT = 19,
            EQUIP_RELIABILITY = 20
        }

        public static IEnumerable<Indices> REVENUE = new[] {
            Indices.REVENUE_150_TO_500,
            Indices.REVENUE_500_TO_1000,
            Indices.REVENUE_ABOVE_1000
        };

        public virtual bool IsProduction { get; set; }
        public virtual string SapCode { get; set; }
        public virtual string CodeGroup { get; set; }
    }
}
