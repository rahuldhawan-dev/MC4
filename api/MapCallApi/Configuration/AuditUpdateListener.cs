using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Event;
using NHibernate.Proxy;
using StructureMap;

namespace MapCallApi.Configuration {
    public class AuditUpdateListener : AuditEventListener, IPostUpdateEventListener
    {
        #region Exposed Methods

        public void OnPostUpdate(PostUpdateEvent @event)
        {
            if (IGNORED_TYPES.Contains(@event.Entity.GetType().AssemblyQualifiedName))
            {
                return;
            }

            var entityFullName = @event.Entity.GetType().FullName;
            var entityName = NHibernateProxyHelper.GuessClass(@event.Entity).Name;

            if (@event.OldState == null)
            {
                throw new ArgumentNullException("No old state available for entity type '" + entityFullName +
                                                "'. Make sure you're loading it into Session before modifying and saving it.");
            }

            var dirtyFieldIndexes = @event.Persister.FindDirty(@event.State, @event.OldState, @event.Entity,
                @event.Session);

            //var session = @event.Session.GetSession(EntityMode.Poco);
            // Hold all the entries we want to record
            var entries = new List<AuditLogEntry>();

            foreach (var dirtyFieldIndex in dirtyFieldIndexes)
            {
                var oldValue = GetStringValueFromStateArray(@event.OldState, dirtyFieldIndex);
                var newValue = GetStringValueFromStateArray(@event.State, dirtyFieldIndex);

                if (oldValue == newValue)
                {
                    continue;
                }

                var entry = new AuditLogEntry {
                    EntityName = entityName,
                    FieldName = @event.Persister.PropertyNames[dirtyFieldIndex],
                    OldValue = oldValue,
                    NewValue = newValue,
                    EntityId = (int)@event.Id,
                    AuditEntryType = "Update",
                };
                entries.Add(entry);
            }
            Save(@event, entries);
        }

        protected void Save(PostUpdateEvent postUpdateEvent, IList<AuditLogEntry> entries)
        {
            var connection = postUpdateEvent.Session.ConnectionManager.GetConnection();
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            using (var session = postUpdateEvent.Session.SessionFactory.OpenStatelessSession(connection))
            {
                using (var tx = session.BeginTransaction())
                {
                    foreach (var entry in entries)
                    {
                        entry.User = CurrentUser;
                        entry.Timestamp = now;
                        session.Insert(entry);
                    }
                    tx.Commit();
                }
            }
        }

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            OnPostUpdate(@event);
            return Task.FromResult<object>(null);
        }

        #endregion

        public AuditUpdateListener(IContainer container) : base(container) { }
    }
}