using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using MapCall.Common.ClassExtensions.WorkOrderAssetViewModelExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Helpers;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class EditEquipment : EditEquipmentBase
    {
        [Required]
        public override int? RequestedBy { get; set; }

        [DoesNotAutoMap]
        public string EquipmentTypesWithLockoutRequirements
        {
            get
            {
                var equipmentTypes = _container.GetInstance<IRepository<EquipmentType>>().Where(x => x.IsLockoutRequired).Select(x => x.Id).ToArray();
                return string.Join(",", equipmentTypes);
            }
        }

        public EditEquipment(IContainer container) : base(container) { }
    }

    public abstract class EditEquipmentBase : EquipmentViewModel
    {
        #region Constants

        public const string SAP_ERROR_CODE = "Invalid SAP Equipment No";

        #endregion

        #region Properties

        [DropDown(Area = "", Controller = "Facility", Action = "ActiveAndPendingRetirementByOperatingCenterOrPlanningPlant", DependsOn = "OperatingCenter,PlanningPlant", PromptText = "Please select an operating center above", DependentsRequired = DependentRequirement.One)]
        [View("Facility")]
        [EntityMustExist(typeof(Facility))]
        [EntityMap]
        [Required]
        public override int? Facility { get; set; }

        [DoesNotAutoMap]
        public bool SAPEquipmentIdEditable => !SAPEquipmentId.HasValue ||
                                              SAPEquipmentId == 0 ||
                                              (SAPErrorCode != null && SAPErrorCode.StartsWith(SAP_ERROR_CODE));

        [Required, DropDown("", "EquipmentPurpose", "ByEquipmentTypeId", DependsOn = "EquipmentType")]
        public override int? EquipmentPurpose { get; set; }

        [Required]
        public override int? ABCIndicator { get; set; }

        [DropDown("", "EquipmentManufacturer", "ByEquipmentTypeId", DependsOn = "EquipmentType", PromptText = "Please select an Equipment Type above")]
        public override int? EquipmentManufacturer { get; set; }

        [EntityMap(MapDirections.None)]
        public IList<EquipmentCharacteristic> Characteristics { get; set; }

        [EntityMap(MapDirections.None)]
        public IList<EquipmentCharacteristicField> Fields { get; set; }

        [DoesNotAutoMap("Used for view logic only.")]
        public bool HasCharacteristics { get; protected set; }

        [DoesNotAutoMap]
        public FormCollection Form { get; set; }

        [DisplayName("Equipment Number")]
        public virtual int Number { get; set; }

        [AutoMap(MapDirections.ToViewModel)]
        public virtual string SAPErrorCode { get; set; }

        [DoesNotAutoMap]
        public virtual string FacilityShowUrl { get; set; }

        [Max(9999, ErrorMessage = "Year cannot be longer than 4 digits")]
        public virtual int? PlannedReplacementYear { get; set; }

        public virtual decimal? EstimatedReplaceCost { get; set; }
        
        [DropDown("Equipment", "ByFacilityFunctionalLocation", DependsOn = "Facility", PromptText = "Please select a Facility above")]
        [EntityMap, EntityMustExist(typeof(Equipment))]
        public override int? ParentEquipment { get; set; }

        #endregion

        #region Constructors

        public EditEquipmentBase(IContainer container) : base(container)
        {
            Characteristics = Characteristics ?? new List<EquipmentCharacteristic>();
            Fields = Fields ?? new List<EquipmentCharacteristicField>();
        }

        #endregion

        #region Private Methods

        private bool IsValidField(string key)
        {
            return !GetType().HasPropertyNamed(key) && key != FormBuilder.SECURE_FORM_HIDDEN_FIELD_NAME &&
                   key != "undefined";
        }

        private void MapCharacteristics(Equipment entity)
        {
            var fieldRepository = _container.GetInstance<IRepository<EquipmentCharacteristicField>>();

            Form.Where(
                     (k, v) =>
                         IsValidField(k) && !String.IsNullOrWhiteSpace(v) && v != SelectAttribute.DEFAULT_ITEM_LABEL)
                .Each((k, v) => Characteristics.Add(new EquipmentCharacteristic {
                     Equipment = entity,
                     Field = fieldRepository.GetByEquipmentTypeAndName(entity.EquipmentType.Id, k),
                     Value = v
                 }));

            foreach (var characteristic in Characteristics)
            {
                if (entity.Characteristics.All(c => c.Field.FieldName != characteristic.Field.FieldName))
                {
                    entity.Characteristics.Add(characteristic);
                }
                else
                {
                    entity.Characteristics.Single(c => c.Field.FieldName == characteristic.Field.FieldName)
                          .Value = characteristic.Value;
                }
            }

            Form.Where(
                     (k, v) => IsValidField(k) && v == SelectAttribute.DEFAULT_ITEM_LABEL ||
                               String.IsNullOrWhiteSpace(v))
                .Each((k, v) => {
                     var toRemove = entity.Characteristics.SingleOrDefault(c => c.Field.FieldName == k);
                     if (toRemove != null)
                     {
                         entity.Characteristics.Remove(toRemove);
                     }
                 });
        }
        #endregion

        #region Exposed Methods

        public override void Map(Equipment entity)
        {
            base.Map(entity);
            if (entity.EquipmentType != null)
            {
                Fields = entity.EquipmentType.CharacteristicFields;
            }

            if (SAPEquipmentId.HasValue)
            {
                SAPEquipmentIdOverride = true;
            }

            Characteristics = entity.Characteristics;
            HasCharacteristics = entity.Characteristics.Count > 0;
            RequestedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser?.Employee?.Id ??
                          RequestedBy;
        }

        public override Equipment MapToEntity(Equipment entity)
        {
            this.MaybeCancelWorkOrders(entity, _container.GetInstance<IRepository<WorkOrderCancellationReason>>(),
                t => t.DateRetired);

            if (entity.EquipmentType != null)
            {
                MapCharacteristics(entity);
            }

            if (EquipmentPurpose.HasValue && entity.Facility != null &&
                ((entity.EquipmentPurpose != null && entity.EquipmentPurpose.Id != EquipmentPurpose)
                 ||
                 (entity.EquipmentPurpose == null && EquipmentPurpose != null)
                 ||
                 (entity.Facility.Id != Facility)
                ))
            {
                var facilityRepo = _container.GetInstance<IFacilityRepository>();
                var facility = facilityRepo.Find(Facility.Value);
                var equipmentPurpose =
                    _container.GetInstance<IRepository<EquipmentPurpose>>()
                              .Find(EquipmentPurpose.Value);
                var number = facilityRepo.GetNextEquipmentNumberForFacilityByEquipmentPurposeId(Facility.Value,
                    EquipmentPurpose.Value);

                entity = base.MapToEntity(entity);

                entity.Number = number;
            }
            else
            {
                entity = base.MapToEntity(entity);
            }

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = base.Validate(validationContext).ToList();

            if (Original != null && Original.EquipmentType != null && Original.Characteristics.Count > 0 &&
                EquipmentType != Original.EquipmentType.Id)
            {
                results.Add(
                    new ValidationResult("Cannot change Equipment Type after Characteristics have been set."));
            }

            results.AddRange(from field in Fields.Where(f => f.Required)
                             let characteristic =
                                 Characteristics.SingleOrDefault(c => c.Field.FieldName == field.FieldName)
                             where characteristic == null
                             select new ValidationResult(String.Format(
                                 "Characteristic for required field '{0}' has not been provided.", field.FieldName)));

            foreach (var characteristic in Characteristics)
            {
                results.AddRange(characteristic.Validate(validationContext));
            }
            return results;
        }

        #endregion
    }
}
