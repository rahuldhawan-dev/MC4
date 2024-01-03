using MMSINC.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class FireDistrictTownViewModel : ViewModel<FireDistrictTown>
    {
        #region Constructors

        public FireDistrictTownViewModel(IContainer container) : base(container) {}

        #endregion

        #region Private Methods

        protected void RemoveDefaults(Town town)
        {
            var repo = _container.GetInstance<MMSINC.Data.NHibernate.IRepository<FireDistrictTown>>();

            foreach (var district in town.TownFireDistricts)
            {
                if (district.IsDefault)
                {
                    district.IsDefault = false;
                    repo.Save(district);
                }
            }
        }

        #endregion
    }

    public class CreateFireDistrictTown : FireDistrictTownViewModel
    {
        #region Properties

        public int FireDistrict { get; set; }

        [DoesNotAutoMap("Manually mapped.")]
        public int TownId { get; set; }
        public bool IsDefault { get; set; }

        #endregion

        #region Constructors

        public CreateFireDistrictTown(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override FireDistrictTown MapToEntity(FireDistrictTown entity)
        {
            entity = base.MapToEntity(entity);
            var town = _container.GetInstance<ITownRepository>().Find(TownId);

            entity.Town = town;
            entity.State = town.State;
            entity.IsDefault = IsDefault;
            entity.FireDistrict = _container.GetInstance<IFireDistrictRepository>().Find(FireDistrict);

            if (IsDefault)
            {
                RemoveDefaults(town);
            }

            return entity;
        }

        #endregion
    }

    public class MakeDefaultFireDistrictTown : FireDistrictTownViewModel
    {
        public MakeDefaultFireDistrictTown(IContainer container) : base(container) {}

        public override FireDistrictTown MapToEntity(FireDistrictTown entity)
        {
            entity = base.MapToEntity(entity);
            RemoveDefaults(entity.Town);
            entity.IsDefault = true;
            return entity;
        }
    }
}