using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class CreateEnvironmentalPermit : EnvironmentalPermitViewModel
    {
        #region Constructors

        public CreateEnvironmentalPermit(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required]
        public override bool? RequiresRequirements { get; set; }

        [RequiredWhen("RequiresRequirements", ComparisonType.EqualTo, true), DoesNotAutoMap]
        public CreateEnvironmentalPermitRequirement CreateEnvironmentalPermitRequirement { get; set; }

        [MultiSelect(
            DependsOn = nameof(OperatingCenters), 
            Area = "", 
            Controller = "Facility",
            Action = "ByOperatingCenterIds", 
            PromptText = "Please select an operating center.")]
        public List<int> Facilities { get; set; } = new List<int>();

        [MultiSelect(
            DependsOn = nameof(Facilities), 
            Area = "", 
            Controller = "Equipment", 
            Action = "ByFacilityIds",
            PromptText = "Please select a facility.")]
        public List<int> Equipment { get; set; } = new List<int>();

        [CheckBoxList("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = nameof(State), DependentsRequired = DependentRequirement.None)]
        public override int[] OperatingCenters { get; set; }

        #endregion

        #region Private Methods

        public void MapFacilities(EnvironmentalPermit entity)
        {
            var facilityRepository = _container.GetInstance<IFacilityRepository>();
            entity.Facilities.Clear();
            foreach (var f in Facilities)
            {
                entity.Facilities.Add(facilityRepository.Find(f));
            }
        }

        public void MapEquipment(EnvironmentalPermit entity)
        {
            var equipmentRepository = _container.GetInstance<IEquipmentRepository>();
            entity.Equipment.Clear();
            foreach (var e in Equipment)
            {
                entity.Equipment.Add(equipmentRepository.Find(e));
            }
        }

        #endregion

        #region Exposed Methods

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            entity = base.MapToEntity(entity);

            if (OperatingCenters != null)
            {
                var operatingCenterRepository = _container.GetInstance<IOperatingCenterRepository>();
                foreach (var oc in OperatingCenters)
                {
                    entity.OperatingCenters.Add(operatingCenterRepository.Find(oc));
                }
            }

            MapFacilities(entity);
            MapEquipment(entity);

            CreateEnvironmentalPermitRequirement?.MapToEntity(entity);

            return entity;
        }
        
        #endregion
    }
}