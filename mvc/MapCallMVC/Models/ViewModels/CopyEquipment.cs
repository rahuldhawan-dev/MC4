using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using StructureMap;

namespace MapCallMVC.Models.ViewModels 
{
    public class CopyEquipment : CreateEquipment
    {
        #region Constructors

        public CopyEquipment(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(Equipment entity)
        {
            base.Map(entity);
            //null every property out that isn't one of the below properties
            //Operating Center, Planning Plant, Facility, Coordinates, Department, Critical Notes
            //Description, Functional Location, WBS Number, Parent Equipment, Equipment Purpose
            //Equipment Purpose, Equipment Manufacturer, Equipment Model, Serial Number
            //Potable, Equipment Criticality (ABCIndicator), Prerequisites, PSMTCPA, Safety Notes, Maintenance Notes,
            //Operation Notes

            SAPEquipmentId = null;
            ReplacedEquipment = null;
            RequestedBy = null;
            AssetControlSignOffBy = null;
            AssetControlSignOffDate = null;
            DateRetired = null;
            DateInstalled = null;
            SAPEquipmentIdBeingReplaced = null;
            ScadaTagName = null;
            ArcFlashHierarchy = null;
            ArcFlashRating = null;
            IsReplacement = null;
            OtherComplianceReason = null;

            //default every property out that is one of the below properties
            HasCompanyRequirement = false;
            HasProcessSafetyManagement = false;
            HasOshaRequirement = false;
            OtherCompliance = false;
            HasRegulatoryRequirement = false;
        }

        public override Equipment MapToEntity(Equipment entity)
        {
            base.MapToEntity(entity);
            entity.EquipmentStatus = _container.GetInstance<IRepository<EquipmentStatus>>().Find(MapCall.Common.Model.Entities.EquipmentStatus.Indices.PENDING);
            return entity;
        }

        #endregion
    }
}