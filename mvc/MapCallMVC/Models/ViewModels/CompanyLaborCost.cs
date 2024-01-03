using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CompanyLaborCostViewModel : ViewModel<CompanyLaborCost>
    {
        #region Properties

        [Required, StringLength(AddNJAWLaborTableForBug1778.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }
        [Required, StringLength(AddNJAWLaborTableForBug1778.StringLengths.UNIT)]
        public virtual string Unit { get; set; }
        [Required]
        public virtual decimal? Cost { get; set; }
        [StringLength(AddNJAWLaborTableForBug1778.StringLengths.LABOR_ITEM)]
        public virtual string LaborItem { get; set; }
        
        #endregion

        #region Constructors

        public CompanyLaborCostViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateCompanyLaborCost : CompanyLaborCostViewModel
    {
        public CreateCompanyLaborCost(IContainer container) : base(container) { }
    }

    public class EditCompanyLaborCost : CompanyLaborCostViewModel
    {
        public EditCompanyLaborCost(IContainer container) : base(container) { }
    }

    // TODO: What is this for? There aren't any properties to search for.
    public class SearchCompanyLaborCost : SearchSet<CompanyLaborCost>
    {
    }
}