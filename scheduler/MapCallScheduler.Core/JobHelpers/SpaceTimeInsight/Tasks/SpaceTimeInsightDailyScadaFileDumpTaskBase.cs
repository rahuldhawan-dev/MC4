using Historian.Data.Client.Entities;
using Historian.Data.Client.Repositories;
using log4net;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks
{
    public abstract class SpaceTimeInsightDailyScadaFileDumpTaskBase : SpaceTimeInsightDailyFileDumpTaskBase<RawData, IRawDataRepository>
    {
        #region Private Members

        protected IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public SpaceTimeInsightDailyScadaFileDumpTaskBase(SpaceTimeInsightDailyScadaFileDumpTaskBaseArgs args) : base(args.Repository, args.Serializer, args.UploadService, args.Log)
        {
            _dateTimeProvider = args.DateTimeProvider;
        }

        #endregion

        #region Nested Type: SpaceTimeInsightDailyScadaFileDumpTaskBaseArgs

        public class SpaceTimeInsightDailyScadaFileDumpTaskBaseArgs
        {
            #region Properties

            public IDateTimeProvider DateTimeProvider { get; }

            public ISpaceTimeInsightFileUploadService UploadService { get; }

            public ISpaceTimeInsightJsonFileSerializer Serializer { get; }

            public IRawDataRepository Repository { get; }

            public ILog Log { get; }

            #endregion

            #region Constructors

            public SpaceTimeInsightDailyScadaFileDumpTaskBaseArgs(IRawDataRepository repository, ISpaceTimeInsightJsonFileSerializer serializer, ISpaceTimeInsightFileUploadService uploadService, IDateTimeProvider dateTimeProvider, ILog log)
            {
                Repository = repository;
                Serializer = serializer;
                UploadService = uploadService;
                DateTimeProvider = dateTimeProvider;
                Log = log;
            }

            #endregion
        }

        #endregion
    }
}