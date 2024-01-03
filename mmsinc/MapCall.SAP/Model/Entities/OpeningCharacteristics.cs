using System;
using MapCall.Common.Model.Entities;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class OpeningCharacteristics
    {
        #region properties

        public virtual string OWNED_BY { get; set; }
        public virtual string SPECIAL_MAINT_NOTES_MH { get; set; }
        public virtual string SPECIAL_MAINT_NOTES_DETAILS { get; set; }
        public virtual string DEPENDENCY_DRIVER_1 { get; set; }
        public virtual string DEPENDENCY_DRIVER_2 { get; set; }
        public virtual string MH_TYP { get; set; }
        public virtual string WASTE_WATER_TP { get; set; }
        public virtual string BASIN { get; set; }
        public virtual string SUB_BASIN { get; set; }
        public virtual string MH_COVER_SIZE { get; set; }
        public virtual string MH_COVER_MATERIAL { get; set; }
        public virtual string MH_COVER_LABEL { get; set; }
        public virtual string MH_LID_TP { get; set; }
        public virtual string MH_SIZE { get; set; }
        public virtual string MATERIAL_OF_CONSTRUCTION_MH { get; set; }
        public virtual string MAP_PAGE { get; set; }
        public virtual string SURFACE_COVER { get; set; }
        public virtual string SURFACE_COVER_LOC_TP { get; set; }
        public virtual string ASSET_LOCATION { get; set; }
        public virtual string INSTALLATION_WO { get; set; }
        public virtual string MH_CONE { get; set; }
        public virtual string MH_CONE_MATERIAL { get; set; }
        public virtual string MH_CONE_INSERT { get; set; }
        public virtual string LINED { get; set; }
        public virtual DateTime LINED_DATE { get; set; }
        public virtual string MH_HAS_STEPS { get; set; }
        public virtual string MH_STEP_MATERIAL { get; set; }
        public virtual double MH_STEPS { get; set; }
        public virtual decimal? MH_DEPTH { get; set; }
        public virtual string MH_TROUGH { get; set; }
        public virtual string MH_PONDING { get; set; }
        public virtual string MH_PIPE_SEAL_TP { get; set; }
        public virtual string MH_ADJUSTING_RING_MATL { get; set; }
        public virtual string MH_CASTING_MATERIAL { get; set; }
        public virtual string MH_DROP_TP { get; set; }
        public virtual string HISTORICAL_ID { get; set; }
        public virtual string LAM_GEOACCURACY { get; set; }

        #endregion

        #region Constructors

        public OpeningCharacteristics() { }

        public OpeningCharacteristics(SewerOpening sewerOpening) { }

        #endregion
    }
}
