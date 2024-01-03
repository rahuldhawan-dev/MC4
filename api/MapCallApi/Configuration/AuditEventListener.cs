using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Metadata;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Event;
using StructureMap;

namespace MapCallApi.Configuration
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
                    return _container.GetInstance<IAuthenticationService<User>>().CurrentUser;
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
            entry.Timestamp = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            session.Save(entry);
        }

        ///// <summary>
        ///// Save a collection of AuditLogEntries using a stateless transaction
        ///// </summary>
        ///// <param name="event"></param>
        ///// <param name="entries"></param>
        //protected void Save(IEventSource eventSource, IList<AuditLogEntry> entries)
        //{
        //    // this line is important, without it it takes much longer
        //    // why?
        //    var connection = eventSource.ConnectionManager.GetConnection();
        //    var now = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
        //    using (var session = eventSource.SessionFactory.OpenStatelessSession(connection))
        //    {
        //        using (var tx = session.BeginTransaction())
        //        {
        //            foreach (var entry in entries)
        //            {
        //                entry.User = CurrentUser;
        //                entry.Timestamp = now;
        //                session.Insert(entry);
        //            }
        //            tx.Commit();
        //        }
        //    }
        //}

        #endregion
    }
}
