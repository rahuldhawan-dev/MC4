using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class LargeServiceProjectViewModel : ViewModel<LargeServiceProject>
    {
        #region Properties

        [DropDown, Required, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [StringLength(LargeServiceProject.StringLengths.WBS_NUMBER)]
        public string WBSNumber { get; set; }

        [Required]
        [StringLength(LargeServiceProject.StringLengths.PROJECT_TITLE)]
        public string ProjectTitle { get; set; }

        [Required]
        [StringLength(LargeServiceProject.StringLengths.PROJECT_ADDRESS)]
        public string ProjectAddress { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AssetCategory))]
        public int? AssetCategory { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AssetType))]
        public int? AssetType { get; set; }

        [DropDown,EntityMap, EntityMustExist(typeof(PipeDiameter))]
        public int? ProposedPipeDiameter { get; set; }

        [StringLength(LargeServiceProject.StringLengths.CONTACT_NAME)]
        public string ContactName { get; set; }
        [StringLength(LargeServiceProject.StringLengths.CONTACT_EMAIL)]
        public string ContactEmail { get; set; }
        [StringLength(LargeServiceProject.StringLengths.CONTACT_PHONE)]
        public string ContactPhone { get; set; }
        
        public DateTime? InitialContactDate { get; set; }
        public DateTime? InServiceDate { get; set; }
    
        [Coordinate(AddressCallback = "LargeServiceProject.getAddress", IconSet = IconSets.Shovels), EntityMap]
        public int? Coordinate { get; set; }

        #endregion

        #region Constructors

        public LargeServiceProjectViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateLargeServiceProject : LargeServiceProjectViewModel
    {
        #region Constructors

		public CreateLargeServiceProject(IContainer container) : base(container) {}

        #endregion
	}

    public class EditLargeServiceProject : LargeServiceProjectViewModel
    {
        #region Constructors

		public EditLargeServiceProject(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchLargeServiceProject : SearchSet<LargeServiceProject>
    {
        #region Properties

        [DisplayName("Id")]
        public int? EntityId { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        public int? Town { get; set; }
        
        public string ProjectTitle { get; set; }
        public string ProjectAddress { get; set; }
        
        [DropDown]
        public int? AssetCategory { get; set; }
        [DropDown]
        public int? AssetType { get; set; }
        [DropDown]
        public int? ProposedPipeDiameter { get; set; }

        public DateRange InServiceDate { get; set; }

        public string CreatedBy { get; set; }

        #endregion
	}
}