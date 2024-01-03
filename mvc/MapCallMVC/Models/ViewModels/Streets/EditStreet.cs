using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.Streets
{
    public class EditStreet : StreetViewModel
    {
        #region Private Members

        private Street _original;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display Only")]
        public Street Display
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<Street>>().Find(Id);
                }

                return _original;
            }
        }

        #endregion

        #region Constructors

        public EditStreet(IContainer container) : base(container) { }

        #endregion

        protected override Town TryGetTownForStreetNameValidation()
        {
            return Display?.Town;
        }
    }
}