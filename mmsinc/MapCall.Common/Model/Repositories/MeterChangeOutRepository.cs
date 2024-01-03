using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IMeterChangeOutRepository : IRepository<MeterChangeOut>
    {
        bool IsNewSerialNumberUnique(int meterChangeOutId, string newSerialNumber);
        IEnumerable<MeterChangeOut> GetScheduledReport();
        IEnumerable<MeterChangeOutCompletionReportItem> GetCompletionsReport(ISearchMeterChangeOutCompletions search);
        IEnumerable<MeterChangeOut> GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber();
    }

    public class MeterChangeOutRepository : RepositoryBase<MeterChangeOut>, IMeterChangeOutRepository
    {
        #region Constructor

        public MeterChangeOutRepository(ISession session, IContainer container,
            IMeterChangeOutStatusRepository meterChangeOutStatusRepository) : base(session, container)
        {
            _meterChangeOutStatusRepository = meterChangeOutStatusRepository;
        }

        #endregion

        #region Fields

        private readonly IMeterChangeOutStatusRepository _meterChangeOutStatusRepository;

        #endregion

        #region Public Methods

        public bool IsNewSerialNumberUnique(int meterChangeOutId, string newSerialNumber)
        {
            // NOTE: This only has to check against NewSerialNumber. RemovedSerialNumber isn't important.
            // DOUBLE NOTE: This method is generally being called because the new serial number has been set 
            //              or changed, so that's why we aren't just checking it directly on the meterChangeOutId's record itself.
            newSerialNumber = (newSerialNumber ?? string.Empty).Trim();

            return !Linq.Any(x => x.Id != meterChangeOutId && x.NewSerialNumber == newSerialNumber);
        }

        public IEnumerable<MeterChangeOut> GetScheduledReport()
        {
            var scheduledStatus = _meterChangeOutStatusRepository.GetScheduledStatus();
            return Linq.Where(x => x.MeterChangeOutStatus == scheduledStatus && x.DateScheduled != null)
                        // NOTE: DateScheduled must use the .Date part so it can ignore the time the field was set at. The time is ignored when it comes to
                        // listing the scheduled stuff since they use the ScheduledTime property instead for whatever reason.
                       .OrderBy(x => x.AssignedContractorMeterCrew.Description)
                       .ThenBy(x => x.DateScheduled.Value.Date)
                       .ThenBy(x => x.MeterScheduleTime.Description)
                       .ThenBy(x => x.ServiceCity != null ? x.ServiceCity.ToString() : string.Empty)
                       .ThenBy(x => x.ServiceStreetAddressCombined)
                       .ToList();
        }

        public IEnumerable<MeterChangeOutCompletionReportItem> GetCompletionsReport(
            ISearchMeterChangeOutCompletions search)
        {
            MeterChangeOut meterChangeOut = null;
            MeterChangeOutCompletionReportItem result = null;
            var query = Session.QueryOver(() => meterChangeOut);
            query.SelectList(x => x
                                 .SelectGroup(y => y.CalledInByContractorMeterCrew)
                                 .WithAlias(() => result.ContractorMeterCrew)
                                  //.SelectGroup(y => y.DateStatusChanged.Value.ToShortDateString()).WithAlias(() => result.CompletionDate)
                                 .Select(Projections.GroupProperty(Projections.SqlFunction("date", NHibernateUtil.Date,
                                      Projections.Property(() => meterChangeOut.DateStatusChanged))))
                                 .WithAlias(() => result.CompletionDate)
                                 .SelectCount(y => y.CalledInByContractorMeterCrew).WithAlias(() => result.Changed)
            );
            query.Where(x => x.MeterChangeOutStatus.Id == MeterChangeOutStatus.Indices.CHANGED);
            query.TransformUsing(Transformers.AliasToBean<MeterChangeOutCompletionReportItem>());
            return Search(search, query);
        }

        public IEnumerable<MeterChangeOut> GetActiveMeterChangeOutsWithOutOfDateNewSerialNumber()
        {
            // MC-815 states that the following is what counts as an active meter change out that
            // is out of sync with the premise records.
            // 1. The Meter Change Out Contract must still be active
            // 2. The Meter Change Out Status must not be Already Changed/AW To Complete/Changed
            // 3. The Meter Change Out NewSerialNumber value must not be null
            // 4. The Meter Change Out must match a Premise based on the premise number
            // 5. That premise must only have a water or fire service type
            // 6. That premise must have a meter size that is <= 2
            // 7. The Meter Change Out's NewSerialNumber value must not equal the Premise's MeterSerialNumber 
            //    but only if the MeterSerialNumber value is not null or empty.
            var statusIdsToIgnore = new[] {
                MeterChangeOutStatus.Indices.ALREADY_CHANGED,
                MeterChangeOutStatus.Indices.AW_TO_COMPLETE,
                MeterChangeOutStatus.Indices.CHANGED
            };

            var serviceUtilityTypesToMatch = new[] {
                ServiceUtilityType.Indices.BULK_WATER,
                ServiceUtilityType.Indices.BULK_WATER_MASTER,
                ServiceUtilityType.Indices.DOMESTIC_WATER,
                ServiceUtilityType.Indices.PRIVATE_FIRE_SERVICE,
                ServiceUtilityType.Indices.PUBLIC_FIRE_SERVICE,
            };

            return (from mco in Linq
                    from p in Session.Query<Premise>()
                    where mco.Contract.IsActive
                    // NOTE: NewSerialNumber is occasionally an empty string for some reason
                    where mco.NewSerialNumber != null && mco.NewSerialNumber != string.Empty
                    where !statusIdsToIgnore.Contains(mco.MeterChangeOutStatus.Id)
                    where mco.PremiseNumber == p.PremiseNumber
                    where serviceUtilityTypesToMatch.Contains(p.ServiceUtilityType.Id)
                    // NOTE: The Premise.MeterSerialNumber column is filled with empty string values.
                    where p.MeterSerialNumber != null && p.MeterSerialNumber != string.Empty &&
                          p.MeterSerialNumber != mco.NewSerialNumber
                    where p.MeterSize.Size <= (decimal)2.0
                    select mco);
        }

        #endregion
    }
}
