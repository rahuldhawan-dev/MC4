using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetPaths
    {
        public AspnetPaths() { }
        public virtual System.Guid PathId { get; set; }
        public virtual AspnetApplications AspnetApplications { get; set; }
        public virtual string Path { get; set; }
        public virtual string LoweredPath { get; set; }
    }
}
