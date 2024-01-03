namespace MapCallScheduler.JobHelpers.LeakAlert
{
    public class LeakAlertFileRecord
    {
        public virtual string PersistedCorrelatedNoiseId { get; set; } = "";
        public virtual string DatePCNCreated { get; set; } = "";
        public virtual string POIStatusId { get; set; } = "";
        public virtual string POIStatus { get; set; } = "";
        public virtual string Latitude { get; set; } = "";
        public virtual string Longitude { get; set; } = "";
        public virtual string SocketID1 { get; set; } = "";
        public virtual string SocketID2 { get; set; } = "";
        public virtual string DistanceFrom1 { get; set; } = "";
        public virtual string DistanceFrom2 { get; set; } = "";
        public virtual string Note { get; set; } = "";
        public virtual string SiteName { get; set; } = "";
        public virtual string FieldInvestigationRecommendedOn { get; set; } = "";
    }
}
