using System;
using MapCall.Common.Model.Entities;

namespace MapCall.Common.Model
{
    public class AspnetPersonalizationPerUser
    {
        public virtual System.Guid Id { get; set; }
        public virtual AspnetPaths AspnetPaths { get; set; }
        public virtual AspnetUsers AspnetUsers { get; set; }
        public virtual byte[] PageSettings { get; set; }
        public virtual DateTime LastUpdatedDate { get; set; }
    }
}
