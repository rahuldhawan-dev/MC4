using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface
        IMainCrossingInspectionAssessmentRatingRepository : IRepository<MainCrossingInspectionAssessmentRating> { }

    public class MainCrossingInspectionAssessmentRatingRepository :
        RepositoryBase<MainCrossingInspectionAssessmentRating>, IMainCrossingInspectionAssessmentRatingRepository
    {
        #region Properties

        public override IQueryable<MainCrossingInspectionAssessmentRating> Linq
        {
            get
            {
                // SQL Server 2012(and perhaps others) will automatically return rows
                // sorted by Description due to the unique index on it. This ends up
                // being the wrong display order.
                return base.Linq.OrderBy(x => x.Id);
            }
        }

        #endregion

        #region Constructor

        public MainCrossingInspectionAssessmentRatingRepository(ISession session, IContainer container) : base(session,
            container) { }

        #endregion
    }
}
