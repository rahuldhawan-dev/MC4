using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels.WellTests
{
    public class NewWellTestViewModel : ViewModel<WellTest>
    {
        #region Constructors

        public NewWellTestViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Required,
         EntityMap,
         EntityMustExist(typeof(ProductionWorkOrder))]
        public int? ProductionWorkOrder { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [Required,
         EntityMap,
         EntityMustExist(typeof(Equipment))]
        public int? Equipment { get; set; }

        #endregion
    }
}