using System;
using System.ComponentModel;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OneCallMarkoutResponse : IEntity
    {
        public virtual int Id { get; set; }
        public virtual OneCallMarkoutTicket OneCallMarkoutTicket { get; set; }
        public virtual User CompletedBy { get; set; }
        public virtual OneCallMarkoutResponseStatus OneCallMarkoutResponseStatus { get; set; }
        public virtual OneCallMarkoutResponseTechnique OneCallMarkoutResponseTechnique { get; set; }
        public virtual DateTime CompletedAt { get; set; }
        public virtual string Comments { get; set; }
        public virtual bool? ReqNotified { get; set; }
        public virtual bool? Paint { get; set; }
        public virtual bool? Flag { get; set; }
        public virtual bool? Stake { get; set; }
        public virtual bool? Over500Feet { get; set; }
        public virtual bool? CrewMarkoutIsNeeded { get; set; }
        public virtual int? NumberOfCsmo { get; set; }
        public virtual int? NumberOfCsmoUnableToLocate { get; set; }

        [Description("(minutes)")]
        public virtual int? TotalTimeSpentForCsmo { get; set; }
    }

    [Serializable]
    public class OneCallMarkoutResponseStatus : EntityLookup { }

    [Serializable]
    public class OneCallMarkoutResponseTechnique : EntityLookup { }
}
