using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public interface IThingWithTown : IEntity
    {
        Town Town { get; }
    }
}
