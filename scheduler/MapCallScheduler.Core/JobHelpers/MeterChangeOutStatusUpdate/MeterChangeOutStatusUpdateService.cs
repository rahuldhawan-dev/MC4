using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;

namespace MapCallScheduler.JobHelpers.MeterChangeOutStatusUpdate
{
    public class MeterChangeOutStatusUpdateService : IMeterChangeOutStatusUpdateService
    {
        #region Private methods

        protected readonly ILog _log;
        private IMeterChangeOutRepository _meterChangeOutRepo;
        private IRepository<MeterChangeOutStatus> _meterChangeOutStatusRepo;
        private IDataTypeRepository _dataTypeRepo;
        private IDateTimeProvider _dateTimeProvider;
        private IRepository<Note> _noteRepo;

        #endregion

        #region Constructors

        public MeterChangeOutStatusUpdateService(ILog log, 
            IMeterChangeOutRepository meterChangeOutRepo,
            IRepository<MeterChangeOutStatus> meterChangeOutStatusRepo,
            IDataTypeRepository dataTypeRepo, 
            IRepository<Note> noteRepo, 
            IDateTimeProvider dateTimeProvider)
        {
            _log = log;
            _meterChangeOutRepo = meterChangeOutRepo;
            _meterChangeOutStatusRepo = meterChangeOutStatusRepo;
            _dataTypeRepo = dataTypeRepo;
            _noteRepo = noteRepo;
            _dateTimeProvider = dateTimeProvider;
        }

        #endregion

        #region Private Methods

        private MeterChangeOutStatus GetAlreadyChangedStatus()
        {
            var status = _meterChangeOutStatusRepo.Find(MeterChangeOutStatus.Indices.ALREADY_CHANGED);
            if (status == null)
            {
                throw new InvalidOperationException(
                    $"Unable to find the 'Already Changed'(Id {MeterChangeOutStatus.Indices.ALREADY_CHANGED}) MeterChangeOutStatus.");
            }

            return status;
        }

        #endregion

        #region Public Methods

        public void Process()
        {
            _log.Info($"Running Meter Change Out Status Update service.");

            var meterChangeOutsToUpdate = _meterChangeOutRepo.GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber().ToList();
            _log.Info($"Meter Change Out Status Update service has {meterChangeOutsToUpdate.Count} records to update.");

            var alreadyChangedStatus = GetAlreadyChangedStatus();
            var notes = new List<Note>();
            var dataType = _dataTypeRepo.GetByTableName(nameof(MeterChangeOut).Pluralize()).Single(); // There should only ever be one.
            var currentDateTime = _dateTimeProvider.GetCurrentDate();
            foreach (var mco in meterChangeOutsToUpdate)
            {
                mco.MeterChangeOutStatus = alreadyChangedStatus;
                mco.DateStatusChanged = currentDateTime;
                var note = new Note();
                note.DataType = dataType; 
                note.Text = "Already confirmed in MapCall";
                note.CreatedBy = "Scheduler - Meter Change Out Status Update";
                note.LinkedId = mco.Id;
                notes.Add(note);
            }

            // We're dealing with potentially hundreds or thousands of records.
            // Save these all at once so we're not wasting time flushing over and over in the loop.
            _meterChangeOutRepo.Save(meterChangeOutsToUpdate);
            _noteRepo.Save(notes);
            _log.Info($"Compeleted Meter Change Out Status Update service.");
        }

        #endregion
    }
}
