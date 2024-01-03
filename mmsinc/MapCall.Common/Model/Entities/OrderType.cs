using System;
using System.Collections.Generic;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OrderType : EntityLookup
    {
        #region Constants

        public struct StringLengths
        {
            public const int DESCRIPTION = 50, SAP_CODE = 2;
        }

        public struct Indices
        {
            public const int
                OPERATIONAL_ACTIVITY_10 = 1,
                PLANT_MAINTENANCE_WORK_ORDER_11 = 2,
                CORRECTIVE_ACTION_20 = 3,
                RP_CAPITAL_40 = 4,
                ROUTINE_13 = 5;
        }

        public struct SAPCodes
        {
            public const string
                OPERATIONAL_ACTIVITY_10 = "0010",
                PLANT_MAINTENANCE_WORK_ORDER_11 = "0011",
                CORRECTIVE_ACTION_20 = "0020",
                RP_CAPITAL_40 = "0040",
                ROUTINE_13 = "0013";
        }

        public static readonly int[] COMPLIANCE_ORDER_TYPES = new[] {
            Indices.PLANT_MAINTENANCE_WORK_ORDER_11,
            Indices.ROUTINE_13
        };

        #endregion

        #region Properties

        public virtual string SAPCode { get; set; }
        public virtual bool IsSAPEnabled { get; set; }

        #endregion

        #region Logical Properties

        public virtual string Display => $"{SAPCode} - {Description}";

        #endregion

        public override string ToString() => Display;
    }
}
