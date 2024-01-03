using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TrainingModuleRepository : RepositoryBase<TrainingModule>, ITrainingModuleRepository
    {
        #region Exposed Methods

        public IQueryable<TrainingModule> GetActiveTrainingModules()
        {
            return (from tm in Linq where tm.IsActive == true select tm);
        }

        #endregion

        #region Constructors

        public TrainingModuleRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion
    }

    public interface ITrainingModuleRepository : IRepository<TrainingModule>
    {
        IQueryable<TrainingModule> GetActiveTrainingModules();
    }
}
