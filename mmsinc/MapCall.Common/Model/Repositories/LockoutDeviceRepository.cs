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
    public class LockoutDeviceRepository : RepositoryBase<LockoutDevice>, ILockoutDeviceRepository
    {
        #region Exposed Methods

        public IQueryable<LockoutDevice> GetByUserId(int userId)
        {
            return (from l in Linq where l.Person.Id == userId select l);
        }

        #endregion

        #region Constructors

        public LockoutDeviceRepository(ISession session, IContainer container) : base(session, container) { }
        public LockoutDeviceRepository() : this(null, null) { }

        #endregion
    }

    public interface ILockoutDeviceRepository : IRepository<LockoutDevice>
    {
        IQueryable<LockoutDevice> GetByUserId(int userId);
    }
}
