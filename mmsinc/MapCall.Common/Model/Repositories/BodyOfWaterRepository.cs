using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class BodyOfWaterRepository : RepositoryBase<BodyOfWater>, IBodyOfWaterRepository
    {
        #region Constructors

        public BodyOfWaterRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public IQueryable<BodyOfWater> GetByOperatingCenterId(int operatingCenterId)
        {
            return (from b in Linq where b.OperatingCenter.Id == operatingCenterId select b);
        }

        #endregion
    }

    public interface IBodyOfWaterRepository : IRepository<BodyOfWater>
    {
        IQueryable<BodyOfWater> GetByOperatingCenterId(int operatingCenterId);
    }
}
