using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using NHibernate;
using NHibernate.Event;
using StructureMap;

namespace MapCallApi.Configuration {
    public class AuditDeleteListener : AuditEventListener, IPostDeleteEventListener
    {
        #region Exposed Methods

        public void OnPostDelete(PostDeleteEvent @event)
        {
            if (((IList)IGNORED_TYPES).Contains(@event.Entity.GetType().AssemblyQualifiedName))
            {
                return;
            }

            var session = @event.Session.GetSession(EntityMode.Poco);
            Save(session, new AuditLogEntry
            {
                EntityName = @event.Entity.GetType().Name,
                EntityId = @event.Id is int ? (int)@event.Id : 0,
                AuditEntryType = "Delete"
            });

            session.Flush();
            session.Clear();
        }

        public Task OnPostDeleteAsync(PostDeleteEvent @event, CancellationToken cancellationToken)
        {
            OnPostDelete(@event);
            return Task.FromResult<object>(null);
        }

        #endregion

        public AuditDeleteListener(IContainer container) : base(container) { }
    }
}