using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Don't confuse these with ServiceInstallationMaterials. 
    /// Those are a list of materials used when installing a service.
    /// This is the type of pipe used the service.
    /// </summary>
    [Serializable]
    public class ServiceMaterial : EntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            public const int CODE = 2;
        }

        /// <summary>
        /// ServiceLineProtectionInvestigationMap - contains static references to GALVANIZED,LEAD,TUBELOY
        /// </summary>
        public struct Indices
        {
            public const int AC = 1,
                             CARLON = 2,
                             CAST_IRON = 3,
                             COPPER = 4,
                             DUCTILE = 5,
                             GALVANIZED = 6,
                             LEAD = 7,
                             PLASTIC = 8,
                             TRANSITE = 9,
                             TUBELOY = 10,
                             UNKNOWN = 11,
                             VITRIFIED_CLAY = 12,
                             WICL = 13,
                             NOT_PRESENT = 14,
                             GALVANIZED_WITH_LEAD_GOOSENECK = 15,
                             OTHER_WITH_LEAD_GOOSENECK = 16,
                             NOT_LEAD_MATERIAL_UNDETERMINED = 17,
                             UNCLASSIFIED_NO_TAP_INFO = 18,
                             UNCLASSIFIED_POTENTIAL_LEAD = 19,
                             UNCLASSIFIED_UNLIKELY_LEAD = 20,
                             UNCLASSIFIED_RESEARCH_NEEDED = 21,
                             UNKNOWN_NOT_VISIBLE = 22,
                             UNKNOWN_PIPE_HEAVILY_PAINTED = 23,
                             UNKNOWN_VIA_TEST = 24,
                             BRASS = 25,
                             ORANGEBURG = 26,
                             GALVANIZED_REQUIRING_REPLACEMENT = 27;
        }

        public struct Descriptions
        {
            public const string UNKNOWN = "UNKNOWN",
                                GALVANIZED = "GALVANIZED",
                                LEAD = "LEAD";
        }

        #endregion

        #region Properties

        /// <summary>
        /// This is used by the sample site lead and copper certification pdf.
        /// If that pdf ever goes away, this field can also go away.
        /// </summary>
        public virtual string Code { get; set; }

        public virtual bool IsEditEnabled { get; set; }

        public virtual EPACode CustomerEPACode { get; set; }

        public virtual EPACode CompanyEPACode { get; set; }

        public virtual IList<OperatingCenter> OperatingCenters
        {
            get { return OperatingCentersServiceMaterials.Select(x => x.OperatingCenter).ToList(); }
        }

        public virtual IList<OperatingCenterServiceMaterial> OperatingCentersServiceMaterials { get; set; }
        public virtual IList<ServiceMaterialEPACodeOverride> ServiceMaterialEPACodeOverrides { get; set; }

        #endregion

        #region Constructors

        public ServiceMaterial()
        {
            OperatingCentersServiceMaterials = new List<OperatingCenterServiceMaterial>();
            ServiceMaterialEPACodeOverrides = new List<ServiceMaterialEPACodeOverride>();
        }

        #endregion

        #region Exposed Methods
        
        /// <summary>
        /// This returns EPACode overriden by state, and if not default Customer EPA code
        /// </summary>
        public virtual EPACode CustomerEPACodeOverridenOrDefault(State state)
        {
            return ServiceMaterialEPACodeOverrides?
                      .FirstOrDefault(s => s.State.Id == state?.Id)?.CustomerEPACode
                   ?? CustomerEPACode;
        }
        
        /// <summary>
        /// This returns EPACode overriden by state, and if not default Company EPA code
        /// </summary>
        public virtual EPACode CompanyEPACodeOverridenOrDefault(State state)
        {
            return ServiceMaterialEPACodeOverrides?
                      .FirstOrDefault(s => s.State.Id == state?.Id)?.CompanyEPACode
                   ?? CompanyEPACode;
        }
        
        #endregion
    }

    [Serializable]
    public class OperatingCenterServiceMaterial : IEntity
    {
        public virtual int Id { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual ServiceMaterial ServiceMaterial { get; set; }
        public virtual bool NewServiceRecord { get; set; }
    }
}
