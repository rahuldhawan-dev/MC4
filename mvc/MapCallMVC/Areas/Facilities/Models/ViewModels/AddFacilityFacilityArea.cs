using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using SecuredAttribute = MMSINC.Metadata.SecuredAttribute;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class AddFacilityFacilityArea : ViewModel<Facility>
    {
        #region Properties

        [Secured]
        [Required, EntityMustExist(typeof(Facility)), EntityMap(MapDirections.None)]
        public int? Facility { get; set; }

        [Required, DoesNotAutoMap]
        [DropDown] // Needed for cascades only
        public int? FacilityArea { get; set; }

        [Coordinate, View("Coordinates")]
        [EntityMustExist(typeof(Coordinate))]
        [EntityMap("Coordinate")]
        public int? Coordinate { get; set; }

        [EntityMap(MapDirections.None), EntityMustExist(typeof(FacilitySubArea))]
        [DropDown("Facilities", "FacilitySubArea", "ByFacilityArea", DependsOn = "FacilityArea",
            PromptText = "Select Area above")]
        public int? FacilitySubArea { get; set; }

        #endregion

        #region Constructors

        public AddFacilityFacilityArea(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(Facility entity)
        {
            base.Map(entity);
            Facility = entity.Id;
        }

        public override Facility MapToEntity(Facility entity)
        {
            var fp = new FacilityFacilityArea();
            fp.Facility = _container.GetInstance<IFacilityRepository>().Find(Facility.Value);
            fp.FacilityArea = _container.GetInstance<IRepository<FacilityArea>>().Find(FacilityArea.Value);

            if (entity.Coordinate != null)
            {
                fp.Coordinate = _container.GetInstance<ICoordinateRepository>().Find(Coordinate.Value);
            }

            if (FacilitySubArea != null)
            {
                fp.FacilitySubArea = _container.GetInstance<IRepository<FacilitySubArea>>()
                                               .Find(FacilitySubArea.Value);
            }
            entity.FacilityAreas.Add(fp);
            return entity;
        }

        #endregion
    }
}
