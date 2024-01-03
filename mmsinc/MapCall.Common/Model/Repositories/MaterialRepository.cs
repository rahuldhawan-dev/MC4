using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using System.Collections.Generic;
using System.Linq;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class MaterialRepository : RepositoryBase<Material>, IMaterialRepository
    {
        #region Properties

        public override IQueryable<Material> Linq
        {
            get { return base.Linq.OrderBy(m => m.PartNumber); }
        }

        #endregion

        #region Constructors

        public MaterialRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IEnumerable<Material> FindByPartialPartNumberOrDescription(string partial)
        {
            return (from m in Linq
                    where m.PartNumber.Contains(partial)
                          || m.Description.Contains(partial)
                    select m).Take(10);
        }

        public IQueryable<Material> FindActiveMaterial()
        {
            return from m in Linq where m.IsActive select m;
        }

        public IQueryable<Material> FindActiveMaterialByOperatingCenter(OperatingCenter operatingCenter)
        {
            return FindActiveMaterial().Where(m => m.OperatingCenters.Contains(operatingCenter));
        }

        #endregion
    }

    public static class MaterialRepositoryExtensions
    {
        public static Material GetByPartNumber(this IRepository<Material> that, string partNumber)
        {
            return that.Where(m => m.PartNumber == partNumber).SingleOrDefault();
        }
    }

    public interface IMaterialRepository : IRepository<Material>
    {
        IEnumerable<Material> FindByPartialPartNumberOrDescription(string partial);
        IQueryable<Material> FindActiveMaterial();
        IQueryable<Material> FindActiveMaterialByOperatingCenter(OperatingCenter operatingCenter);
    }
}
