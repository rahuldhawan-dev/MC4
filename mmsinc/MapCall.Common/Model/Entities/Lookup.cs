using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // NOTE: DO NOT USE THIS IF YOU CAN AVOID IT
    // This is only here for certain tables like tblPosition_History
    // which cannot yet be cleaned up and normalized because of existing
    // pages.  If you've found yourself here, you should probably create
    // a migration to normalize yourself out a table instead.

    // NOTE 2: What is this Lookup class needed for? StringLength doesn't mean 
    // anything on an entity, only on a view model. -Ross 11/18/2015

    [Serializable]
    public class Lookup : ReadOnlyEntityLookup
    {
        // overridden to get rid of the StringLength attribute
        [Required]
        public override string Description { get; set; }
    }

    [Serializable]
    public class DepartmentName : Lookup { }

    [Serializable]
    public class EmergencyResponsePriority : Lookup { }

    [Serializable]
    public class LicenseType : Lookup { }

    [Serializable]
    public class SOPCategory : Lookup { }

    [Serializable]
    public class SOPSection : Lookup { }

    [Serializable]
    public class SOPStatus : Lookup { }

    [Serializable]
    public class SOPSubSection : Lookup { }

    [Serializable]
    public class SOPSystem : Lookup { }

    [Serializable]
    public class TailgateTopicCategory : Lookup { }

    [Serializable]
    public class TailgateTopicMonth : Lookup { }
}
