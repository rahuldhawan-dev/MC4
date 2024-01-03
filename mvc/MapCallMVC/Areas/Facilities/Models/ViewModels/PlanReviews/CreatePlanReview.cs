using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews
{
    public class CreatePlanReview : PlanReviewViewModel
    {
        #region Constructor

        public CreatePlanReview(IContainer container) : base(container) { }

        #endregion
    }
}