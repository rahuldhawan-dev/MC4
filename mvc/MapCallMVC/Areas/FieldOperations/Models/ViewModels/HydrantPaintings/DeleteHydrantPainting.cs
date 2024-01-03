using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    public class DeleteHydrantPainting : ViewModel<HydrantPainting>
    {
        #region Properties

        [Required, EntityMustExist(typeof(HydrantPainting))]
        public override int Id { get; set; }

        #endregion

        #region Constructors

        public DeleteHydrantPainting(IContainer container) : base(container) { }

        #endregion
    }
}
