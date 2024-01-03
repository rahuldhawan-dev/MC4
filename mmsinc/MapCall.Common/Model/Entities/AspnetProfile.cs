using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetProfile
    {
        public virtual System.Guid UserId { get; set; }
        public virtual AspnetUsers AspnetUsers { get; set; }
        public virtual string PropertyNames { get; set; }
        public virtual string PropertyValuesString { get; set; }
        public virtual byte[] PropertyValuesBinary { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }
    }
}
