using System.Collections.Generic;
using System.ServiceProcess;
using log4net;
using MapCallActiveMQListener.Library;
using MMSINC.ClassExtensions.ReflectionExtensions;
using StructureMap;
using StructureMap.TypeRules;

namespace MapCallActiveMQListener
{
    partial class MapCallActiveMQListener : ServiceBase
    {
        #region Private Members

        private IList<IListener> _listeners;
        private readonly ILog _log;
        private readonly IContainer _container;

        #endregion

        #region Constructors

        public MapCallActiveMQListener(ILog log, IContainer container)
        {
            _container = container;
            _log = log;
            SetListeners();
        }

        #endregion

        #region Private Methods

        private void SetListeners()
        {
            _listeners = new List<IListener>();
            var types = typeof(ListenerBase<>).Assembly.GetClassesByCondition(t =>
                    t.Namespace.StartsWith("MapCallActiveMQListener.Listeners") &&
                    !t.IsAbstract &&
                    !t.IsNested &&
                    !t.IsOpenGeneric() &&
                    t.IsSubclassOfRawGeneric(typeof(ListenerBase<>)));
            foreach (var type in types)
            {
                _listeners.Add((IListener)_container.GetInstance(type));
            }
        }

        protected override void OnStart(string[] args)
        {
            foreach (var listener in _listeners)
            {
                _log.Info($"Starting listener service: {listener.GetType().Name}");
                 listener.Start();
            }
        }

        protected override void OnStop()
        {
            foreach (var listener in _listeners)
            {
                _log.Info($"Stopping listener service: {listener.GetType().Name}");
                listener.Stop();
            }
        }

        #endregion

        public void Start()
        {
            OnStart(null);
        }
    }
}
