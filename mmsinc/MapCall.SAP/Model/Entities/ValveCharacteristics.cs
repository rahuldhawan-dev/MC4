using System;
using MapCall.Common.Model.Entities;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class ValveCharacteristics
    {
        public virtual string OWNED_BY { get; set; }
        public virtual string SPECIAL_MAINT_NOTES_DIST { get; set; }
        public virtual string SPECIAL_MAINT_NOTES_DETAILS { get; set; }
        public virtual string DEPENDENCY_DRIVER_1 { get; set; }
        public virtual string DEPENDENCY_DRIVER_2 { get; set; }
        public virtual string SVLV_TYP { get; set; }
        public virtual string APPLICATION_SVLV { get; set; }
        public virtual string SUB_DIVISION { get; set; }
        public virtual string PRESSURE_ZONE { get; set; }
        public virtual string PRESSURE_ZONE_HGL { get; set; }
        public virtual double NORMAL_SYS_PRESSURE { get; set; }
        public virtual string MAP_PAGE { get; set; }
        public virtual string BOOK_PAGE { get; set; }
        public virtual string OPEN_DIRECTION { get; set; }
        public virtual decimal? NUMBER_OF_TURNS { get; set; }
        public virtual string NORMAL_POSITION { get; set; }
        public virtual string VLV_VALVE_SIZE { get; set; }
        public virtual string BYPASS_VALVE { get; set; }
        public virtual string TORQUE_LIMIT { get; set; }
        public virtual string VLV_DEPTH_TO_TOP_OF_MAIN { get; set; }
        public virtual string VLV_TOP_NUT_DEPTH { get; set; }
        public virtual string OPERATING_NUT_TP { get; set; }
        public virtual string VLV_OPER_NUT_SIZE { get; set; }
        public virtual string ACTUATOR_TP { get; set; }
        public virtual string GEAR_TP { get; set; }
        public virtual string VLV_SEAT_TP { get; set; }
        public virtual string ACCESS_TP { get; set; }
        public virtual string VLV_VALVE_BOX_MARKING { get; set; }
        public virtual string VLV_SPECIAL_V_BOX_MARKING { get; set; }
        public virtual string SURFACE_COVER { get; set; }
        public virtual string SURFACE_COVER_LOC_TP { get; set; }
        public virtual string PRESSURE_CLASS { get; set; }
        public virtual string JOINT_TP { get; set; }
        public virtual string EAM_PIPE_SIZE { get; set; }
        public virtual string PIPE_MATERIAL { get; set; }
        public virtual string ON_SCADA { get; set; }
        public virtual string VLV_VALVE_TP { get; set; }
        public virtual string INSTALLATION_WO { get; set; }
        public virtual string SKETCH_NUM { get; set; }
        public virtual string HISTORICAL_ID { get; set; }
        public virtual string LAM_GEOACCURACY { get; set; }

        public ValveCharacteristics() { }

        public ValveCharacteristics(Valve valve)
        {
            //SPECIAL_MAINT_NOTES_DETAILS = "";   //valve.StringLengths.CRITICAL_NOTES
            //SVLV_TYP = "";  //valve.ValveControls
            //APPLICATION_SVLV = "";  //valve.ValveControls
            //MAP_PAGE = "";  //valve.MapPage
            //OPEN_DIRECTION = "";    //valve.Opens
            //NUMBER_OF_TURNS = 0;    //valve.Turns
            //NORMAL_POSITION = "";   //valve.InNormalPosition
            //VLV_VALVE_SIZE = "";    //valve.ValveSize.size
            //BYPASS_VALVE = "";  //valve.ValveControls
            //EAM_PIPE_SIZE = ""; //
            //PIPE_MATERIAL = ""; //valve.MainType
            //VLV_VALVE_TP = "";  //valve.ValveType
            //INSTALLATION_WO = "";   //valve.WorkOrderNumber
            //HISTORICAL_ID = "";	// valve.ValveNumber
        }
    }
}
