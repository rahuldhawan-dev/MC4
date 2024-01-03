using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Json;
using Newtonsoft.Json;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ShortCycleWorkOrderSafetyBrief : IEntity
    {
        #region Constants

        public const string LOCATION_WHERE_WORKING = "Where will you be working today?",
                            PPE = "Is your PPE in good condition?",
                            DAILY_STRETCHING_ROUTINE = "Have you completed your daily stretching routine?",
                            HAZARDS = "What hazards will likely be present while working today?",
                            INSPECTION = "Have you performed inspection on your vehicle and is it safe to operate?",
                            NEEDED_PPE = "What PPE do you need today based on the hazards you will face?",
                            TOOL_NEEDED = "What tools do you need today?";

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [JsonConverter(typeof(ToStringJsonConverter))]
        public virtual Employee FSR { get; set; }

        public virtual DateTime DateCompleted { get; set; }

        [View(PPE)]
        public virtual bool IsPPEInGoodCondition { get; set; }

        [View(DAILY_STRETCHING_ROUTINE)]
        public virtual bool HasCompletedDailyStretchingRoutine { get; set; }

        [View(INSPECTION)]
        public virtual bool HasPerformedInspectionOnVehicle { get; set; }

        [View(LOCATION_WHERE_WORKING)]
        public virtual IList<ShortCycleWorkOrderSafetyBriefLocationType> LocationTypes { get; set; }

        [View(HAZARDS)]
        public virtual IList<ShortCycleWorkOrderSafetyBriefHazardType> HazardTypes { get; set; }

        [View(NEEDED_PPE)]
        public virtual IList<ShortCycleWorkOrderSafetyBriefPPEType> PPETypes { get; set; }

        [View(TOOL_NEEDED)]
        public virtual IList<ShortCycleWorkOrderSafetyBriefToolType> ToolTypes { get; set; }

        #endregion

        #region Constructor

        public ShortCycleWorkOrderSafetyBrief()
        {
            LocationTypes = new List<ShortCycleWorkOrderSafetyBriefLocationType>();
            HazardTypes = new List<ShortCycleWorkOrderSafetyBriefHazardType>();
            PPETypes = new List<ShortCycleWorkOrderSafetyBriefPPEType>();
            ToolTypes = new List<ShortCycleWorkOrderSafetyBriefToolType>();
        }

        #endregion
    }
}
