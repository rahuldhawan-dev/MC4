using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EditEstimatingProjectOtherCost : EstimatingProjectOtherCostViewModel
    {
        #region Properties

        [DropDown, EntityMap, Required, EntityMustExist(typeof(AssetType))]
        public virtual int? AssetType { get; set; }

        #endregion

        #region Constructors

        public EditEstimatingProjectOtherCost(IContainer container) : base(container) { }

        #endregion
    }
}