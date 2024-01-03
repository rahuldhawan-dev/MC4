using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IUserTypeRepository : IRepository<UserType>
    {
        UserType GetInternalUserType();
    }

    public class UserTypeRepository : RepositoryBase<UserType>, IUserTypeRepository
    {
        #region Consts

        public struct KnownUserTypeDescriptions
        {
            public const string INTERNAL = "Internal",
                                INTERNAL_CONTRACTOR = "Internal Contractor",
                                EXTERNAL_CONTRACTOR = "External Contractor";
        }

        #endregion

        #region Constructor

        public UserTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public UserType GetInternalUserType()
        {
            return Linq.Single(x => x.Description == KnownUserTypeDescriptions.INTERNAL);
        }

        #endregion
    }
}
