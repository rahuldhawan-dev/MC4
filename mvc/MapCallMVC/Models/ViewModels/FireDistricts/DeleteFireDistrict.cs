using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.FireDistricts
{
    public class DeleteFireDistrict : ViewModel<FireDistrict>
    {
        [Required, EntityMustExist(typeof(FireDistrict))]
        public override int Id { get; set; }

        public DeleteFireDistrict(IContainer container) : base(container) { }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var district = _container.GetInstance<IFireDistrictRepository>().Find(Id);

            if (!district.CanBeDeleted)
            {
                yield return new ValidationResult(
                    "Cannot delete a Fire District which is linked to other records.");
            }
        }
    }
}
