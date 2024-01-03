using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class RemoveEnvironmentalPermitFee : ViewModel<EnvironmentalPermit>
    {
        #region Properties

        [DoesNotAutoMap("Not a direct property on EnvironmentalPermit")]
        [Required, EntityMustExist(typeof(EnvironmentalPermitFee))]
        public int? EnvironmentalPermitFeeId { get; set; }

        #endregion

        #region Constructor

        public RemoveEnvironmentalPermitFee(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override EnvironmentalPermit MapToEntity(EnvironmentalPermit entity)
        {
            // Don't call base, there's nothing to auto map.

            var feeToRemove = entity.Fees.SingleOrDefault(x => x.Id == EnvironmentalPermitFeeId.Value);
            if (feeToRemove != null)
            {
                entity.Fees.Remove(feeToRemove);
            }

            return entity;
        }

        private IEnumerable<ValidationResult> ValidatePermitFee()
        {
            var permit = _container.GetInstance<IRepository<EnvironmentalPermit>>().Find(Id);
            if (permit == null)
            {
                // Cut out early. Controller will deal with 404'ing this.
                yield break;
            }

            if (!permit.Fees.Any(x => x.Id == EnvironmentalPermitFeeId.Value))
            {
                yield return new ValidationResult($"Fee#{EnvironmentalPermitFeeId} does not exist for permit#{Id}.", new[] {nameof(EnvironmentalPermitFeeId)});
            }
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return base.Validate(validationContext).Concat(ValidatePermitFee());
        }

        #endregion
    }
}