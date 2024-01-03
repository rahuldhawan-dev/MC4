using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    // TODO: There's no reason for this sto be a secured repo base. It's just work descriptions.
    public class WorkDescriptionRepository : SecuredRepositoryBase<WorkDescription, ContractorUser>, IWorkDescriptionRepository
    {
        #region Constants

        public static readonly int[] MAIN_BREAKS = new[] {
            (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPAIR,
            (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE
        };

        #endregion

        #region Constructors

        public WorkDescriptionRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<WorkDescription> GetByAssetTypeId(int assetTypeId)
        {
            return (from wd in Linq where wd.AssetType.Id == assetTypeId select wd);
        }

        #endregion
    }
}
