using System;
using MMSINC.Data;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MainBreak : IEntity
    {
        #region Constants

        public const string DEPTH = "Depth(in.)",
                            DISINFECTION_METHOD = "Disinfection Method",
                            FAILURE_TYPE = "Failure Type",
                            FLUSH_METHOD = "Flush Method",
                            FOOTAGE_REPLACED = "Footage Installed",
                            MAIN_CONDITION = "Main Condition",
                            MAIN_BREAK_MATERIAL = "Existing Material",
                            MATERIAL = "Material",
                            SIZE = "Size",
                            SHUT_DOWN_TIME = "Shut Down Time(Hrs)",
                            SOIL_CONDITION = "Soil Condition";

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }

        [View(FAILURE_TYPE)]
        public virtual MainFailureType MainFailureType { get; set; }

        [View(MAIN_BREAK_MATERIAL)]
        public virtual MainBreakMaterial MainBreakMaterial { get; set; }

        public virtual MainCondition MainCondition { get; set; }

        [View(SOIL_CONDITION)]
        public virtual MainBreakSoilCondition MainBreakSoilCondition { get; set; }

        [View(DISINFECTION_METHOD)]
        public virtual MainBreakDisinfectionMethod MainBreakDisinfectionMethod { get; set; }

        [View(FLUSH_METHOD)]
        public virtual MainBreakFlushMethod MainBreakFlushMethod { get; set; }

        [View(SIZE)]
        public virtual ServiceSize ServiceSize { get; set; }

        public virtual MainBreakMaterial ReplacedWith { get; set; }

        [View(DEPTH)]
        public virtual decimal Depth { get; set; }
        public virtual int CustomersAffected { get; set; }

        [View(SHUT_DOWN_TIME)]
        public virtual decimal ShutdownTime { get; set; }
        public virtual decimal? ChlorineResidual { get; set; }
        public virtual bool BoilAlertIssued { get; set; }

        [View(FOOTAGE_REPLACED)]
        public virtual int? FootageReplaced { get; set; }

        #endregion
    }

    [Serializable]
    public class MainFailureType : ReadOnlyEntityLookup { }

    [Serializable]
    public class MainBreakMaterial : ReadOnlyEntityLookup { }

    [Serializable]
    public class MainCondition : ReadOnlyEntityLookup { }

    [Serializable]
    public class MainBreakSoilCondition : ReadOnlyEntityLookup { }

    [Serializable]
    public class MainBreakDisinfectionMethod : ReadOnlyEntityLookup { }

    [Serializable]
    public class MainBreakFlushMethod : ReadOnlyEntityLookup { }
}
