using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public abstract class CreateSewerOpeningBase : SewerOpeningViewModel
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Town", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Select an operating center above")]
        [Required, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [DropDown("", "TownSection", "ActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(TownSection))]
        public override int? TownSection { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [Required, EntityMap, EntityMustExist(typeof(Street))]
        public int? Street { get; set; }

        [DropDown("", "Street", "GetActiveByTownId", DependsOn = "Town", PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(Street))]
        public int? IntersectingStreet { get; set; }

        [DropDown("FieldOperations", "FunctionalLocation", "ActiveByTownIdForSewerOpeningAssetType", DependsOn = "Town",
            PromptText = "Select a town above")]
        [EntityMap, EntityMustExist(typeof(FunctionalLocation))]
        public override int? FunctionalLocation
        {
            get { return base.FunctionalLocation; }
            set { base.FunctionalLocation = value; }
        }

        [StringLength(SewerOpening.StringLengths.TASK_NUMBER)]
        [Required]
        public string TaskNumber { get; set; }

        #endregion

        #region Constructors

        public CreateSewerOpeningBase(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateFunctionalLocation()
        {
            if (!FunctionalLocation.HasValue)
            {
                var opc = _container.GetInstance<IRepository<OperatingCenter>>().Find(OperatingCenter.Value);
                if (opc != null && !opc.IsContractedOperations && opc.SAPEnabled)
                {
                    yield return new ValidationResult("The Functional Location field is required.", new[] { "FunctionalLocation" });
                }
            }
        }

        #endregion

        #region Exposed Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateFunctionalLocation());
        }

        #endregion
    }
}
