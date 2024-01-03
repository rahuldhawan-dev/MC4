using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public abstract class BasePersonnelAreaViewModel : ViewModel<PersonnelArea>
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [Required, DisplayName("Personnel Area Id")]
        public int? PersonnelAreaId { get; set; }

        [Required, StringLength(PersonnelArea.MAX_DESCRIPTION_LENGTH)]
        public string Description { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        protected BasePersonnelAreaViewModel(IContainer container) : base(container) { }

        #endregion

        #region Private Methods

        private IEnumerable<ValidationResult> ValidatePersonnelAreaIdUniqueness()
        {
            var hasExistingPersonnelArea = _container.GetInstance<PersonnelAreaRepository>().Where(x => x.PersonnelAreaId == PersonnelAreaId && x.Id != Id).Any();
            if (hasExistingPersonnelArea)
            {
                yield return new ValidationResult(
                    "The given Personnel Area ID is already being used by another Personnel Area.",
                    new[] { nameof(PersonnelAreaId) });
            }
        }

        #endregion

        #region Public Methods

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidatePersonnelAreaIdUniqueness());
        }

        #endregion
    }

    public class CreatePersonnelArea : BasePersonnelAreaViewModel
    {
        public CreatePersonnelArea(IContainer container) : base(container) { }
    }

    public class EditPersonnelArea : BasePersonnelAreaViewModel
    {
        public EditPersonnelArea(IContainer container) : base(container) { }
    }

    public class SearchPersonnelArea : SearchSet<PersonnelArea>
    {
        #region Properties

        public int? PersonnelAreaId { get; set; }
        public string Description { get; set; }
        [DropDown]
        public int? OperatingCenter { get; set; }

        #endregion
    }
}