using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TrainingContactHoursProgramCoordinatorRepository :
        RepositoryBase<TrainingContactHoursProgramCoordinator>, ITrainingContactHoursProgramCoordinatorRepository
    {
        public TrainingContactHoursProgramCoordinatorRepository(ISession session, IContainer container) : base(session,
            container) { }

        public IEnumerable<TrainingContactHoursProgramCoordinator> GetByOperatingCenterId(int id)
        {
            return (from t in Linq where t.ProgramCoordinator.OperatingCenter.Id == id select t);
        }
    }

    public interface ITrainingContactHoursProgramCoordinatorRepository :
        IRepository<TrainingContactHoursProgramCoordinator>
    {
        IEnumerable<TrainingContactHoursProgramCoordinator> GetByOperatingCenterId(int id);
    }
}
