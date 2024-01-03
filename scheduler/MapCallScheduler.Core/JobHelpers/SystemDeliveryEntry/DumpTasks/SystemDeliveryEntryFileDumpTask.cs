using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallScheduler.Library.JobHelpers.FileDumps;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks
{
    public class SystemDeliveryEntryFileDumpTask :
        FileDumpTaskBase<ISystemDeliveryEntryFileSerializer, ISystemDeliveryEntryFileUploader,
            SystemDeliveryEntryFileDumpViewModel, ISystemDeliveryEntryRepository>, ISystemDeliveryEntryFileDumpTask
    {
        #region Protected members

        protected IDateTimeProvider _dateTimeProvider;
        protected int[] _includedStates = {
            State.Indices.CA,
            State.Indices.HI,
            State.Indices.IA,
            State.Indices.IL,
            State.Indices.IN,
            State.Indices.KY,
            State.Indices.MD,
            State.Indices.MO,
            State.Indices.NJ,
            State.Indices.PA,
            State.Indices.TN,
            State.Indices.VA,
            State.Indices.WV
        };

        #endregion

        #region Constructor

        public SystemDeliveryEntryFileDumpTask(ISystemDeliveryEntryRepository repository,
            ISystemDeliveryEntryFileSerializer serializer, ISystemDeliveryEntryFileUploader uploadService,
            IDateTimeProvider dateTimeProvider, ILog log) : base(repository, serializer, uploadService, log)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Protected methods

        protected override IQueryable<SystemDeliveryEntryFileDumpViewModel> GetEntities()
        {
            var month = _dateTimeProvider.GetCurrentDate().AddMonths(-1).GetBeginningOfMonth();
            // We want everything from the beginning of the previous month to the end of the previous month
            return _repository.GetDataForSystemDeliveryEntryFileDump(month, _includedStates);
        }

        protected override string SerializeEntities(IQueryable<SystemDeliveryEntryFileDumpViewModel> entities)
        {
            return _serializer.Serialize(entities);
        }

        protected override void PostProcessing(IQueryable<SystemDeliveryEntryFileDumpViewModel> entities)
        {
            var month = _dateTimeProvider.GetCurrentDate().AddMonths(-1).GetBeginningOfMonth();
            
            // entities is a roll up of many system delivery entries grouped by facility, so entities
            // does not contain all the system delivery entry Id's that need updating. But GetEntryIds does.
            var entryIds = _repository.GetEntryIds(month, _includedStates);
            
            _log.Info($"post processing {entryIds.Count()} system delivery entry records...");
            
            var count = 0;
            var errorCount = 0;
            
            foreach (var id in entryIds)
            {
                var systemDeliveryEntry = _repository.Find(id);

                if (systemDeliveryEntry == null)
                {
                    _log.Warn($"could not find System Delivery Entry {id}");
                    continue;
                }

                _log.Info($"setting entry {systemDeliveryEntry.Id} {nameof(systemDeliveryEntry.IsHyperionFileCreated)} to true");

                count++;
                try
                {
                    systemDeliveryEntry.IsHyperionFileCreated = true;
                    _repository.Save(systemDeliveryEntry);
                }
                catch (Exception ex)
                {
                    errorCount++;
                    _log.Error($"An error occurred while updating 'IsHyperionFileCreated' for System Delivery Entry {systemDeliveryEntry.Id}: {ex.Message}");
                }
            }
            
            _log.Info($"completed processing {count}/{entryIds.Count()} system delivery entry records with {errorCount} exceptions.");
        }

        protected override void UploadFile(string fileContents)
        {
            _uploadService.UploadSystemDeliveryEntries(fileContents);
        }

        #endregion
    }
}
