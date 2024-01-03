using System.Collections;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;

namespace Contractors.Data.Models.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        #region Abstract Methods

        bool AnyWithInstallationNumberAndOperatingCenterAndSampleSites(string entityInstallation, int operatingCenterId);

        IEnumerable<Service> FindByStreetId(int id);

        #endregion
    }
}