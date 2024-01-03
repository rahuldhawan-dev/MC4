using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCallMVC.Controllers;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.PlanReviews
{
    public class PlanReviewViewModel : ViewModel<PlanReview>
    {
        #region Constructors

        public PlanReviewViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required]
        public DateTime? ReviewDate { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Employee)), 
         Description("Enter the employee's first or last name to populate autocomplete list"), 
         AutoComplete("Employee", nameof(EmployeeController.PlanActiveEmployeesByRoleAndPartial), 
             DisplayProperty = nameof(EmployeeDisplayItem.Display))]
        public int? ReviewedBy { get; set; }

        [Required, Multiline, StringLength(PlanReview.StringLengths.REVIEW_CHANGE_NOTES)]
        public string ReviewChangeNotes { get; set; }

        [Required]
        public DateTime? NextReviewDate { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(EmergencyResponsePlan))]
        public int? Plan { get; set; }

        #endregion
    }
}