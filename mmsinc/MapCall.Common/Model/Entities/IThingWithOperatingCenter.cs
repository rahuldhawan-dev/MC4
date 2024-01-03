using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public interface IThingWithOperatingCenter : IEntity
    {
        OperatingCenter OperatingCenter { get; }
    }
}
