using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TailgateTalkRepository : RepositoryBase<TailgateTalk>, ITailgateTalkRepository
    {
        #region Fields

        private readonly IRepository<EmployeeLink> _employeeLinkRepository;

        #endregion

        #region Constructors

        public TailgateTalkRepository(ISession session, IContainer container,
            IRepository<EmployeeLink> employeeLinkRepository) : base(session, container)
        {
            _employeeLinkRepository = employeeLinkRepository;
        }

        #endregion

        #region Exposed Methods

        public override void Delete(TailgateTalk entity)
        {
            entity.LinkedEmployees.Each(x => {
                var empLink = Session.Load<EmployeeLink>(x.Id);
                _employeeLinkRepository.Delete(empLink);
            });

            base.Delete(entity);
        }

        #endregion
    }

    public interface ITailgateTalkRepository : IRepository<TailgateTalk> { }
}
