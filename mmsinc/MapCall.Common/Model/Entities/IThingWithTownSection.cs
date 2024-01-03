using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public interface IThingWithTownSection : IEntity
    {
        TownSection TownSection { get; }
    }
}
