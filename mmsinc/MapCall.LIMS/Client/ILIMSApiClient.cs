using MapCall.LIMS.Model.Entities;
using System.Collections.Generic;

namespace MapCall.LIMS.Client
{
    public interface ILIMSApiClient
    {
        Profile[] GetProfiles();

        Location CreateLocation(Location location);

        Location UpdateLocation(Location location);
    }
}