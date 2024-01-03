using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class InterconnectionTestViewModel : ViewModel<InterconnectionTest>
    {
        #region Constructor

        public InterconnectionTestViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State)), Required]
        public virtual int? State { get; set; }

        [DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = nameof(State), PromptText = "Please select a State above."), 
         EntityMap(MapDirections.None), EntityMustExist(typeof(OperatingCenter)), Required]
        public virtual int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter"), 
         EntityMap, EntityMustExist(typeof(Facility)), Required]
        public int? Facility { get; set; }

        [Required]
        public DateTime? InspectionDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(InterconnectionInspectionRating)), Required]
        public int? InterconnectionInspectionRating { get; set; }

        public float? MaxFlowMGDAchieved { get; set; }

        public bool? AllValvesOperational { get; set; }

        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter)), 
         EntityMap, EntityMustExist(typeof(Employee)), Required]
        public int? Employee { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(Contractor))]
        public int? Contractor { get; set; }

        [EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        [Multiline, StringLength(InterconnectionTest.StringLengths.INSPECTION_COMMENTS)]
        public string InspectionComments { get; set; }

        [StringLength(InterconnectionTest.StringLengths.REPRESENTATIVE_ON_SITE)]
        public string RepresentativeOnSite { get; set; }

        #endregion

    }

    public class CreateInterconnectionTest : InterconnectionTestViewModel
    {
        #region Constructors

        public CreateInterconnectionTest(IContainer container) : base(container) { }

        #endregion
    }

    public class EditInterconnectionTest : InterconnectionTestViewModel
    {
        #region Constructors

        public EditInterconnectionTest(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DropDown("", "OperatingCenter", "ByStateIdOrAll", DependsOn = nameof(State), PromptText = "Please select a State above."),
         EntityMap(MapDirections.None), EntityMustExist(typeof(OperatingCenter)), Required]
        public override int? OperatingCenter { get; set; }

        #endregion

        #region Mapping

        public override void Map(InterconnectionTest entity)
        {
            base.Map(entity);

            State = entity.Facility?.OperatingCenter?.State?.Id ?? entity.Facility?.OperatingCenter?.State?.Id;
            OperatingCenter = entity.Facility?.OperatingCenter?.Id ?? entity.Facility?.OperatingCenter?.Id;
        }

        #endregion
    }
}