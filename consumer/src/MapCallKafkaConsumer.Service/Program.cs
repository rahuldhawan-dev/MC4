using log4net;
using StructureMap;
using System;
using System.ServiceProcess;
using System.Threading;

namespace MapCallKafkaConsumer.Service
{
    static class Program
    {
        static void Main()
        {
            IContainer container;
            ILog logger;

            try
            {
                container = DependencyRegistry.Initialize();
                logger = container.GetInstance<ILog>();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(typeof(Program)).Error("Configuration Error", ex);
                throw;
            }

            MapCallKafkaConsumer consumerService;

            try
            {
                consumerService = container.GetInstance<MapCallKafkaConsumer>();
            }
            catch (Exception e)
            {
                logger.Error("Error Initializing Service", e);
                throw;
            }

            if (Environment.UserInteractive)
            {
                Console.WriteLine("MapCallKafkaConsumer Host Service (User Interactive): Started");

                consumerService.StartFromConsole();
                Console.WriteLine("Press [enter] to quit.");
                Console.ReadLine();
                consumerService.StopFromConsole();
            }
            else
            {
                ServiceBase.Run(new ServiceBase[] { consumerService });
            }
        }
    }
}
