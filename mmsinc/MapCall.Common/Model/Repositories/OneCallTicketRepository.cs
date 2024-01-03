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
    public interface IOneCallTicketRepository : IRepository<OneCallTicket>
    {
        OneCallTicket FindByRequestNumber(string requestNumber);
    }

    public class OneCallTicketRepository : RepositoryBase<OneCallTicket>, IOneCallTicketRepository
    {
        #region Constructor

        public OneCallTicketRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public override void Delete(OneCallTicket entity)
        {
            throw new NotSupportedException("OneCallTickets are not allowed to be deleted.");
        }

        public override OneCallTicket Save(OneCallTicket entity)
        {
            throw new NotSupportedException("OneCallTickets can not be created.");
        }

        public override void Save(IEnumerable<OneCallTicket> entities)
        {
            throw new NotSupportedException("OneCallTickets can not be created.");
        }

        public OneCallTicket FindByRequestNumber(string requestNumber)
        {
            // RequestNumber is not a unique field. It's almost unique, but there
            // are redundant rows for whatever reason. If you need a specific row
            // for a request number, gl, wacoy. -Ross 3/25/2014
            return Linq.FirstOrDefault(x => x.RequestNumber == requestNumber);
        }

        #endregion
    }
}
