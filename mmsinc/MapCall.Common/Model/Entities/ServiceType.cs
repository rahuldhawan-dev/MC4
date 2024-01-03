using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// This is a special class. It is has composite id made up of the operating 
    /// center and service category. It does NOT have an Id. It's not a property 
    /// on any classes. If it needs to be it will need a proper id column and a 
    /// unique constraint
    /// </summary>
    [Serializable]
    public class ServiceType
    {
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual ServiceCategory ServiceCategory { get; set; }
        public virtual string Description { get; set; }
        public virtual CategoryOfServiceGroup CategoryOfServiceGroup { get; set; }

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;
            if (obj.GetType() != typeof(ServiceType))
                return false;
            var st = (ServiceType)obj;
            return (st.OperatingCenter == OperatingCenter && st.ServiceCategory == ServiceCategory);
        }

        public override int GetHashCode()
        {
            return (OperatingCenter.ToString() + ServiceCategory.ToString() + Description).GetHashCode();
        }

        public override string ToString()
        {
            return Description;
        }
    }

    [Serializable]
    public class CategoryOfServiceGroup : ReadOnlyEntityLookup
    {
        public struct Indices
        {
            public const int NEW = 1, RENEWAL = 2, RETIRE = 3;
        }
    }
}
