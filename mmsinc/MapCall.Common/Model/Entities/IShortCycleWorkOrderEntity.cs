using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    public interface IShortCycleWorkOrderEntity : IEntity, ISAPEntity
    {
        int Id { get; }
        SapCommunicationStatus SapCommunicationStatus { get; set; }
        bool HasSAPError { get; set; }
    }
}
