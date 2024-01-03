using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews
{
    public class SearchPlanReview : SearchSet<PlanReview>
    {
        #region Properties

        [EntityMap, EntityMustExist(typeof(EmergencyResponsePlan))]
        public int? Plan { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter)), 
         SearchAlias("Plan", "OperatingCenter.Id", Required = true)]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(User)), 
         DropDown("", "User", "LockoutFormUsersByOperatingCenterId", 
             DependsOn = nameof(OperatingCenter))]
        public int? CreatedBy { get; set; }

        public DateRange ReviewDate { get; set; }

        public DateRange NextReviewDate { get; set; }

        #endregion
    }
}