using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class FunctionalLocationViewModel : ViewModel<FunctionalLocation>
    {
        #region Properties

        [Required, StringLength(EquipmentModel.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        // Operating Center
        [DropDown, EntityMap(MapDirections.None)]
        public int? OperatingCenter { get; set; }

        // Town
        [DropDown(Area = "", Controller = "Town", Action = "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        [DisplayName("Town")]
        [EntityMustExist(typeof(Town))]
        [EntityMap]
        public virtual int? Town { get; set; }

        [DisplayName("Asset Type")]
        [EntityMustExist(typeof(AssetType))]
        [EntityMap]
        [DropDown]
        public virtual int? AssetType { get; set; }
        
        [Required]
        public bool? IsActive { get; set; }

        #endregion

        #region Constructors

        public FunctionalLocationViewModel(IContainer container) : base(container) { }

        #endregion

        public override string ToString()
        {
            return Description;
        }
    }

    public class CreateFunctionalLocation : FunctionalLocationViewModel
    {
        #region Constructors

        public CreateFunctionalLocation(IContainer container) : base(container) { }

        #endregion
    }

    public class EditFunctionalLocation : FunctionalLocationViewModel
    {
        #region Constructors

        public EditFunctionalLocation(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void Map(FunctionalLocation entity)
        {
            base.Map(entity);
            // I don't know why we don't set this otherwise. This is what FunctionalLocationController.Edit
            // was doing.
            if (entity.Town != null)
            {
                OperatingCenter = entity.Town.OperatingCenters.First().Id;
            }
        }

        #endregion
    }

    public class SearchFunctionalLocation : SearchSet<FunctionalLocation>
    {
        #region Properties

        public string Description { get; set; }
        
        [DropDown, DoesNotExport, Search(CanMap = false)]
        public int? OperatingCenter { get; set; }

        [DropDown(Area = "", Controller = "Town", Action = "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an operating center above")]
        public int? Town { get; set; }
        [DropDown]
        public int? AssetType { get; set; }
        public bool? IsActive { get; set; }

        #endregion

    }
}