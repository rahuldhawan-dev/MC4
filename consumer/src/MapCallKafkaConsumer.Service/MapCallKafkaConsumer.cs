using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using log4net;
using MapCallKafkaConsumer.Library;
using MMSINC.ClassExtensions.ReflectionExtensions;
using StructureMap;
using StructureMap.TypeRules;

namespace MapCallKafkaConsumer.Service
{
    public partial class MapCallKafkaConsumer : ServiceBase
    {
        #region Private Members

        private readonly ILog _logger;
        private readonly IContainer _container;
        private readonly IList<Task> _consumerTasks = new List<Task>();

        private IList<IConsumer> _consumers;

        #endregion

        #region Constructors

        public MapCallKafkaConsumer(ILog logger, IContainer container)
        {
            _container = container;
            _logger = logger;

            SetConsumers();
        }

        #endregion

        #region Private Methods

        private void SetConsumers()
        {
            _consumers = new List<IConsumer>();
            var consumers = typeof(IConsumer).Assembly
                                             .GetClassesByCondition(t =>
                                                  t.IsSubclassOfRawGeneric(typeof(ConsumerBase<,,>)) &&
                                                  !t.IsAbstract &&
                                                  !t.IsNested &&
                                                  !t.IsOpenGeneric() &&
                                                  !string.IsNullOrEmpty(t.Namespace) &&
                                                  t.Namespace.StartsWith("MapCallKafkaConsumer.Consumers"));
            foreach (var consumer in consumers)
            {
                try
                {
                    _logger.Info($"Attempting to create the follow consumer: {consumer}");
                    _consumers.Add((IConsumer)_container.GetInstance(consumer));
                }
                catch (Exception ex)
                {
                    _logger.Error("Could not create consumer.", ex);
                    throw;
                }
            }
        }

        #endregion

        #region Exposed Methods

        protected override void OnStart(string[] args)
        {
            foreach (var consumer in _consumers)
            {
                _consumerTasks.Add(Task.Factory.StartNew(() => {
                    try
                    {
                        consumer.Start();
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.Info($"{consumer.Identifier}: cancelled.");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"{consumer.Identifier}: Maybe an unhandled exception.", ex);
                        throw;
                    }
                }, TaskCreationOptions.LongRunning));
            }
        }

        protected override void OnStop()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Stop();
            }

            Task.WaitAll(_consumerTasks.ToArray());
        }

        public void StartFromConsole()
        {
            OnStart(null);
        }

        public void StopFromConsole()
        {
            OnStop();
        }

        #endregion
    }
}
