using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class CorrectiveOrderProblemCodeViewModel : ViewModel<CorrectiveOrderProblemCode>
    {
        #region Properties

        [Required, StringLength(CorrectiveOrderProblemCode.StringLengths.CODE)]
        public virtual string Code { get; set; }

        [Required, StringLength(CorrectiveOrderProblemCode.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(EquipmentType))] 
        public int[] EquipmentTypes { get; set; }

        #endregion

        #region Constructors 

        public CorrectiveOrderProblemCodeViewModel(IContainer container) : base(container) { }

        #endregion
    }
}