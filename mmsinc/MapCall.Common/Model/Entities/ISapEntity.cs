namespace MapCall.Common.Model.Entities
{
    public interface ISAPEntity
    {
        string SAPErrorCode { get; set; }
    }

    public interface ISAPLookup
    {
        string SAPCode { get; set; }
    }
}
