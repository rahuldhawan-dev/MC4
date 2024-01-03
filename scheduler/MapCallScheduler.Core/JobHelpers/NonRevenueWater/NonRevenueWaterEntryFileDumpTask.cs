using System.Linq;
using log4net;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallScheduler.Library.JobHelpers.FileDumps;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.NonRevenueWater
{
    public class NonRevenueWaterEntryFileDumpTask :
        FileDumpTaskBase<INonRevenueWaterEntryFileSerializer, INonRevenueWaterEntryFileUploader,
            NonRevenueWaterEntryFileDumpViewModel, INonRevenueWaterEntryRepository>, INonRevenueWaterEntryFileDumpTask
    {
        #region Private Members

        private readonly IDateTimeProvider _dateTimeProvider;

        #endregion

        #region Constructors

        public NonRevenueWaterEntryFileDumpTask(INonRevenueWaterEntryRepository repository,
            INonRevenueWaterEntryFileSerializer serializer, INonRevenueWaterEntryFileUploader uploadService,
            IDateTimeProvider dateTimeProvider, ILog log) : base(repository, serializer, uploadService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Protected Methods

        protected override IQueryable<NonRevenueWaterEntryFileDumpViewModel> GetEntities()
        {
            var fromTheBeginningOfThePreviousMonth =
                _dateTimeProvider.GetCurrentDate().AddMonths(-1).GetBeginningOfMonth();

            return _repository.GetDataForNonRevenueWaterEntryFileDump(fromTheBeginningOfThePreviousMonth);
        }

        protected override string SerializeEntities(IQueryable<NonRevenueWaterEntryFileDumpViewModel> entities) =>
            _serializer.Serialize(entities);

        protected override void UploadFile(string fileContents) =>
            _uploadService.UploadNonRevenueWaterEntries(fileContents);

        #endregion
    }
}
