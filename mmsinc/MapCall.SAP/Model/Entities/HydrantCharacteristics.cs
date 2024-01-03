using System;
using System.ComponentModel;
using MapCall.Common.Model.Entities;

namespace MapCall.SAP.Model.Entities
{
    /// <summary>
    /// Class - HYD Characteristics
    /// </summary>
    [Serializable]
    public class HydrantCharacteristics
    {
        #region properties

        #region Logical properties

        public virtual string HydrantBillingType
        {
            get
            {
                switch (HYD_BILLING_TP)
                {
                    case "PRIVATE":
                        return "PRIVATE FIRE";
                    case "MUNICIPAL":
                    case "PUBLIC":
                        return "PUBLIC FIRE";
                    case "COMPANY":
                        return "UNBILLED AW HYDRANT";
                    default:
                        return string.Empty;
                }
            }
        }

        public virtual string HYD_AUXILLARY_VALVE => HYD_AUX_VALVENUM != null ? "Y" : "N";

        public virtual string SteamerThreadType
        {
            get
            {
                switch (HYD_STEAMER_THREAD_TP)
                {
                    case "BBT":
                        return "BBT";
                    case "MHT":
                        return "MHT";
                    case "NST":
                        return "NST";
                    case "NYC":
                        return "NYC";
                    case "PRT":
                        return "PRT";
                    case "STORZ 4IN":
                    case "STORZ 5IN":
                        return "STORZ";
                    default:
                        return string.Empty;
                }
            }
        }

        #endregion

        public virtual string OWNED_BY { get; set; }
        public virtual string SPECIAL_MAINT_NOTES_DIST { get; set; }
        public virtual string SPECIAL_MAINT_NOTES_DETAILS { get; set; }
        public virtual string DEPENDENCY_DRIVER_1 { get; set; }
        public virtual string DEPENDENCY_DRIVER_2 { get; set; }
        public virtual string SUB_DIVISION { get; set; }
        public virtual string HYD_AUX_VALVENUM { get; set; }
        public virtual string PRESSURE_ZONE { get; set; }
        public virtual string PRESSURE_ZONE_HGL { get; set; }
        public virtual double NORMAL_SYS_PRESSURE { get; set; }
        public virtual string MAP_PAGE { get; set; }
        public virtual string BOOK_PAGE { get; set; }
        public virtual string OPEN_DIRECTION { get; set; }
        public virtual string HYD_BARREL_SIZE { get; set; }
        public virtual string EAM_HYD_BURY_DEPTH { get; set; }

        [DisplayName("HYD_EXTENSION-SIZES")]
        public virtual string HYD_EXTENSION_SIZES { get; set; }

        public virtual string HYD_BRANCH_LENGTH { get; set; }
        public virtual string HYD_AUX_VALVE_BRANCH_SIZE { get; set; }
        public virtual string HYD_DEAD_END_MAIN { get; set; }
        public virtual string HYD_OUTLET_CONFIG { get; set; }
        public virtual string HYD_SIDE_NOZZLE_SIZE { get; set; }
        public virtual string HYD_SIDE_PORT_THREAD_TP { get; set; }
        public virtual string HYD_STEAMER_SIZE { get; set; }
        public virtual string HYD_STEAMER_THREAD_TP { get; set; }
        public virtual string HYD_LOCK_DEVICE_TP { get; set; }
        public virtual string HYD_COLOR_CODE_METHOD { get; set; }
        public virtual string HYD_COLOR_CODE_TP { get; set; }
        public virtual string HYD_COLORCODE { get; set; }
        public virtual string HYD_STEM_LUBE { get; set; }
        public virtual string HYD_REPAIR_KIT { get; set; }
        public virtual string PRESSURE_CLASS { get; set; }
        public virtual string JOINT_TP { get; set; }
        public virtual string EAM_PIPE_SIZE { get; set; }
        public virtual string PIPE_MATERIAL { get; set; }
        public virtual string INSTALLATION_WO { get; set; }
        public virtual string SKETCH_NUM { get; set; }
        public virtual string HYD_FIRE_DISTRICT { get; set; }
        public virtual string HYD_ACCOUNT { get; set; }
        public virtual string HYD_BILLING_TP { get; set; }
        public virtual string HISTORICAL_ID { get; set; }
        public virtual string LAM_GEOACCURACY { get; set; }
        public virtual string HYD_TYP { get; set; }

        #endregion

        #region Constructors

        public HydrantCharacteristics() { }

        public HydrantCharacteristics(Hydrant hydrant)
        {
            SPECIAL_MAINT_NOTES_DETAILS = ""; // hydrant.StringLengths.CRITICAL_NOTES
            PRESSURE_ZONE = ""; //hydrant.Gradient
            MAP_PAGE = ""; //hydrant.StringLengths.MAP_PAGE
            OPEN_DIRECTION = ""; //hydrant.OpensDirection
            HYD_BARREL_SIZE = ""; //hydrant.HydrantSize
            EAM_HYD_BURY_DEPTH = ""; //hydrant.DepthBuryFeet + hydrant.DepthBuryInches
            HYD_BRANCH_LENGTH = ""; //hydrant.BranchLengthFeet + hydrant.BranchLengthInches
            HYD_AUX_VALVE_BRANCH_SIZE = ""; //hydrant.LateralSize
            HYD_DEAD_END_MAIN = ""; //hydrant.IsDeadEndMain
            HYD_AUX_VALVENUM = ""; //hydrant.LateralSize
            EAM_PIPE_SIZE = ""; //hydrant.HydrantMainSize
            PIPE_MATERIAL = ""; //hydrant.MainType
            INSTALLATION_WO = ""; //hydrant.WorkOrderNumber
            HYD_FIRE_DISTRICT = ""; //hydrant.FireDistrict
            HYD_ACCOUNT = ""; //hydrant.StringLengths.PREMISE_NUMBER
            HYD_BILLING_TP = ""; //hydrant.HydrantBilling
            HISTORICAL_ID = ""; //hydrant.HydrantNumber
            HYD_TYP = ""; //Hydrant Type
        }

        #endregion
    }
}
