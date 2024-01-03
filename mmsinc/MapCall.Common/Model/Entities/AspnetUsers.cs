using System;
using System.Text;
using System.Collections.Generic;

namespace MapCall.Common.Model.Entities
{
    public class AspnetUsers
    {
        public AspnetUsers() { }
        public virtual System.Guid UserId { get; set; }
        public virtual AspnetApplications AspnetApplications { get; set; }
        public virtual string UserName { get; set; }
        public virtual string LoweredUserName { get; set; }
        public virtual string MobileAlias { get; set; }
        public virtual bool IsAnonymous { get; set; }
        public virtual DateTime LastActivityDate { get; set; }
    }
}
