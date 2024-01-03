using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class EditEnvironmentalPermit : EnvironmentalPermitViewModel
    {
        #region Constants

        public const string REQUIRES_REQUIREMENTS_VALIDATION_ERROR =
            "Please enter the requirements from the regular view for this permit before setting this value.";

        #endregion

        #region Constructors

        public EditEnvironmentalPermit(IContainer container) : base(container) { }

        #endregion

        #region Properties
        
        /// <summary>
        /// This one is going to require that child elements have been entered in another tab
        /// If it's set to yes, then one has to have been entered in order to save.
        /// </summary>
        [Required,
         ClientCallback("EnvironmentalPermit.validateRequiresRequirements", ErrorMessage = REQUIRES_REQUIREMENTS_VALIDATION_ERROR)]
        public override bool? RequiresRequirements { get; set; }

        [DoesNotAutoMap]
        public virtual int RequirementCount { get; set; }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidateRequiresRequirements()
        {
            if (RequiresRequirements.GetValueOrDefault() && RequirementCount == 0)
            {
                yield return new ValidationResult(REQUIRES_REQUIREMENTS_VALIDATION_ERROR, new[] {
                    "RequiresRequirements"
                });
            }
        }

        #endregion

        #region Exposed Methods

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            entity = base.MapToEntity(entity);
            
            var operatingCenterRepository = _container.GetInstance<IOperatingCenterRepository>();
            entity.OperatingCenters.Clear();

            if (OperatingCenters != null)
            {
                foreach (var oc in OperatingCenters)
                {
                    entity.OperatingCenters.Add(operatingCenterRepository.Find(oc));
                }
            }

            return entity;
        }

        public override void Map(EnvironmentalPermit entity)
        {
            base.Map(entity);

            // TODO: It looks like we're grabbing all of the operating centers just so they're 
            // sorted correctly? Create a helper method that can sort any IEnumerable<OperatingCenter> 
            // so we aren't hitting up the db to get data we already have.
            var allOpCenters = _container.GetInstance<IOperatingCenterRepository>().GetAllSorted();
            OperatingCenters = allOpCenters.Where(x => entity.OperatingCenters.Contains(x)).Select(x => x.Id).ToArray();

            RequirementCount = entity.Requirements.Count;

            // Greg - Add this to make sure that the Permit Expires checkbox is persistent. 
            if (PermitExpirationDate.HasValue)
            {
                PermitExpires = true;
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext)
                       .Concat(ValidateRequiresRequirements());
        }
        
        #endregion
    }
}