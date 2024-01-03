using System;
using System.Diagnostics;
using log4net;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.Jobs;
using MMSINC.Utilities;
using StructureMap;

namespace MapCallScheduler.STIGenerator
{
    public class Runner
    {
        private readonly IContainer _container;
        private readonly Stopwatch _stopwatch;
        private readonly ILog _log;

        public Runner(IContainer container, Stopwatch stopwatch, ILog log)
        {
            _container = container;
            _stopwatch = stopwatch;
            _log = log;
        }

        public void OverrideTypeRegistrations()
        {
        }

        public void Run()
        {
            // 03/11/2013 - 11/04/2012
            // 03/10/2013 - 11/03/2013
            // 03/09/2014 - 11/02/2014
            // 03/08/2015 - 11/01/2015
            // 03/06/2016 - 11/06/2016
            var beginning = new DateTime(2014, 8, 22, 0, 0, 0);
            var now = beginning;
//            var now = new DateTime(2016, 11, 7, 9, 0, 0);
            _stopwatch.Start();
            for (var date = beginning; date <= now; date = date.AddDays(1))
            {
                _container.Inject<IDateTimeProvider>(new StaticDateTimeProvider(date));
                _container.Inject<ISpaceTimeInsightFileUploadService>(
                    _container.GetInstance<DummySpaceTimeInsightFileUploadService>());
                _container.Inject<ISpaceTimeInsightFileDumpTaskService>(_container.GetInstance<DummySpaceTimeInsightFileDumpTaskService>());
                LogStuff(date, beginning, now);

                foreach (var job in
                    new[] {typeof(DailySpaceTimeInsightFileDumpJob)}) //, typeof(MonthlySpaceTimeInsightFileDumpJob)})
                {
                    var jaerb = (MapCallJobBase)_container.GetInstance(job);
                    jaerb.Execute(null);
                }
            }

            _stopwatch.Stop();
            _log.Info($"Finished! Elapsed time {_stopwatch.Elapsed}");
        }

        private void LogStuff(DateTime date, DateTime beginning, DateTime now)
        {
            var soFar = date == beginning ? 1 : (date - beginning).Days;
            var remaining = (now - date).Days;
            var avgMs = _stopwatch.ElapsedMilliseconds/soFar;
            var eta = DateTime.Now.AddMilliseconds(avgMs*remaining);
            _log.Info($"Pretending today is {date:yyyy-MM-dd}");
            _log.Info($"{soFar} days processed so far, {remaining} days remaining.");
            _log.Info($"Averaging {avgMs} ms so far, ETA is {eta:yy-MMM-dd HH:mm:ss}");
        }
    }
}