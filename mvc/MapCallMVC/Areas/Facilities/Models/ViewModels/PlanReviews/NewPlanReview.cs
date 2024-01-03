using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews
{
    public class NewPlanReview : ViewModel<PlanReview>
    {
        #region Constructor

        public NewPlanReview(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(EmergencyResponsePlan))]
        public int? Plan { get; set; }

        #endregion
    }
}