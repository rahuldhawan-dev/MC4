using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateEquipment : CreateEquipmentBase
    {
        #region Properties

        [Required]
        public override DateTime? DateInstalled { get; set; }

        [Required]
        public override int? EquipmentPurpose { get; set; }

        [Required]
        public override int? ABCIndicator { get; set; }

        [Required]
        public override int? RequestedBy { get; set; }

        [Required, DropDown("", "EquipmentManufacturer", "ByEquipmentTypeId", DependsOn = "EquipmentType", PromptText = "Please select an Equipment Type above")]
        public override int? EquipmentManufacturer { get; set; }

        [ClientCallback("EquipmentForm.manufacturerOtherKnown", ErrorMessage = "Please enter the manufacturer")]
        public override string ManufacturerOther { get; set; }

        [DoesNotAutoMap]
        public string EquipmentTypesWithLockoutRequirements { get; set; }

        #endregion

        #region Constructors

        public CreateEquipment(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            RequestedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser?.Employee?.Id ??
                          RequestedBy;

            var equipmentTypes = _container.GetInstance<IRepository<EquipmentType>>().Where(x => x.IsLockoutRequired).Select(x => x.Id).ToArray();
            EquipmentTypesWithLockoutRequirements = string.Join(",", equipmentTypes);
        }

        #endregion
    }

    public abstract class CreateEquipmentBase : EquipmentViewModel
    {
        #region Properties

        public override int? EquipmentManufacturer { get; set; }

        #endregion

        #region Constructors

        public CreateEquipmentBase(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        // For reasons I do not know, this validation is only done on the create model.
        // Client-side validation is done in the CreateEquipment model.
        private IEnumerable<ValidationResult> ValidateManufacturerOther()
        {
            // This field is already Required. We can skip this validation if there's no value.
            if (!EquipmentManufacturer.HasValue)
            {
                yield break;
            }

            var manu = _container.GetInstance<IRepository<EquipmentManufacturer>>().Find(EquipmentManufacturer.Value);

            // Don't care if manu is null, it's already covered by EntityMustExist.
            if (manu != null && manu.MapCallDescription == MapCall.Common.Model.Entities.EquipmentManufacturer.MAPCALLDESCRIPTION_OTHER && string.IsNullOrWhiteSpace(ManufacturerOther))
            {
                yield return new ValidationResult("Manufacturer Other is required.", new[] { nameof(ManufacturerOther) });
            }
        }

        #endregion

        #region Exposed Methods

        public override Equipment MapToEntity(Equipment entity)
        {
            entity = base.MapToEntity(entity);

            SetNumber(entity);
            if (ReplacedEquipment.HasValue)
            {
                SetEquipmentToPendingRetirement(ReplacedEquipment.Value, entity);
            }

            return entity;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();
            ABCIndicator = _container.GetInstance<IRepository<ABCIndicator>>().Find(MapCall.Common.Model.Entities.ABCIndicator.Indices.HIGH)?.Id;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateManufacturerOther());
        }

        #endregion
    }
}