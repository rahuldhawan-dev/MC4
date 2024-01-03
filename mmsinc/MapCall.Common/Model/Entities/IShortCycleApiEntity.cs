namespace MapCall.Common.Model.Entities
{
    public interface IShortCycleApiEntity : IShortCycleWorkOrderEntity
    {
        string ToJsonForApi();
    }
}
