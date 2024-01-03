namespace MMSINC.Utilities.Auditing
{
    public interface IAuditor
    {
        string SqlConnectionString { get; set; }
        void Insert(AuditCategory category, string createdBy, string details);
    }
}
