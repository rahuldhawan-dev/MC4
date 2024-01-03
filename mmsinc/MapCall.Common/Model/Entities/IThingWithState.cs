namespace MapCall.Common.Model.Entities
{
    public interface IThingWithState
    {
        int Id { get; }
        State State { get; }
    }
}
