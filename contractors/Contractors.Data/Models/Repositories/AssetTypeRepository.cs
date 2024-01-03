using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class AssetTypeRepository : SecuredRepositoryBase<AssetType, ContractorUser>, IAssetTypeRepository
    {
        #region Constructors

        public AssetTypeRepository(ISession session, IAuthenticationService<ContractorUser> authenticationService, IContainer container) : base(session, authenticationService, container) { }

        #endregion

        #region Properties

        public override IQueryable<AssetType> Linq
        {
            get
            {
                return (from t in base.Linq
                        where t.OperatingCenterAssetTypes.Any(o => CurrentUser.Contractor.OperatingCenters.Contains(o.OperatingCenter))
                        select t);
            }
        }

        // TODO: This query is inefficient
        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria
                    .CreateAlias("OperatingCenterAssetTypes", "ocat")
                    .Add(Subqueries.Exists(
                        DetachedCriteria.For<OperatingCenter>()
                            .SetProjection(Projections.Id())
                            .CreateAlias("Contractors", "c")
                            .Add(Restrictions.EqProperty("Id", "ocat.OperatingCenter.Id"))
                            .Add(Restrictions.Eq("c.Id", CurrentUser.Contractor.Id))));
            }
        }

        #endregion
    }
}
