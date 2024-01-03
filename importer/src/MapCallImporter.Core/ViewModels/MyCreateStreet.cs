using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCallMVC.Models.ViewModels.Streets;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateStreet : StreetViewModel
    {
        #region Properties

        [Required, DropDown, EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        #endregion

        #region Constructors

        public MyCreateStreet(IContainer container) : base(container) { }

        #endregion

        protected override Town TryGetTownForStreetNameValidation()
        {
            return _container.GetInstance<IRepository<Town>>().Find(Town.GetValueOrDefault());
        }
    }
}