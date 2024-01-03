namespace MapCall.Common.Model.Entities
{
    /// <summary>
    /// Primarily for use with a TEntity that has an Employee property
    /// </summary>
    public interface IThingWithEmployee
    {
        Employee Employee { get; set; }
    }
}
