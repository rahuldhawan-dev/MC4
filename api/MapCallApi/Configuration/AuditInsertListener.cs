using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Event;
using StructureMap;

namespace MapCallApi.Configuration 
{
    public class AuditInsertListener : AuditEventListener, IPostInsertEventListener
    {
        #region Exposed Methods

        public void OnPostInsert(PostInsertEvent @event)
        {
            if (((IList)IGNORED_TYPES).Contains(@event.Entity.GetType().AssemblyQualifiedName))
            {
                return;
            }
            // Hold all the entries we want to record
            var entries = new List<AuditLogEntry>();
            for (var index = 0; index < @event.Persister.PropertyNames.Length; ++index)
            {
                var value = GetStringValueFromStateArray(@event.State, index);

                // Don't insert logs for values that weren't set.
                // And don't insert logs where we're getting a type name back because
                // it doesn't tell us anything about values.
                if (value == null || value.StartsWith("NHibernate.Collection"))
                {
                    continue;
                }

                var entry = new AuditLogEntry
                {
                    EntityName = @event.Entity.GetType().Name,
                    FieldName = @event.Persister.PropertyNames[index],
                    NewValue = value,
                    AuditEntryType = "Insert",
                };

                // OneCallTicket doesn't have a primary key.
                if (@event.Id is int)
                {
                    entry.EntityId = (int)@event.Id;
                }
                entries.Add(entry);
            }
            // Save all the entries at once
            Save(@event, entries);
        }
        
        /// <summary>
        /// Save a collection of AuditLogEntries using a stateless transaction
        /// </summary>
        /// <param name="postInsertEvent"></param>
        /// <param name="entries"></param>
        protected void Save(PostInsertEvent postInsertEvent, IList<AuditLogEntry> entries)
        {
            var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            var connection = postInsertEvent.Session.ConnectionManager.GetConnection();
            using (var session = postInsertEvent.Session.SessionFactory.OpenStatelessSession(connection))
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

        public Task OnPostInsertAsync(PostInsertEvent @event, CancellationToken cancellationToken)
        {
            OnPostInsert(@event);
            return Task.FromResult<object>(null);
        }
        #endregion

        public AuditInsertListener(IContainer container) : base(container) { }
    }
}