using MapCall.Common.Model.Entities;
using MMSINC.Data;
using System;
using System.ComponentModel.DataAnnotations;
using StructureMap;
using MMSINC.Metadata;
using MMSINC.Validation;
using DataAnnotationsExtensions;
using MMSINC.Utilities.ObjectMapping;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public abstract class EmployeeHeadCountViewModel : ViewModel<EmployeeHeadCount>
    {
        #region Properties

        // State isn't a property on the entity itself.
        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public int? State { get; set; }
        
        // OperaetingCenter also isn't a property on the entity itself.
        [DoesNotAutoMap, EntityMustExist(typeof(OperatingCenter))]
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = nameof(State))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "BusinessUnit", "FindByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMustExist(typeof(BusinessUnit)), EntityMap]
        public int? BusinessUnit { get; set; }

        [Range(1000, 9999)]
        public int? Year { get; set; }

        [CompareTo(nameof(EndDate), ComparisonType.LessThanOrEqualTo, TypeCode.DateTime, IgnoreNullValues = true)]
        public DateTime? StartDate { get; set; }

        [CompareTo(nameof(StartDate), ComparisonType.GreaterThanOrEqualTo, TypeCode.DateTime, IgnoreNullValues = true)]
        public DateTime? EndDate { get; set; }

        [DropDown, EntityMustExist(typeof(EmployeeHeadCountCategory)), EntityMap]
        public int? Category { get; set; }

        [Required, Min(0)]
        public int? NonUnionCount { get; set; }

        [Required, Min(0)]
        public int? UnionCount { get; set; }

        [Required, Min(0)]
        public int? OtherCount { get; set; }

        [View("Notes"), Multiline]
        public string MiscNotes { get; set; }

        #endregion

        #region Constructor

        protected EmployeeHeadCountViewModel(IContainer container) : base(container) { }

        #endregion

        #region Public Methods
              
        public override void Map(EmployeeHeadCount entity)
        {
            base.Map(entity);
            var opc = entity.BusinessUnit?.OperatingCenter;
            OperatingCenter = opc?.Id;
            State = opc?.State.Id;
        }

        public override EmployeeHeadCount MapToEntity(EmployeeHeadCount entity)
        {
            base.MapToEntity(entity);
            entity.TotalCount = UnionCount.GetValueOrDefault() + NonUnionCount.GetValueOrDefault() + OtherCount.GetValueOrDefault();
            return entity;
        }

        #endregion
    }
}