using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class ModuleRepository : RepositoryBase<Module>, IModuleRepository
    {
        #region Constructors

        public ModuleRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Query Parts

        public virtual AbstractCriterion GetNameMatchesCriterion(string name)
        {
            return Restrictions.Eq("Name", name);
        }

        public virtual AbstractCriterion GetApplicationNameMatchesCriterion(string applicationName)
        {
            return Restrictions.Eq("Name", applicationName);
        }

        #endregion

        #region Exposed Methods

        public Module FindByApplicationAndModuleName(string applicationName, string moduleName)
        {
            return Criteria
                  .Add(GetNameMatchesCriterion(moduleName))
                  .CreateCriteria("Application")
                  .Add(GetApplicationNameMatchesCriterion(applicationName))
                  .UniqueResult<Module>();
        }

        #endregion
    }

    public interface IModuleRepository : IRepository<Module>
    {
        #region Methods

        Module FindByApplicationAndModuleName(string applicationName,
            string moduleName);

        #endregion
    }
}
