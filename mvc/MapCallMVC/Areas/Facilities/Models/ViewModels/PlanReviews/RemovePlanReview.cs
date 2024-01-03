using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews
{
    public class RemovePlanReview : ViewModel<EmergencyResponsePlan>
    {
        #region Constructors

        public RemovePlanReview(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, DoesNotAutoMap, EntityMustExist(typeof(PlanReview))]
        public int? PlanReviewId { get; set; }

        #endregion

        #region Public Methods

        public override EmergencyResponsePlan MapToEntity(EmergencyResponsePlan entity)
        {
            var reviewToRemove = entity.Reviews.Single(x => x.Id == PlanReviewId.GetValueOrDefault());
            entity.Reviews.Remove(reviewToRemove);
            return entity;
        }

        #endregion
    }
} 