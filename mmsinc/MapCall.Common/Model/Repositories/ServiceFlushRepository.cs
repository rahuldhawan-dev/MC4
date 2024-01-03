using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IServiceFlushRepository : IRepository<ServiceFlush>
    {
        IEnumerable<ServiceFlush> GetServiceFlushNotReceivedAfterTwoWeeks();
    }

    public class ServiceFlushRepository : RepositoryBase<ServiceFlush>, IServiceFlushRepository 
    {
        #region Constructors

        public ServiceFlushRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public IEnumerable<ServiceFlush> GetServiceFlushNotReceivedAfterTwoWeeks()
        {
            var today = _container.GetInstance<IDateTimeProvider>().GetCurrentDate().Date;
            var twoWeeksAgo = today.AddDays(-14);

            return (from e in Linq
                    where
                        e.SampleStatus.Id != ServiceFlushSampleStatus.Indices.RESULTS_RECEIVED && e.SampleDate <= twoWeeksAgo && e.HasSentNotification == false
                    select e).ToList();
        }

        #endregion
    }
}
