using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels.Easements
{
    public class EditEasement : EasementViewModel
    {
        #region Constructors

        public EditEasement(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Secured, EntityMap(MapDirections.ToViewModel)]
        public int? OperatingCenter { get; set; }

        #endregion

        #region Logical Properties

        private Easement _displayEasement;

        [DoesNotAutoMap]
        public Easement DisplayEasement
        {
            get
            {
                if (_displayEasement == null)
                {
                    _displayEasement = _container.GetInstance<IRepository<Easement>>().Find(Id);
                }
                return _displayEasement;
            }
        }

        #endregion
    }
}