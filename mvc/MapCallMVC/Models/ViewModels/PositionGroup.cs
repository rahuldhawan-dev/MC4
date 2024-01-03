using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public abstract class BasePositionGroupViewModel : ViewModel<PositionGroup>
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [Required, StringLength(PositionGroup.StringLengths.GROUP)]
        public string Group { get; set; }

        [Required, StringLength(PositionGroup.StringLengths.POSITION_DESCRIPTION)]
        public string PositionDescription { get; set; }

        [Required, StringLength(PositionGroup.StringLengths.BUSINESS_UNIT)]
        public string BusinessUnit { get; set; }

        [Required, StringLength(PositionGroup.StringLengths.BUSINESS_UNIT_DESCRIPTION)]
        public string BusinessUnitDescription { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(SAPCompanyCode))]
        public int? SAPCompanyCode { get; set; }

        [DropDown, Required, EntityMap, EntityMustExist(typeof(PositionGroupCommonName))]
        public int? CommonName { get; set; }

        [Required, StringLength(PositionGroup.StringLengths.SAP_POSITION_GROUP_KEY)]
        public string SAPPositionGroupKey { get; set; }

        #endregion

        #region Constructors

        protected BasePositionGroupViewModel(IContainer container) : base(container) { }

        #endregion

        #region Validation

        private IEnumerable<ValidationResult> ValidateUniqueSAPPositionGroupKey() 
        {
            // Create: Test fails if any exist with same key
            // Create: Passes if no exist
            // Edit: Test fails if any exist with same key that aren't itself
            // Edit: Passes if itself is the only thing with key
            // Edit: PAsses if key changes and nothing exists

            if (string.IsNullOrWhiteSpace(SAPPositionGroupKey))
            {
                // This is already handled by the Required attribute, so break out early.
                yield break;
            }

            var existingPositionGroupIdWithSAPKey = _container.GetInstance<IRepository<PositionGroup>>().Where(x => x.SAPPositionGroupKey == SAPPositionGroupKey).Select(x => x.Id).SingleOrDefault();
            // if (create logic || edit logic)
            if (existingPositionGroupIdWithSAPKey > 0 && Id != existingPositionGroupIdWithSAPKey)
            {
                yield return new ValidationResult(
                    $"Position Group ID#{existingPositionGroupIdWithSAPKey} already has the SAPPositionGroupKey value \"{SAPPositionGroupKey}\".", new[] {nameof(SAPPositionGroupKey)});
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidateUniqueSAPPositionGroupKey());
        }

        #endregion
    }

    public class CreatePositionGroup : BasePositionGroupViewModel
    {
        #region Constructors

        public CreatePositionGroup(IContainer container) : base(container) { }

        #endregion
    }

    public class EditPositionGroup : BasePositionGroupViewModel
    {
        #region Constructors

        public EditPositionGroup(IContainer container) : base(container) { }

        #endregion
    }

    public class SearchPositionGroup : SearchSet<PositionGroup>
    {
        #region Properties

        public string Group { get; set; }
        public string PositionDescription { get; set; }
        public string BusinessUnit { get; set; }
        public string BusinessUnitDescription { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PositionGroupCommonName))]
        public int? CommonName { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(SAPCompanyCode))]
        public int? SAPCompanyCode { get; set; }

        public string SAPPositionGroupKey { get; set; }

        #endregion
    }

    public class SearchTrainingClassificationSum : SearchSet<TrainingClassificationSumReportItem>, ISearchTrainingClassificationSum
    {
        [SearchAlias("employee.OperatingCenter", "opCntr", "Id"), DropDown]
        public int? OperatingCenter { get; set; }

        [SearchAlias("commonName.TrainingRequirements", "requirement", "IsOSHARequirement")]
        [DisplayName("OSHA Requirement")]
        public bool? OSHARequirement { get; set; }

        [SearchAlias("requirement.TrainingModules", "module", "AmericanWaterCourseNumber")]
        public string ClassId { get; set; }

        [ComboBox]
        [SearchAlias("requirement.TrainingModules", "module", "Id")]
        public int? TrainingModule { get; set; }

        [DropDown, DisplayName("Common Name")]
        [SearchAlias("CommonName", "commonName", "Id")]
        public int? PositionGroupCommonName { get; set; }

        [View("Position"), DropDown("", "PositionGroup", "GetByCommonName", DependsOn = "PositionGroupCommonName")]
        public int? Id { get; set; }

        [Search(CanMap = false), MaxCurrentYear, Required]
        public int? Year { get; set; }

        public SearchTrainingClassificationSum()
        {
            EnablePaging = false;
        }
    }
}