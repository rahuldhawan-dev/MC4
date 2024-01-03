using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class MeasurementPointEquipmentTypeViewModel : ViewModel<MeasurementPointEquipmentType>
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(EquipmentType))]
        public virtual int EquipmentType { get; set; }

        [Required, StringLength(MeasurementPointEquipmentType.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [Required, StringLength(MeasurementPointEquipmentType.StringLengths.CATEGORY)]
        public virtual string Category { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(UnitOfMeasure))]
        public virtual int? UnitOfMeasure { get; set; }
        
        [Required, CompareTo(nameof(Max), ComparisonType.LessThanOrEqualTo, TypeCode.Decimal)]
        public virtual decimal? Min { get; set; }
        
        [Required]
        public virtual decimal? Max { get; set; }
        
        [Required]
        public virtual int? Position { get; set; }
        
        [Required]
        public virtual bool? IsActive { get; set; }
        
        #endregion

        #region Constructors

        public MeasurementPointEquipmentTypeViewModel(IContainer container) : base(container) { }

        #endregion
    }
}