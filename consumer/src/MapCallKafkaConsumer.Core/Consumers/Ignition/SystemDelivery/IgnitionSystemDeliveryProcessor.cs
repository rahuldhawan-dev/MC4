using System;
using System.Collections.Generic;
using System.Globalization;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallKafkaConsumer.Library;
using MMSINC.Data;

namespace MapCallKafkaConsumer.Consumers.Ignition.SystemDelivery
{
    public class IgnitionSystemDeliveryProcessor :
        MessageToEntityProcessorBase<Model.SystemDelivery, SystemDeliveryIgnitionEntry>,
        IIgnitionSystemDeliveryProcessor
    {
        #region Constants

        private const int MGD_TO_GPD = 1000000;

        #endregion

        #region Private Members

        private List<SystemDeliveryIgnitionEntry> _systemDeliveryIgnitionEntries;
        private decimal _entryValue;

        #endregion

        #region Constructors

        public IgnitionSystemDeliveryProcessor(ILog logger, IUnitOfWorkFactory unitOfWorkFactory) : base(logger,
            unitOfWorkFactory) { }

        #endregion

        #region Exposed Methods

        public override void Process(string message)
        {
            try
            {
                _logger.Info($"Processing message: {message}");

                var hydratedMessage = HydrateMessage(message);

                using (var unitOfWork = _unitOfWorkFactory.Build())
                {
                    _systemDeliveryIgnitionEntries = new List<SystemDeliveryIgnitionEntry>();

                    foreach (var entry in hydratedMessage.SystemDeliveryEntry.FacilityEntries)
                    {
                        decimal.TryParse(entry.EntryValue, NumberStyles.Any, null, out _entryValue);

                        _systemDeliveryIgnitionEntries.Add(new SystemDeliveryIgnitionEntry {
                            FacilityId = entry.FacilityId ?? 0,
                            UnitOfMeasure = entry.UnitOfMeasure,
                            SystemDeliveryType = entry.SystemDeliveryType,
                            SystemDeliveryEntryType = entry.SystemDeliveryEntryType,
                            EntryDate = DateTimeOffset.FromUnixTimeSeconds(entry.EntryDate).Date,
                            FacilityName = entry.FacilityName,
                            EntryValue = _entryValue * MGD_TO_GPD
                        });
                    }

                    unitOfWork.GetRepository<SystemDeliveryIgnitionEntry>().Save(_systemDeliveryIgnitionEntries);
                    unitOfWork.Commit();
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"{GetType().Name} could not process message: {message}", ex);
                throw;
            }
        }

        public override SystemDeliveryIgnitionEntry RetrieveEntity(IUnitOfWork unitOfWork, Model.SystemDelivery message)
        {
            // This will never be called, but better than throwing a new NotImplementedException() I guess
            return new SystemDeliveryIgnitionEntry();
        }

        public override SystemDeliveryIgnitionEntry MapMessageToEntity(Model.SystemDelivery message,
            SystemDeliveryIgnitionEntry entity)
        {
            // This will never be called, but better than throwing a new NotImplementedException() I guess
            return new SystemDeliveryIgnitionEntry();
        }

        #endregion
    }
}
