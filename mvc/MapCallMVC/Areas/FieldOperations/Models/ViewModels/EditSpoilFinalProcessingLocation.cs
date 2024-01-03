using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditSpoilFinalProcessingLocation : SpoilFinalProcessingLocationViewModel
    {
        #region Properties

        private SpoilFinalProcessingLocation _original;

        [DoesNotAutoMap("Display only")]
        public SpoilFinalProcessingLocation Display
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<SpoilFinalProcessingLocation>>().Find(Id);
                }
                return _original;
            }
        }
        [EntityMap(MapDirections.None)]
        public override int? State { get; set; }

        [EntityMap(MapDirections.ToPrimary)]
        public override int? OperatingCenter { get; set; }
 
        #endregion

        #region Constructors

        public EditSpoilFinalProcessingLocation(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(SpoilFinalProcessingLocation entity)
        {
            base.Map(entity);
            State = entity.OperatingCenter?.State?.Id;
        }

        #endregion
    }
}