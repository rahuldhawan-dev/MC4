using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities.Pdf;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public enum MapImageDirection
    {
        North,
        South,
        East,
        West
    }

    public interface IMapImageRepository : IAssetImageRepository<MapImage>
    {
        MapImage FindImageInDirection(MapImage entity, MapImageDirection direction);
    }

    public class MapImageRepository : BaseAssetImageRepository<MapImage>, IMapImageRepository
    {
        #region Constructor

        public MapImageRepository(ISession session, IContainer container, IImageToPdfConverter imageToPdfConverter) :
            base(session, container, imageToPdfConverter) { }

        #endregion

        #region Private Methods

        protected override string RelativeFileDirectory
        {
            get { throw new NotSupportedException("Map Image are not uploaded."); }
        }

        protected override string GetActualFileName(MapImage entity)
        {
            // MapImages are dumb and special cased for how their file names are actually picked.
            if (string.IsNullOrWhiteSpace(entity.MapPage))
            {
                return entity.FileName;
            }

            // For whatever reason, this specific folder has leading zeros on to the filename so that
            // it meets the 8 character filename thing. The actual files don't have the leading zeroes
            // in the file name. The records that actually have this directory all have MapPages set
            // but they do not coorespond to the file number at all(they're off by -9 or so).
            if (entity.Directory == "NJ/00-Maps/00630001/00/")
            {
                // Removes the leading zero
                return entity.FileName.TrimStart('0');
            }

            return entity.MapPage;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns the next MapImage based on direction and districtid. This can return null.
        /// </summary>
        public MapImage FindImageInDirection(MapImage entity, MapImageDirection direction)
        {
            var nextMapPage = "";
            switch (direction)
            {
                case MapImageDirection.North:
                    nextMapPage = entity.North;
                    break;
                case MapImageDirection.South:
                    nextMapPage = entity.South;
                    break;
                case MapImageDirection.East:
                    nextMapPage = entity.East;
                    break;
                case MapImageDirection.West:
                    nextMapPage = entity.West;
                    break;
                default:
                    throw new NotSupportedException(direction.ToString());
            }

            // Don't query for these as it won't return a result
            if (string.IsNullOrWhiteSpace(nextMapPage) || nextMapPage == "NONE")
            {
                return null;
            }

            return Linq.Where(x => x.MapPage == nextMapPage && x.Town.DistrictId == entity.Town.DistrictId)
                       .OrderByDescending(x => x.Id).FirstOrDefault();
        }

        public override void Save(IEnumerable<MapImage> entities)
        {
            throw new NotSupportedException("Map images can not be created or modified.");
        }

        public override MapImage Save(MapImage entity)
        {
            throw new NotSupportedException("Map images can not be created or modified.");
        }

        public override void Delete(MapImage entity)
        {
            throw new NotSupportedException("Map images can not be deleted.");
        }

        #endregion
    }
}
