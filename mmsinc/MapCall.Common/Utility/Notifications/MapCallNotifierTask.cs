using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using MapCall.Common.Utility.Scheduling;
using MMSINC.Data.NHibernate;

namespace MapCall.Common.Utility.Notifications
{
    public abstract class MapCallNotifierTask : ITask
    {
        #region Constants

        public const string BASE_URL_KEY = "BaseUrl";

        #endregion

        #region Private Members

        private readonly INotifier _notifier;
        private readonly INotificationService _notificationService;
        private readonly ILog _log;

        #endregion

        #region Properties

        protected virtual INotifier Notifier
        {
            get { return _notifier; }
        }

        protected virtual INotificationService NotificationService
        {
            get { return _notificationService; }
        }

        protected virtual ILog Log
        {
            get { return _log; }
        }

        public virtual string BaseUrl => ConfigurationManager.AppSettings[BASE_URL_KEY];

        #endregion

        #region Constructors

        protected MapCallNotifierTask(INotifier notifier, INotificationService notificationService, ILog log)
        {
            _notifier = notifier;
            _notificationService = notificationService;
            _log = log;
        }

        #endregion

        #region Abstract Methods

        public abstract void Run();

        #endregion
    }

    public abstract class MapCallNotifierTask<TRepository, TEntity> : MapCallNotifierTask
        where TRepository : IRepository<TEntity>
    {
        #region Private Members

        private readonly TRepository _repository;

        #endregion

        #region Properties

        protected virtual TRepository Repository
        {
            get { return _repository; }
        }

        #endregion

        #region Constructors

        public MapCallNotifierTask(TRepository repository, INotifier notifier, INotificationService notificationService,
            ILog log) : base(notifier, notificationService, log)
        {
            _repository = repository;
        }

        #endregion

        #region Abstract Methods

        public abstract IEnumerable<TEntity> GetData();
        public abstract void SendNotification(TEntity entity);

        #endregion

        #region Exposed Methods

        public override void Run()
        {
            Log.InfoFormat("Gathering data and sending notifications for {0} task...", GetType().Name);
            IEnumerable<TEntity> data;

            try
            {
                data = GetData();
            }
            catch (Exception e)
            {
                Log.ErrorFormat("Exception occurred gathering data for {0} task: {1}", GetType().Name, e);
                throw;
            }

            if (data == null)
            {
                var error = string.Format("GetData for task {0} returned null...", GetType().Name);
                Log.Error(error);
                throw new NullReferenceException(error);
            }

            Log.InfoFormat("Found {0} data items for {1} task...", data.Count(), GetType().Name);

            foreach (var entity in data)
            {
                Log.InfoFormat("Sending notification with {0}...", entity);

                SendNotification(entity);
            }
        }

        #endregion
    }

    public abstract class MapCallNotifierTask<TEntity> : MapCallNotifierTask<IRepository<TEntity>, TEntity>
    {
        #region Constructors

        public MapCallNotifierTask(IRepository<TEntity> repository, INotifier notifier,
            INotificationService notificationService, ILog log) :
            base(repository, notifier, notificationService, log) { }

        #endregion
    }
}
