using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetPersonalizationAllUsers
    {
        public virtual System.Guid PathId { get; set; }
        public virtual AspnetPaths AspnetPaths { get; set; }
        public virtual byte[] PageSettings { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }
    }
}
