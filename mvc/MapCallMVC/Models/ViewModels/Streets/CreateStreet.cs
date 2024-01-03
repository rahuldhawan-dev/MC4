using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.Streets
{
    public class CreateStreet : StreetViewModel
    {
        #region Properties

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(State))]
        public virtual int? State { get; set; }

        [Required, DropDown("County", "ByStateId", DependsOn = "State"), EntityMap(MapDirections.None), EntityMustExist(typeof(County))]
        public virtual int? County { get; set; }

        [Required, DropDown("Town", "ByCountyId", DependsOn = "County"), EntityMap, EntityMustExist(typeof(Town))]
        public int? Town { get; set; }

        [Required]
        public bool IsActive { get; set; }

        protected override Town TryGetTownForStreetNameValidation()
        {
            if (!Town.HasValue)
            {
                return null;
            }
            return _container.GetInstance<IRepository<Town>>().Find(Town.Value);
        }

        #endregion

        #region Constructors

        public CreateStreet(IContainer container) : base(container) {}

        #endregion
    }
}