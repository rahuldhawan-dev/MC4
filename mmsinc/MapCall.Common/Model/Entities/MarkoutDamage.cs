using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.Data.ChangeTracking;
using MMSINC.Utilities;
using MMSINC.Metadata;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MarkoutDamage
        : IEntityWithCreationTimeTracking,
            IValidatableObject,
            IThingWithCoordinate,
            IThingWithNotes,
            IThingWithDocuments
    {
        #region Consts

        public struct StringLengths
        {
            public const int REQUEST_NUM = 20,
                             STREET = CreateMarkoutDamagesTableBug575.MAX_STREET,
                             CREATED_BY = CreateMarkoutDamagesTableBug575.MAX_CREATEDBY,
                             CROSS_STREET = CreateMarkoutDamagesTableBug575.MAX_CROSS_STREET,
                             EXCAVATOR = CreateMarkoutDamagesTableBug575.MAX_EXCAVATOR,
                             EXCAVATOR_ADDRESS = CreateMarkoutDamagesTableBug575.MAX_EXCAVATOR_ADDRESS,
                             EXCAVATOR_PHONE = CreateMarkoutDamagesTableBug575.MAX_EXCAVATOR_PHONE,
                             SAP_WORK_ORDER_ID = AddSAPWorkOrderToMarkoutDamagesBug575.MAX_SAP_WORK_ORDER_ID_LENGTH;
        }
        
        #endregion

        #region Properties

        #region Table Columns

        public virtual int Id { get; set; }
        public virtual string RequestNumber { get; set; }

        [Required]
        public virtual string CreatedBy { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; }

        [Required]
        public virtual string Street { get; set; }

        [Required]
        public virtual string NearestCrossStreet { get; set; }

        public virtual string Excavator { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.PhoneNumber)]
        public virtual string ExcavatorPhone { get; set; }

        public virtual string ExcavatorAddress { get; set; }

        [Required, DisplayFormat(DataFormatString = CommonStringFormats.DATETIME_WITHOUT_SECONDS)]
        public virtual DateTime DamageOn { get; set; }

        [Required]
        public virtual string DamageComments { get; set; }

        /// <summary>
        /// Old property not used anymore. Data still exists for historical purposes.
        /// The UtilityDamages property is what you want to use.
        /// </summary>
        [Obsolete("Don't use this. Use UtilityDamages.")]
        [View(Description = "Historical value. This value can no longer be set or modified.")]
        public virtual string UtilitiesDamaged { get; set; }
        public virtual string EmployeesOnJob { get; set; }

        [Required]
        public virtual bool IsMarkedOut { get; set; }

        [Required]
        public virtual bool IsMismarked { get; set; }

        public virtual int MismarkedByInches { get; set; }

        [Required]
        public virtual bool ExcavatorDiscoveredDamage { get; set; }

        [Required]
        public virtual bool ExcavatorCausedDamage { get; set; }

        [Required]
        [DisplayName("Was 911 Called?")]
        public virtual bool Was911Called { get; set; }

        [Required]
        public virtual bool WerePicturesTaken { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? ApprovedOn { get; set; }

        public virtual string SAPWorkOrderId { get; set; }

        public virtual bool HasAttachedPictures { get; set; }

        #endregion

        #region References

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;
        public virtual MarkoutDamageToType MarkoutDamageToType { get; set; }
        public virtual Employee SupervisorSignOffEmployee { get; set; }
        public virtual IList<MarkoutDamageDocument> MarkoutDamageDocuments { get; set; }
        public virtual IList<MarkoutDamageNote> MarkoutDamageNotes { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual IList<MarkoutDamageUtilityDamageType> UtilityDamages { get; set; } = new List<MarkoutDamageUtilityDamageType>();

        #endregion

        #region Logical Properties

        public virtual IList<IDocumentLink> LinkedDocuments => MarkoutDamageDocuments.Cast<IDocumentLink>().ToList();

        public virtual IList<INoteLink> LinkedNotes => MarkoutDamageNotes.Cast<INoteLink>().ToList();

        public virtual string TableName => nameof(MarkoutDamage) + "s";

        public virtual bool IsSignedOffBySupervisor => SupervisorSignOffEmployee != null;

        /// <summary>
        /// A terrible terrible hack property for passing the url
        /// for this record to a notification template.
        /// </summary>
        public virtual string RecordUrl { get; set; }

        [Display(Name = "Date & Request #")]
        public virtual string DateAndRequestNumber => DamageOn.ToString("d") + " " + RequestNumber;

        [Display(Name = "Town/Location")]
        public virtual string TownAndLocation => Town + " " + Street;

        [Display(Name = "Excavator")]
        public virtual string ExcavatorDescription => Excavator + " " + ExcavatorAddress + " " + ExcavatorPhone;

        #endregion

        #endregion

        #region Public Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
