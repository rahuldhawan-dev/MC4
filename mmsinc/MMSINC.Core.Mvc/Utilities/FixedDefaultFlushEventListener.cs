using System;
using System.Diagnostics.CodeAnalysis;
using NHibernate.Event;
using NHibernate.Event.Default;

namespace MMSINC.Utilities
{
    /// <summary>
    /// This is a fix for the issue described here:
    /// http://stackoverflow.com/questions/3090733/an-nhibernate-audit-trail-that-doesnt-cause-collection-was-not-processed-by-fl
    /// </summary>
    [Serializable, ExcludeFromCodeCoverage]
    public class FixedDefaultFlushEventListener : DefaultFlushEventListener
    {
        protected override void PerformExecutions(IEventSource session)
        {
            try
            {
                session.ConnectionManager.FlushBeginning();
                session.PersistenceContext.Flushing = true;
                session.ActionQueue.PrepareActions();
                session.ActionQueue.ExecuteActions();
            }
            finally
            {
                session.PersistenceContext.Flushing = false;
                session.ConnectionManager.FlushEnding();
            }
        }
    }
}
