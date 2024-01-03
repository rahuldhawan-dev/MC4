using System.Collections.Generic;
using System.Linq;
using Historian.Data.Client.Entities;
using MMSINC.ClassExtensions.DateTimeExtensions;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks
{
    public class TankLevelTask : SpaceTimeInsightDailyScadaFileDumpTaskBase
    {
        #region Constants

        public const string TAG_NAME = "AMW_DW_CN_SE_MG_UB_UBS.Level";
        public const int EXPECTED_COUNT = 1440;

        #endregion

        #region Private Members

        private int? _expectedCount;

        #endregion

        #region Properties

        public int? ExpectedCount
        {
            get => _expectedCount ?? (_expectedCount = EXPECTED_COUNT);
            set => _expectedCount = value;
        }

        #endregion

        #region Constructors

        public TankLevelTask(SpaceTimeInsightDailyScadaFileDumpTaskBaseArgs args) : base(args) { }

        #endregion

        #region Exposed Methods

        protected override bool HasDuplicatesOrGaps(IEnumerable<RawData> entities)
        {
            return entities.GroupBy(x => x.TimeStamp).Count() != ExpectedCount;
        }

        protected override IQueryable<RawData> GetEntities()
        {
            var yesterday = _dateTimeProvider.GetCurrentDate().AddDays(-1).Date;
            return _repository.FindByTagName(TAG_NAME, false, yesterday, yesterday.EndOfDay()).AsQueryable();
        }

        protected override string SerializeEntities(IQueryable<RawData> entities)
        {
            return _serializer.SerializeTankLevelData(entities);
        }

        protected override void UploadFile(string fileContents)
        {
            _uploadService.UploadTankLevelData(fileContents);
        }

        #endregion
    }
}