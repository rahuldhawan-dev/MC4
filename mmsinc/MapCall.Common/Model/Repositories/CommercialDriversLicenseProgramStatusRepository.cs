using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface
        ICommercialDriversLicenseProgramStatusRepository : IRepository<CommercialDriversLicenseProgramStatus>
    {
        CommercialDriversLicenseProgramStatus GetNotInProgramStatus();
    }

    public class CommercialDriversLicenseProgramStatusRepository :
        RepositoryBase<CommercialDriversLicenseProgramStatus>,
        ICommercialDriversLicenseProgramStatusRepository
    {
        #region Constructor

        public CommercialDriversLicenseProgramStatusRepository(ISession session, IContainer container) : base(session,
            container) { }

        #endregion

        public CommercialDriversLicenseProgramStatus GetNotInProgramStatus()
        {
            return Linq.Single(x => x.Id == CommercialDriversLicenseProgramStatus.Indices.NOT_IN_PROGRAM);
        }
    }
}
