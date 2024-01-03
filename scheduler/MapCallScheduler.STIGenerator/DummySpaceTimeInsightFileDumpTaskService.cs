using System;
using System.Collections.Generic;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using StructureMap;

namespace MapCallScheduler.STIGenerator
{
    public class DummySpaceTimeInsightFileDumpTaskService : SpaceTimeInsightFileDumpTaskService
    {
        public DummySpaceTimeInsightFileDumpTaskService(IContainer container) : base(container) {}

        protected override IEnumerable<Type> GetAllDailyTypes()
        {
            return new[] {typeof(InterconnectTask), typeof(TankLevelTask)};
        }
    }
}