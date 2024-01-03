using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;

namespace MapCall.Common.Configuration
{
    public class AssetMapResultWithCoordinates : AssetMapResult
    {
        #region Constructors

        public AssetMapResultWithCoordinates(IAuthenticationService<User> authenticationService,
            IIconSetRepository iconSetRepository, IEnumerable<IThingWithCoordinate> coordinates) : base(
            authenticationService, iconSetRepository)
        {
            CoordinateSets.Add(new MapResultCoordinateSetWithCoordinates(iconSetRepository, coordinates));
        }

        #endregion
    }
}
