using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class PositionGroupCommonNameRepository : RepositoryBase<PositionGroupCommonName>,
        IPositionGroupCommonNameRepository
    {
        #region Constructor

        public PositionGroupCommonNameRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<TrainingModulePositionGroupCommonNameReportItem> SearchByTrainingModule(
            ISearchTrainingModulePositionGroupCommonName search)
        {
            var query = Session.QueryOver<PositionGroupCommonName>();

            TrainingRequirement tr = null;
            query.JoinAlias(x => x.TrainingRequirements, () => tr);

            TrainingModule mod = null;
            query.JoinAlias(x => tr.TrainingModules, () => mod);

            TrainingModuleCategory tmc = null;
            query.JoinAlias(x => mod.TrainingModuleCategory, () => tmc);

            if (search.TrainingModule.HasValue)
            {
                query.Where(() => mod.Id == search.TrainingModule.Value);
            }

            if (search.IsOSHARequirement.HasValue)
            {
                query.Where(() => tr.IsOSHARequirement == search.IsOSHARequirement.Value);
            }

            TrainingModulePositionGroupCommonNameReportItem result = null;
            query.SelectList(x => x.Select(y => y.Description).WithAlias(() => result.PositionGroupCommonName)
                                   .Select(() => tmc.Description).WithAlias(() => result.ModuleCategory)
                                   .Select(() => mod.Title).WithAlias(() => result.ModuleTitle)
                                   .Select(() => tr.IsOSHARequirement).WithAlias(() => result.IsOSHARequirement));

            query.TransformUsing(Transformers.AliasToBean<TrainingModulePositionGroupCommonNameReportItem>());

            return Search(search, query);
        }

        #endregion
    }

    public interface IPositionGroupCommonNameRepository : IRepository<PositionGroupCommonName>
    {
        IEnumerable<TrainingModulePositionGroupCommonNameReportItem> SearchByTrainingModule(
            ISearchTrainingModulePositionGroupCommonName search);
    }
}
