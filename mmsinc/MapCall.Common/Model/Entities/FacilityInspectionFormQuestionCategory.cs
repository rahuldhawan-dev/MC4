using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class FacilityInspectionFormQuestionCategory : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int GENERAL_WORK_AREA_CONDITIONS = 1, 
                             EMERGENCY_RESPONSE_FIRST_AID = 2, 
                             SECURITY = 3, 
                             FIRE_SAFETY = 4, 
                             PERSONAL_PROTECTIVE_EQUIPMENT = 5, 
                             CHEMICAL_STORAGE_HAZ_COM = 6, 
                             EQUIPMENT_TOOLS = 7, 
                             CONFINED_SPACE = 8, 
                             VEHICLE_MOTORIZED_EQUIPMENT = 9, 
                             OSHA_TRAINING = 10;
        }

        #endregion
    }
}