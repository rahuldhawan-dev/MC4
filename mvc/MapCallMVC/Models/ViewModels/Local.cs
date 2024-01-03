using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class LocalViewModel : ViewModel<Local>
    {
        #region Properties

        [DropDown]
        [EntityMustExist(typeof(Union))]
        [EntityMap]
        [Display(Name = "Bargaining Unit"), Required]
        public virtual int? Union { get; set; }
        [DropDown]
        [EntityMustExist(typeof(OperatingCenter))]
        [EntityMap]
        [Display(Name = "OperatingCenter"), Required]
        public virtual int? OperatingCenter { get; set; }
        [StringLength(Local.StringLengths.LOCAL), Display(Name = "Local"), Required]
        public virtual string Name { get; set; }
        [StringLength(Local.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }
        [Required, StringLength(Local.StringLengths.SAP_UNION_DESCRIPTION)]
        public virtual string SAPUnionDescription { get; set; }

        [Coordinate, Display(Name = "Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        [Required]
        public virtual int? Coordinate { get; set; }

        public virtual bool IsActive { get; set; }

        [DropDown("", "Division", "ByStateId", DependsOn = "State", PromptText = "Select a state above")]
        [EntityMustExist(typeof(Division))]
        [EntityMap]
        public virtual int? Division { get; set; }

        [EntityMustExist(typeof(State))]
        [EntityMap]
        [DropDown]
        public virtual int? State { get; set; }

        #endregion

        #region Constructors

        public LocalViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class EditLocal : LocalViewModel
    {
        #region Constructors

        public EditLocal(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateLocal : LocalViewModel
    {
        #region Constructors

        public CreateLocal(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchLocal : SearchSet<Local>
    {
        #region Properties

        [DropDown, Display(Name = "Bargaining Unit")]
        public virtual int? Union { get; set; }

        [Display(Name = "Local")]
        public virtual string Name { get; set; }

        [DisplayName("Local")]
        [DropDown("", "Local", "ByUnionId", DependsOn = "Union", PromptText = "Please select a union")]
        public virtual int? EntityId { get; set; }

        public virtual bool? IsActive { get; set; }

        // State
        [DropDown]
        public virtual int? State { get; set; }

        // Description
        public string Description { get; set; }

        // Operating Center
        [DropDown("", "OperatingCenter", "ByStateId", DependsOn = "State", PromptText = "Please select a state")]
        public virtual int? OperatingCenter { get; set; }

        // Division
        [DropDown("", "Division", "ByStateId", DependsOn = "State", PromptText = "Please select a state")]
        public virtual int? Division { get; set; }

        // SAP Union Description
        public virtual string SAPUnionDescription { get; set; }

        #endregion

    }
}