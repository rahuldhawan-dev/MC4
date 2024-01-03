using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class CustomerCoordinateRepository : RepositoryBase<CustomerCoordinate>, ICustomerCoordinateRepository
    {
        #region Constructors

        public CustomerCoordinateRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Exposed Methods

        public override CustomerCoordinate Save(CustomerCoordinate entity)
        {
            entity = base.Save(entity);
            // If this one is verified the others cannot be, clean them up.
            if (entity.Verified == true)
            {
                Linq.Where(x => x.CustomerLocation == entity.CustomerLocation && x.Id != entity.Id)
                    .Each(x => {
                         x.Verified = false;
                         Save(x);
                     });
            }

            return entity;
        }

        #endregion
    }

    public interface ICustomerCoordinateRepository : IRepository<CustomerCoordinate> { }
}
