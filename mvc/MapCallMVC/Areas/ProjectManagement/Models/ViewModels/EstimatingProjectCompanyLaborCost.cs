using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EstimatingProjectCompanyLaborCostViewModel : ViewModel<EstimatingProjectCompanyLaborCost>
    {
        #region Properties

        [Required, EntityMustExist(typeof(EstimatingProject)), EntityMap]
        public virtual int? EstimatingProject { get; set; }

        [DropDown, Required, EntityMustExist(typeof(CompanyLaborCost)), EntityMap]
        public virtual int? CompanyLaborCost { get; set; }

        [DropDown, Required, EntityMustExist(typeof(AssetType)), EntityMap]
        public virtual int AssetType { get; set; }

        [Required]
        public virtual int Quantity { get; set; }

        #endregion
        
        #region Constructors

        public EstimatingProjectCompanyLaborCostViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class EditEstimatingProjectCompanyLaborCost : EstimatingProjectCompanyLaborCostViewModel
    {
        public EditEstimatingProjectCompanyLaborCost(IContainer container) : base(container) {}        
    }

    public class CreateEstimatingProjectCompanyLaborCost : EstimatingProjectCompanyLaborCostViewModel
    {
        #region Constructors

        public CreateEstimatingProjectCompanyLaborCost(IContainer container) : base(container) {}

        #endregion
    }
}