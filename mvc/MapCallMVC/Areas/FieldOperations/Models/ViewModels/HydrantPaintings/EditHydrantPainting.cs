using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.HydrantPaintings
{
    public class EditHydrantPainting : HydrantPaintingViewModel
    {
        #region Properties

        [Secured, EntityMap(MapDirections.ToViewModel)]
        public override int? Hydrant { get; set; }

        [Required]
        public override DateTime? PaintedAt { get; set; }

        #endregion

        #region Constructors

        public EditHydrantPainting(IContainer container) : base(container) { }

        #endregion
    }
}
