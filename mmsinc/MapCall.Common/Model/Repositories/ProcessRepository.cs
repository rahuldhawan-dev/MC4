using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IProcessRepository : IRepository<Process>
    {
        IEnumerable<Process> GetByProcessStage(int processStageId);
    }

    public class ProcessRepository : RepositoryBase<Process>, IProcessRepository
    {
        #region Constructor

        public ProcessRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<Process> GetByProcessStage(int processStageId)
        {
            return Linq.Where(x => x.ProcessStage.Id == processStageId).ToList();
        }

        #endregion
    }
}
