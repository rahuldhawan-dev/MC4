using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class WorkDescription : IEntityLookup
    {
        #region Constants

        public struct TimeEstimates
        {
            public const string LESS_THAN_HOUR = "< 1",
                                LESS_THAN_TWO_HOURS = "< 2",
                                GREATER_THAN_TWO_HOURS = "> 2";
        }

        public struct StringLengths
        {
            public const int DESCRIPTION = 50,
                             MAINT_ACT_TYPE = 3;
        }

        public enum Indices
        {
            WATER_MAIN_BLEEDERS = 2,
            CHANGE_BURST_METER = 3,
            CHECK_NO_WATER = 4,
            CURB_BOX_REPAIR = 5,
            BALL_CURB_STOP_REPAIR = 9,
            EXCAVATE_METER_BOX_SETTER = 14,
            SERVICE_LINE_FLOW_TEST = 18,
            HYDRANT_FROZEN = 19,
            FROZEN_METER_SET = 20,
            FROZEN_SERVICE_LINE_COMPANY_SIDE = 21,
            FROZEN_SERVICE_LINE_CUST_SIDE = 22,
            GROUND_WATER_SERVICE = 23,
            HYDRANT_FLUSHING = 24,
            HYDRANT_INVESTIGATION = 25,
            HYDRANT_INSTALLATION = 26,
            HYDRANT_LEAKING = 27,
            HYDRANT_NO_DRIP = 28,
            HYDRANT_REPAIR = 29,
            HYDRANT_REPLACEMENT = 30,
            HYDRANT_RETIREMENT = 31,
            INACTIVE_ACCOUNT = 32,
            VALVE_BLOW_OFF_INSTALLATION = 34,
            FIRE_SERVICE_INSTALLATION = 35,
            INSTALL_LINE_STOPPER = 36,
            INSTALL_METER = 37,
            INTERIOR_SETTING_REPAIR = 38,
            SERVICE_INVESTIGATION = 40,
            MAIN_INVESTIGATION = 41,
            LEAK_IN_METER_BOX_INLET = 42,
            LEAK_IN_METER_BOX_OUTLET = 43,
            LEAK_SURVEY = 44,
            METER_BOX_SETTER_INSTALLATION = 47,
            METER_CHANGE = 49,
            METER_BOX_ADJUSTMENT_RESETTER = 50,
            NEW_MAIN_FLUSHING = 54,
            SERVICE_LINE_INSTALLATION = 56,
            SERVICE_LINE_LEAK_CUST_SIDE = 58,
            SERVICE_LINE_RENEWAL = 59,
            SERVICE_LINE_RETIRE = 60,
            SUMP_PUMP = 61,
            TEST_SHUT_DOWN = 62,
            VALVE_BOX_REPAIR = 64,
            VALVE_BOX_BLOW_OFF_REPAIR = 65,
            SERVICE_LINE_VALVE_BOX_REPAIR = 66,
            VALVE_INVESTIGATION = 67,
            VALVE_LEAKING = 68,
            VALVE_REPAIR = 69,
            VALVE_BLOW_OFF_REPAIR = 70,
            VALVE_REPLACEMENT = 71,
            VALVE_RETIREMENT = 72,
            WATER_BAN_RESTRICTION_VIOLATOR = 73,
            WATER_MAIN_BREAK_REPAIR = 74,
            WATER_MAIN_INSTALLATION = 75,
            WATER_MAIN_RETIREMENT = 76,
            WATER_MAIN_REPLACEMENT = 274,
            FLUSHING_SERVICE = 78,
            WATER_MAIN_BREAK_REPLACE = 80,
            METER_BOX_SETTER_REPLACE = 81,
            SEWER_MAIN_BREAK_REPAIR = 82,
            SEWER_MAIN_BREAK_REPLACE = 83,
            SEWER_MAIN_RETIREMENT = 84,
            SEWER_MAIN_INSTALLATION = 85,
            SEWER_MAIN_CLEANING = 86,
            SEWER_LATERAL_INSTALLATION = 87,
            SEWER_LATERAL_REPAIR = 88,
            SEWER_LATERAL_REPLACE = 89,
            SEWER_LATERAL_RETIRE = 90,
            SEWER_LATERAL_CUSTOMER_SIDE = 91,
            SEWER_OPENING_REPAIR = 92,
            SEWER_OPENING_REPLACE = 93,
            SEWER_OPENING_INSTALLATION = 94,
            SEWER_MAIN_OVERFLOW = 95,
            SEWER_BACKUP_COMPANY_SIDE = 96,
            HYDRAULIC_FLOW_TEST = 98,
            MARKOUT_CREW = 99,
            VALVE_BOX_REPLACEMENT = 100,
            SITE_INSPECTION_SURVEY_NEW_SERVICE = 101,
            SITE_INSPECTION_SURVEY_SERVICE_RENEWAL = 102,
            SERVICE_LINE_REPAIR = 103,
            SEWER_CLEAN_OUT_INSTALLATION = 104,
            SEWER_CLEAN_OUT_REPAIR = 105,
            SEWER_CAMERA_SERVICE = 106,
            SEWER_CAMERA_MAIN = 107,
            SEWER_DEMOLITION_INSPECTION = 108,
            SEWER_MAIN_TEST_HOLES = 109,
            WATER_MAIN_TEST_HOLES = 110,
            VALVE_BROKEN = 111,
            GROUND_WATER_MAIN = 112,
            SERVICE_TURN_ON = 113,
            SERVICE_TURN_OFF = 114,
            METER_OBTAIN_READ = 115,
            METER_FINAL_START_READ = 116,
            METER_REPAIR_TOUCH_PAD = 117,
            VALVE_INSTALLATION = 118,
            VALVE_BLOW_OFF_REPLACEMENT = 119,
            HYDRANT_PAINT = 120,
            BALL_CURB_STOP_REPLACE = 121,
            VALVE_BLOW_OFF_RETIREMENT = 122,
            VALVE_BLOW_OFF_BROKEN = 123,
            WATER_MAIN_RELOCATION = 124,
            HYDRANT_RELOCATION = 125,
            SERVICE_RELOCATION = 126,
            SEWER_INVESTIGATION_MAIN = 127,
            SEWER_SERVICE_OVERFLOW = 128,
            SEWER_INVESTIGATION_LATERAL = 129,
            SEWER_INVESTIGATION_OPENING = 130,
            SEWER_LIFT_STATION_REPAIR = 131,
            CURB_BOX_REPLACE = 132,
            SERVICE_LINE_VALVE_BOX_REPLACE = 133,
            STORM_CATCH_REPAIR = 134,
            STORM_CATCH_REPLACE = 135,
            STORM_CATCH_INSTALLATION = 136,
            STORM_CATCH_INVESTIGATION = 137,
            HYDRANT_LANDSCAPING = 138,
            HYDRANT_RESTORATION_INVESTIGATION = 139,
            HYDRANT_RESTORATION_REPAIR = 140,
            MAIN_LANDSCAPING = 141,
            MAIN_RESTORATION_INVESTIGATION = 142,
            MAIN_RESTORATION_REPAIR = 143,
            SERVICE_LANDSCAPING = 144,
            SERVICE_RESTORATION_INVESTIGATION = 145,
            SERVICE_RESTORATION_REPAIR = 146,
            SEWER_LATERAL_LANDSCAPING = 147,
            SEWER_LATERAL_RESTORATION_INVESTIGATION = 148,
            SEWER_LATERAL_RESTORATION_REPAIR = 149,
            SEWER_MAIN_LANDSCAPING = 150,
            SEWER_MAIN_RESTORATION_INVESTIGATION = 151,
            SEWER_MAIN_RESTORATION_REPAIR = 152,
            SEWER_OPENING_LANDSCAPING = 153,
            SEWER_OPENING_RESTORATION_INVESTIGATION = 154,
            SEWER_OPENING_RESTORATION_REPAIR = 155,
            VALVE_LANDSCAPING = 159,
            VALVE_RESTORATION_INVESTIGATION = 160,
            VALVE_RESTORATION_REPAIR = 161,
            STORM_CATCH_LANDSCAPING = 162,
            STORM_CATCH_RESTORATION_INVESTIGATION = 163,
            STORM_CATCH_RESTORATION_REPAIR = 164,
            RSTRN_RESTORATION_INQUIRY = 165,
            SERVICE_OFF_AT_MAIN_STORM_RESTORATION = 169,
            SERVICE_OFF_AT_CURB_STOP_STORM_RESTORATION = 170,
            SERVICE_OFF_AT_METER_PIT_STORM_RESTORATION = 171,
            VALVE_TURNED_OFF_STORM_RESTORATION = 172,
            MAIN_REPAIR_STORM_RESTORATION = 173,
            MAIN_REPLACE_STORM_RESTORATION = 174,
            HYDRANT_TURNED_OFF_STORM_RESTORATION = 175,
            HYDRANT_REPLACE_STORM_RESTORATION = 176,
            VALVE_INSTALLATION_STORM_RESTORATION = 177,
            VALVE_REPLACEMENT_STORM_RESTORATION = 178,
            CURB_BOX_LOCATE_STORM_RESTORATION = 179,
            METER_PIT_LOCATE_STORM_RESTORATION = 190,
            VALVE_RETIREMENT_STORM_RESTORATION = 191,
            EXCAVATE_METER_PIT__STORM_RESTORATION = 192,
            SERVICE_LINE_RENEWAL_STORM_RESTORATION = 193,
            CURB_BOX_REPLACEMENT_STORM_RESTORATION = 194,
            WATER_MAIN_RETIREMENT_STORM_RESTORATION = 195,
            SERVICE_LINE_RETIREMENT_STORM_RESTORATION = 196,
            FRAME_AND_COVER_REPLACE_STORM_RESTORATION = 197,
            PUMP_REPAIR = 203,
            LINE_STOP_REPAIR = 204,
            SAW_REPAIR = 205,
            VEHICLE_REPAIR = 206,
            MISC_REPAIR = 207,
            Z_LWC_EW4_3_CONSECUTIVE_MTHS_OF_0_USAGE_ZERO = 208,
            Z_LWC_EW4_CHECK_METER_NON_EMERGENCY_CKMTR = 209,
            Z_LWC_EW4_DEMOLITION_CLOSED_ACCOUNT_DEMOC = 210,
            Z_LWC_EW4_METER_CHANGE_OUT_MTRCH = 211,
            Z_LWC_EW4_READ_MR_EDIT_LOCAL_OPS_ONLY_MREDT = 212,
            Z_LWC_EW4_READ_TO_STOP_ESTIMATE_EST = 213,
            Z_LWC_EW4_REPAIR_INSTALL_READING_DEVICE_REM = 214,
            Z_LWC_EW4_REREAD_AND_OR_INSPECT_FOR_LEAK_HILOW = 215,
            Z_LWC_EW4_SET_METER_TURN_ON_AND_READ_ONSET = 216,
            Z_LWC_EW4_TURN_ON_WATER_ON = 217,
            HYDRANT_NOZZLE_REPLACEMENT = 220,
            HYDRANT_NOZZLE_INVESTIGATION = 221,
            WATER_SERVICE_RENEWAL_CUST_SIDE = 222,
            CROSSING_INVESTIGATION = 223,
            CROSSING_RETIREMENT = 225,
            CROSSING_INSTALLATION = 226,
            IRRIGATION_RENEWAL = 259,
            FIRE_SERVICE_LINE_RENEWAL = 285,
            SERVICE_LINE_RENEWAL_LEAD = 295,
            SERVICE_LINE_RETIRE_LEAD = 298,
            SERVICE_LINE_RENEWAL_CUST_LEAD = 307,
            IRRIGATION_INSTALLATION = 314,
            SERVICE_LINE_RETIRE_NO_PREMISE = 315,
            SERVICE_LINE_RETIRE_LEAD_NO_PREMISE = 316,
            SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL = 327,
            SERVICE_LINE_INSTALLATION_PARTIAL = 328,
            SERVICE_LINE_UPSIZE_DOWNSIZE = 329
        }

        public static readonly int[] BRB_PMAT_DESCRIPTIONS = {
            (int)Indices.NEW_MAIN_FLUSHING,
            (int)Indices.TEST_SHUT_DOWN,
            (int)Indices.WATER_MAIN_INSTALLATION,
            (int)Indices.WATER_MAIN_RETIREMENT,
            (int)Indices.SEWER_MAIN_INSTALLATION,
            (int)Indices.WATER_MAIN_RELOCATION,
            (int)Indices.CROSSING_INSTALLATION,
            (int)Indices.CROSSING_RETIREMENT,
            (int)Indices.WATER_MAIN_REPLACEMENT,
            (int)Indices.SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL,
            (int)Indices.SERVICE_LINE_INSTALLATION_PARTIAL
        };

        public static readonly int[] NEW_SERVICE_INSTALLATION = new[] {
            (int)Indices.FIRE_SERVICE_INSTALLATION,
            (int)Indices.INSTALL_METER,
            (int)Indices.SERVICE_LINE_INSTALLATION,
            (int)Indices.SEWER_LATERAL_INSTALLATION,
            (int)Indices.IRRIGATION_INSTALLATION,
            (int)Indices.SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL
        };

        public static readonly int[] SERVICE_LINE_RENEWALS = {
            (int)Indices.SERVICE_LINE_RENEWAL,
            (int)Indices.SERVICE_LINE_RENEWAL_STORM_RESTORATION,
            (int)Indices.SERVICE_LINE_RENEWAL_LEAD
        };

        public static readonly int[] SERVICE_LINE_RETIRE = {
            (int)Indices.SERVICE_LINE_RETIRE,
            (int)Indices.SERVICE_LINE_RETIRE_LEAD,
            (int)Indices.SERVICE_LINE_RETIRE_NO_PREMISE,
            (int)Indices.SERVICE_LINE_RETIRE_LEAD_NO_PREMISE
        };

        public static readonly int[] INVESTIGATIVE = {
            (int)Indices.HYDRANT_INVESTIGATION,
            (int)Indices.SERVICE_INVESTIGATION,
            (int)Indices.MAIN_INVESTIGATION,
            (int)Indices.VALVE_INVESTIGATION,
            (int)Indices.SEWER_INVESTIGATION_MAIN,
            (int)Indices.SEWER_INVESTIGATION_LATERAL,
            (int)Indices.SEWER_INVESTIGATION_OPENING,
            (int)Indices.STORM_CATCH_INVESTIGATION,
            (int)Indices.CROSSING_INVESTIGATION
        };

        public static readonly int[] PITCHER_FILTER_REQUIREMENT = {
            (int)Indices.SERVICE_LINE_RENEWAL_CUST_LEAD,
            (int)Indices.SERVICE_LINE_RENEWAL,
            (int)Indices.SERVICE_LINE_RENEWAL_LEAD,
            (int)Indices.WATER_SERVICE_RENEWAL_CUST_SIDE
        };

        public static readonly int[] SEWER_OVERFLOW = new[] { 95, 128 };

        public static readonly int[] PITCHER_FILTER_NOT_DELIVERED_WORK_DESCRIPTIONS = {
            (int)Indices.SERVICE_LINE_RENEWAL,
            (int)Indices.WATER_SERVICE_RENEWAL_CUST_SIDE
        };

        public static readonly int[] SERVICE_LINE_INSTALLATIONS = {
            (int)Indices.SERVICE_LINE_INSTALLATION,
            (int)Indices.SERVICE_LINE_INSTALLATION_COMPLETE_PARTIAL,
            (int)Indices.SERVICE_LINE_INSTALLATION_PARTIAL,
            (int)Indices.SERVICE_LINE_UPSIZE_DOWNSIZE
        };

        public static readonly int[] AUTO_CREATE_SERVICE_WORK_DESCRIPTIONS = {
            (int)Indices.SERVICE_LINE_INSTALLATION,
            (int)Indices.SERVICE_LINE_INSTALLATION_PARTIAL,
            (int)Indices.FIRE_SERVICE_INSTALLATION,
            (int)Indices.IRRIGATION_INSTALLATION,
            (int)Indices.SEWER_LATERAL_INSTALLATION
        };

        public static readonly int[] WORK_DESCRIPTIONS_FOR_INCOMPLETE_LEAKS = {
            (int)Indices.HYDRANT_LEAKING, 
            (int)Indices.SERVICE_INVESTIGATION,
            (int)Indices.MAIN_INVESTIGATION,
            (int)Indices.LEAK_IN_METER_BOX_INLET,
            (int)Indices.LEAK_IN_METER_BOX_OUTLET,  
            (int)Indices.SERVICE_LINE_INSTALLATION,
            (int)Indices.SERVICE_LINE_LEAK_CUST_SIDE,
            (int)Indices.VALVE_LEAKING,
            (int)Indices.WATER_MAIN_BREAK_REPAIR,
            (int)Indices.WATER_MAIN_BREAK_REPLACE
        };

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [StringLength(StringLengths.DESCRIPTION)]
        [Required]
        public virtual string Description { get; set; }

        public virtual AssetType AssetType { get; set; }
        public virtual WorkCategory WorkCategory { get; set; }
        public virtual RestorationAccountingCode FirstRestorationAccountingCode { get; set; }
        public virtual RestorationProductCode FirstRestorationProductCode { get; set; }
        public virtual RestorationAccountingCode SecondRestorationAccountingCode { get; set; }
        public virtual RestorationProductCode SecondRestorationProductCode { get; set; }

        [Required]
        public virtual decimal TimeToComplete { get; set; }

        public virtual AccountingType AccountingType { get; set; }

        [Required]
        public virtual int FirstRestorationCostBreakdown { get; set; }

        public virtual int? SecondRestorationCostBreakdown { get; set; }

        [Required]
        public virtual bool ShowBusinessUnit { get; set; }

        [Required]
        public virtual bool ShowApprovalAccounting { get; set; }

        [Required]
        public virtual bool EditOnly { get; set; }

        [Required]
        public virtual bool Revisit { get; set; }

        [Required]
        public virtual bool IsActive { get; set; }

        [StringLength(StringLengths.MAINT_ACT_TYPE)]
        public virtual string MaintenanceActivityType { get; set; }

        public virtual PlantMaintenanceActivityType PlantMaintenanceActivityType { get; set; }

        public virtual bool? MarkoutRequired { get; set; }
        public virtual bool? MaterialsRequired { get; set; }
        public virtual bool? JobSiteCheckListRequired { get; set; }
        
        [View("Digital As-Built Required")]
        public virtual bool DigitalAsBuiltRequired { get; set; }

        #endregion

        public virtual bool IsMainReplaceOrRepair => Id == (int)Indices.WATER_MAIN_BREAK_REPAIR ||
                                                     Id == (int)Indices.WATER_MAIN_BREAK_REPLACE;

        public virtual bool HasPitcherFilterRequirement => PITCHER_FILTER_REQUIREMENT.Contains(Id);

        #endregion

        #region Constructors

        //public WorkDescription()
        //{
        //    WorkDescriptions = new List<WorkDescription>();
        //    WorkOrderDescriptionChanges = new List<WorkOrderDescriptionChange>();
        //    WorkOrderDescriptionChanges = new List<WorkOrderDescriptionChange>();
        //    WorkOrders = new List<WorkOrder>();
        //}

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return Description;
        }

        public static object WorkDescriptionToJson(WorkDescription workDescription)
        {
            return new {
                workDescription.Id,
                workDescription.Description,
                workDescription.DigitalAsBuiltRequired
            };
        }

        public static int[] GetMainBreakWorkDescriptions()
        {
            return new[] {
                (int)Indices.WATER_MAIN_BREAK_REPAIR,
                (int)Indices.WATER_MAIN_BREAK_REPLACE
            };
        }

        #endregion
    }

    [Serializable]
    public class WorkCategory : EntityLookup { }

    [Serializable]
    public class RestorationAccountingCode : IEntity
    {
        #region Consts

        public struct StringLengths
        {
            public const int CODE = 8,
                             SUBCODE = 2;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string SubCode { get; set; }

        #endregion

        #region Exposed Methods
        
        public override string ToString()
        {
            return Code;
        }
        
        #endregion
    }

    [Serializable]
    public class RestorationProductCode : EntityLookup
    {
        #region Consts

        public const int CODE_DESCRIPTION_LENGTH = 4;

        #endregion
    }
}
