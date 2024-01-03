using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class CoordinateRepository : RepositoryBase<Coordinate>, ICoordinateRepository
    {
        #region Private Members

        private readonly IRepository<IconSet> _iconSetRepository;
        private readonly IRepository<MapIcon> _mapIconRepository;

        #endregion

        #region Constructors

        public CoordinateRepository(ISession session, IContainer container, IRepository<IconSet> iconSetRepository,
            IRepository<MapIcon> mapIconRepository) : base(session, container)
        {
            _iconSetRepository = iconSetRepository;
            _mapIconRepository = mapIconRepository;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Returns a new saved Coordinate instance with the same values as the original instance. If the Icon is null,
        /// the icon is set to the default icon set's icon. 
        /// </summary>
        /// <returns></returns>
        public Coordinate CloneAndSave(Coordinate original)
        {
            var clone = new Coordinate {
                Icon = original.Icon,
                Latitude = original.Latitude,
                Longitude = original.Longitude
            };

            // CoordinateMap has Icon mapped as Not Nullable but old coordinates may
            // have null icons. Need to set to a default one if there isn't one.
            if (clone.Icon == null)
            {
                clone.Icon = _iconSetRepository.GetDefaultIconSet(_mapIconRepository).DefaultIcon;
                if (clone.Icon == null)
                {
                    // This is a sanity check because hunting around for null/transient errors is annoying.
                    throw new InvalidOperationException("The default icon is apparently null.");
                }
            }

            Save(clone);

            return clone;
        }

        #endregion
    }

    public interface ICoordinateRepository : IRepository<Coordinate>
    {
        #region Abstract Methods

        Coordinate CloneAndSave(Coordinate original);

        #endregion
    }

    public static class CoordinateRepositoryExtensions
    {
        public static Coordinate FindByValues(this IRepository<Coordinate> that, decimal latitude, decimal longitude,
            int iconId)
        {
            // Need to use FirstOrDefault because there are duplicate coordinates.
            return that.Where(c => c.Latitude == latitude && c.Longitude == longitude &&
                                   ((iconId == 0 && c.Icon == null) || iconId == c.Icon.Id)).FirstOrDefault();
        }
    }
}
