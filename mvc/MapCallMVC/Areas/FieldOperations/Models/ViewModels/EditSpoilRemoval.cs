using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;
using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Data.NHibernate;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditSpoilRemoval : SpoilRemovalViewModel
    {
        #region Properties
        private SpoilRemoval _original;

        [DoesNotAutoMap("Display only")]
        public SpoilRemoval SpoilRemovalDisplay
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<SpoilRemoval>>().Find(Id);
                }
                return _original;
            }
        }
        [DoesNotAutoMap]
        public override int? State { get; set; }

        [DoesNotAutoMap]
        public override int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public EditSpoilRemoval(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(SpoilRemoval entity)
        {
            base.Map(entity);
            State = entity.RemovedFrom.OperatingCenter.State.Id;
            OperatingCenter = entity.RemovedFrom.OperatingCenter.Id;
            RemovedFrom = entity.RemovedFrom.Id;
            FinalDestination = entity.FinalDestination.Id;
        }

        #endregion
    }
}