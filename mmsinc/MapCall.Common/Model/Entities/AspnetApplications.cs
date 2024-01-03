using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetApplications
    {
        public AspnetApplications() { }
        public virtual System.Guid ApplicationId { get; set; }
        public virtual string ApplicationName { get; set; }
        public virtual string LoweredApplicationName { get; set; }
        public virtual string Description { get; set; }
    }
}
