using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks
{
    public class MainBreakTask : SpaceTimeInsightDailyFileDumpTaskBase<MainBreak, IMainBreakRepository>
    {
        #region Constructors

        public MainBreakTask(IMainBreakRepository repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, ILog log) : base(repository, serializer, uploadService, log) {}

        #endregion

        #region Private Methods

        protected override void UploadFile(string fileContents)
        {
            _uploadService.UploadMainBreaks(fileContents);
        }

        protected override IQueryable<MainBreak> GetEntities()
        {
            return _repository.GetFromPastDay();
        }

        protected override string SerializeEntities(IQueryable<MainBreak> entities)
        {
            return _serializer.SerializeMainBreaks(entities);
        }

        #endregion
    }
}