using System;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class IncidentDrugAndAlcoholTestingDecision : EntityLookup
    {
        public struct Indices
        {
            // NOTE: If you add or change a value to this then you MUST
            // add a cooresponding TestDataFactory.
            public const int
                REASONABLE_SUSPICION = 1,
                POST_INCIDENT_INJURY = 2,
                POST_INJURY_VEHICLE_ACCIDENT = 3,
                POST_INJURY_ENVIRONMENTAL_RELEASE = 4,
                POST_INJURY_OTHER = 5,
                IMMEDIATE_MEDICAL_TREATMENT_NOT_REQUIRED = 7,
                OTHER = 9;
        }

        #region Properties

        /// <summary>
        /// Returns true if this is one of those decisions that starts with "TEST" instead of "NO TEST"
        /// </summary>
        public virtual bool RequiresTesting =>
            Description.StartsWith("TEST", StringComparison.InvariantCultureIgnoreCase);

        #endregion
    }
}
