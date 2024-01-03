using System.Collections.Generic;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Library.JobHelpers.FileImports;

namespace MapCallScheduler.JobHelpers.W1V.ImportTasks
{
    public class CustomerMaterialTask
        : FileImportTaskBase<
                IW1VFileDownloadService,
                IW1VFileParser,
                W1VFileParser.ParsedCustomerMaterial,
                ShortCycleCustomerMaterial,
                IShortCycleCustomerMaterialRepository>,
            IDailyW1VFileImportTask
    {
        #region Fields

        private readonly IW1VRecordMapper _mapper;
        
        #endregion
        
        #region Properties

        protected override string FileType { get; }

        #endregion

        #region Constructors

        public CustomerMaterialTask(
            IW1VFileDownloadService downloadService,
            IW1VFileParser parser,
            IShortCycleCustomerMaterialRepository repository,
            IW1VRecordMapper mapper,
            ILog log)
            : base(
                downloadService,
                parser,
                repository,
                log)
        {
            _mapper = mapper;
        }

        #endregion

        #region Private Methods

        protected override IEnumerable<FileData> DownloadFiles()
        {
            return _downloadService.GetAllFiles();
        }

        protected override IEnumerable<W1VFileParser.ParsedCustomerMaterial> ParseRecords(string csv)
        {
            return _parser.ParseCustomerMaterial(csv);
        }

        protected override ShortCycleCustomerMaterial Find(W1VFileParser.ParsedCustomerMaterial update)
        {
            return _repository.FindByWorkOrderNumber(update.WorkOrderNumber);
        }

        protected override void MapFields(
            ShortCycleCustomerMaterial entity,
            W1VFileParser.ParsedCustomerMaterial update)
        {
            _mapper.Map(entity, update);
            if (entity.Premise != null && entity.Premise.MostRecentService != null)
            {
                entity.Premise.MostRecentService.Service.NeedsToSync = true;
            }
        }

        #endregion
    }
}
