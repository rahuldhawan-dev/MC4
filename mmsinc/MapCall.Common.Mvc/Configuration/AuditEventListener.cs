using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Metadata;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Event;
using NHibernate.Proxy;
using StructureMap;

namespace MapCall.Common.Configuration
{
    public abstract class AuditEventListener : FixedDefaultFlushEventListener
    {
        #region Constants

        public static readonly string[] IGNORED_TYPES = {
            typeof(AuditLogEntry).AssemblyQualifiedName,
            typeof(SecureFormToken).AssemblyQualifiedName,
            typeof(SecureFormDynamicValue).AssemblyQualifiedName,
            typeof(AuthenticationLog).AssemblyQualifiedName,
            typeof(UserViewed).AssemblyQualifiedName,
            typeof(ContractorsSecureFormDynamicValue).AssemblyQualifiedName,
            typeof(ContractorsSecureFormToken).AssemblyQualifiedName,
            typeof(ContractorsAuthenticationLog).AssemblyQualifiedName,
        };

        #endregion

        #region Fields

        protected readonly IContainer _container;

        #endregion

        #region Properties

        public User CurrentUser
        {
            get
            {
                try
                {
                    // we need to get this from a container because the constructor for this
                    // is called where the NHibernate SessionFactory is being configured, so
                    // attempting to retrieve an IAuthenticationService with its repo and all
                    // at that stage would fail
                    return _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        public ContractorUser ContractorUser
        {
            get
            {
                try
                {
                    // we need to get this from a container because the constructor for this
                    // is called where the NHibernate SessionFactory is being configured, so
                    // attempting to retrieve an IAuthenticationService with its repo and all
                    // at that stage would fail
                    return _container.GetInstance<IAuthenticationService<ContractorUser>>().CurrentUser;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }

        #endregion

        #region Constructors

        public AuditEventListener(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        protected static string GetStringValueFromStateArray(IList<object> stateArray, int position)
        {
            var value = stateArray[position];

            if (value == null)
            {
                return null;
            }

            if (value as string == string.Empty)
            {
                return string.Empty;
            }

            // Calling ToString on an NHibernate proxy type, or a regular entity maybe, can potentially
            // cause seemingly unrelated areas to throw exceptions during a Flush. Ex: Changing the
            // Municipality of a Bond will throw a "collection was not processed by flush" exception
            // about Municipality.Forms. 
            if (value.GetType().IsValueType || value is string)
            {
                return value.ToString();
            }

            if (value.HasPublicProperty("Id"))
            {
                var id = value.GetPropertyValueByName("Id").ToString();
                return $"{id} - {value}";
            }

            return value.GetType().FullName;
        }

        protected void Save(ISession session, AuditLogEntry entry)
        {
            entry.User = CurrentUser;
            entry.ContractorUser = ContractorUser;
            entry.Timestamp = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            session.Save(entry);
        }

        #endregion
    }

    public class AuditUpdateListener : AuditEventListener, IPostUpdateEventListener
    {
        #region Exposed Methods

        public Task OnPostUpdateAsync(PostUpdateEvent @event, CancellationToken cancellationToken)
        {
            OnPostUpdate(@event);
            return Task.FromResult<object>(null);
        }

        public void OnPostUpdate(PostUpdateEvent @event)
        {
#if DEBUG
            if (MvcApplication.IsInTestMode && !MvcApplication.RegressionTestFlags.Contains("allow audits"))
            {
                return;
            }
#endif

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

            var session = _container.GetInstance<ISession>();

            foreach (var dirtyFieldIndex in dirtyFieldIndexes)
            {
                var oldValue = GetStringValueFromStateArray(@event.OldState, dirtyFieldIndex);
                var newValue = GetStringValueFromStateArray(@event.State, dirtyFieldIndex);

                if (oldValue == newValue)
                {
                    continue;
                }

                Save(session, new AuditLogEntry {
                    EntityName = entityName,
                    FieldName = @event.Persister.PropertyNames[dirtyFieldIndex],
                    OldValue = oldValue,
                    NewValue = newValue,
                    EntityId = (int)@event.Id,
                    AuditEntryType = "Update",
                });
            }
        }

        #endregion

        public AuditUpdateListener(IContainer container) : base(container) { }
    }

    public class AuditInsertListener : AuditEventListener, IPostInsertEventListener
    {
        #region Exposed Methods

        public void OnPostInsert(PostInsertEvent @event)
        {
#if DEBUG
            if (MvcApplication.IsInTestMode && !MvcApplication.RegressionTestFlags.Contains("allow audits"))
            {
                return;
            }
#endif

            if (IGNORED_TYPES.Contains(@event.Entity.GetType().AssemblyQualifiedName))
            {
                return;
            }

            var session = _container.GetInstance<ISession>();

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

                var entry = new AuditLogEntry {
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

                Save(session, entry);
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

    public class AuditDeleteListener : AuditEventListener, IPostDeleteEventListener
    {
        #region Exposed Methods

        public void OnPostDelete(PostDeleteEvent @event)
        {
#if DEBUG
            if (MvcApplication.IsInTestMode && !MvcApplication.RegressionTestFlags.Contains("allow audits"))
            {
                return;
            }
#endif

            if (IGNORED_TYPES.Contains(@event.Entity.GetType().AssemblyQualifiedName))
            {
                return;
            }

            var session = _container.GetInstance<ISession>();
            Save(session, new AuditLogEntry {
                EntityName = @event.Entity.GetType().Name,
                EntityId = @event.Id is int ? (int)@event.Id : 0,
                AuditEntryType = "Delete"
            });
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