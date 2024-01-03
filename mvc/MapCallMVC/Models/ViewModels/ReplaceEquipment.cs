using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Models.ViewModels {
    public class ReplaceEquipment : CreateEquipment
    {
        #region Constructors

        public ReplaceEquipment(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private int? IdOrNull(dynamic thing)
        {
            return thing != null ? thing.Id : null;
        }

        private bool BoolOrFalse(bool? b)
        {
            return b.HasValue && b.Value;
        }

        #endregion

        #region Exposed Methods

        public override void Map(Equipment entity)
        {
            Description = entity.Description;
            ArcFlashHierarchy = entity.ArcFlashHierarchy;
            ArcFlashRating = entity.ArcFlashRating;

            Prerequisites = entity.ProductionPrerequisites.Select(x => x.Id).ToArray();
            Facility = entity.Facility.Id;
            OperatingCenter = entity.Facility.OperatingCenter.Id;

            FunctionalLocation = entity.FunctionalLocation;
            EquipmentType = IdOrNull(entity.EquipmentType);
            EquipmentPurpose = IdOrNull(entity.EquipmentPurpose);
            ABCIndicator = IdOrNull(entity.ABCIndicator);

            DateInstalled = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            SAPEquipmentIdBeingReplaced = entity.SAPEquipmentId;
            ReplacedEquipment = entity.Id;
            IsReplacement = true;
            EquipmentStatus = MapCall.Common.Model.Entities.EquipmentStatus.Indices.PENDING;

            CriticalNotes = entity.CriticalNotes;
            Legacy = entity.Legacy;
        }

        #endregion
    }
}