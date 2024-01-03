using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IIconSetRepository : IRepository<IconSet>
    {
        #region Abstract Methods

        IconSet Find(IconSets iconSet);

        #endregion
    }

    public static class IconSetRepositoryExtensions
    {
        #region Exposed Methods

        public static IconSet GetDefaultIconSet(this IRepository<IconSet> that, IRepository<MapIcon> mapIconRepository)
        {
            var set = new IconSet {
                Icons = mapIconRepository.GetAll().ToList()
            };
            set.DefaultIcon = set.Icons.FirstOrDefault(i => i.FileName.Contains("pin_black"));

            return set;
        }

        #endregion
    }

    public class IconSetRepository : RepositoryBase<IconSet>, IIconSetRepository
    {
        #region Private Members

        private readonly IRepository<MapIcon> _mapIconRepository;

        #endregion

        #region Constructors

        public IconSetRepository(ISession session, IContainer container, IRepository<MapIcon> mapIconRepository) : base(
            session, container)
        {
            _mapIconRepository = mapIconRepository;
        }

        #endregion

        #region Exposed Methods

        public override IconSet Find(int id)
        {
            if (id > 0)
            {
                return base.Find(id);
            }

            return this.GetDefaultIconSet(_mapIconRepository);
        }

        public IconSet Find(IconSets iconSet)
        {
            return Find((int)iconSet);
        }

        #endregion
    }
}
