using System;
using System.Collections.Generic;
using System.Linq;
using Historian.Data.Client.Entities;
using Historian.Data.Client.Repositories;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks
{
    public class InterconnectTask : SpaceTimeInsightDailyScadaFileDumpTaskBase
    {
        #region Constants

        public static string[] TAG_NAMES = {
            "AMW_DW_CN_SE_MG_UB_UBPAI.FlowDaily",
            "AMW_DW_CN_SE_MG_UB_UBR3I.FlowDaily",
            "AMW_DW_CN_SE_MG_UB_UBR3I_BP1.Flow-Total",
            "AMW_DW_CN_SE_MG_UB_UBR3I_BP2.Flow-Total"
        };

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

        public InterconnectTask(SpaceTimeInsightDailyScadaFileDumpTaskBaseArgs args) : base(args) { }

        #endregion

        #region Private methods

        protected override bool HasDuplicatesOrGaps(IEnumerable<RawData> entities)
        {
            return entities.GroupBy(x => x.TimeStamp).Count() != ExpectedCount;
        }

        protected override IQueryable<RawData> GetEntities()
        {
            var result = new List<RawData>();
            var yesterday = _dateTimeProvider.GetCurrentDate().AddDays(-1).Date;
            foreach (var tagName in TAG_NAMES)
            {
                result = (List<RawData>)result.MergeWith(_repository.FindByTagName(tagName, false, yesterday, yesterday.EndOfDay()));
            }
            return result.AsQueryable();
        }

        protected override string SerializeEntities(IQueryable<RawData> entities)
        {
            return _serializer.SerializeInterconnectData(entities);
        }

        protected override void UploadFile(string fileContents)
        {
             _uploadService.UploadInterconnectData(fileContents);
        }

        #endregion
    }
}
