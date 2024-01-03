using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetRoles
    {
        public AspnetRoles() { }
        public virtual System.Guid RoleId { get; set; }
        public virtual AspnetApplications AspnetApplications { get; set; }
        public virtual string RoleName { get; set; }
        public virtual string LoweredRoleName { get; set; }
        public virtual string Description { get; set; }
    }
}
