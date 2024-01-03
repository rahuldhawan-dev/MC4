using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class CopySewerOpening : CreateSewerOpening
    {
        #region Properties

        [AutoMap]
        public override int? InspectionFrequency { get => base.InspectionFrequency; set => base.InspectionFrequency = value; }
        [EntityMap]
        public override int? InspectionFrequencyUnit { get => base.InspectionFrequencyUnit; set => base.InspectionFrequencyUnit = value; }

        #endregion

        #region Constructors

        public CopySewerOpening(IContainer container, AssetStatusNumberDuplicationValidator numberValidator) : base(container, numberValidator) { }

        #endregion

        #region Exposed Methods

        public override SewerOpening MapToEntity(SewerOpening entity)
        {
            entity = base.MapToEntity(entity);

            if (Status == AssetStatus.Indices.ACTIVE)
            {
                SendNotificationOnSave = true;
            }

            entity.Status = new AssetStatus {Id = MapCall.Common.Model.Entities.AssetStatus.Indices.PENDING};

            return entity;
        }

        public override void Map(SewerOpening entity)
        {
            base.Map(entity);
            Coordinate = null;
        }

        protected override void SetInspectionFrequencyFromOperatingCenter(SewerOpening entity)
        {
            // This is to prevent setting the default values while copying -- MC-1616
        }

        #endregion
    }
}
