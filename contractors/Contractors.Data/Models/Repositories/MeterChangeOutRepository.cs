using System.Collections.Generic;
using System.Linq;

using Contractors.Data.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using StructureMap;

namespace Contractors.Data.Models.Repositories
{
    public class MeterChangeOutRepository : SecuredRepositoryBase<MeterChangeOut, ContractorUser>, IMeterChangeOutRepository
    {
        #region Fields

        private readonly IMeterChangeOutStatusRepository
            _meterChangeOutStatusRepository;

        #endregion

        #region Properties

        #region Base Linq/Criteria

        public override IQueryable<MeterChangeOut> Linq
        {
            get { return base.Linq.Where(wo => wo.Contract.Contractor == CurrentUser.Contractor); }
        }

        public override ICriteria Criteria
        {
            get
            {
                return base.Criteria.CreateAlias("Contract", "mcoc").Add(Restrictions.Eq("mcoc.Contractor", CurrentUser.Contractor));
            }
        }

        #endregion

        #endregion

        #region Constructor

        public MeterChangeOutRepository(ISession session,
            IAuthenticationService<ContractorUser> authenticationService,
            IContainer container,
            IMeterChangeOutStatusRepository meterChangeOutStatusRepository) : base(session,
            authenticationService, container)
        {
            _meterChangeOutStatusRepository = meterChangeOutStatusRepository;
        }

        #endregion

        #region Public Methods

        public bool IsNewSerialNumberUnique(int meterChangeOutId, string newSerialNumber)
        {
            // NOTE: This only has to check against NewSerialNumber. RemovedSerialNumber isn't important.
            // DOUBLE NOTE: This method is generally being called because the new serial number has been set 
            //              or changed, so that's why we aren't just checking it directly on the meterChangeOutId's record itself.
            newSerialNumber = (newSerialNumber ?? string.Empty).Trim();

            // NOTE: This needs to bypass the contractor filter. 
            return !base.Linq.Any(x => x.Id != meterChangeOutId && x.NewSerialNumber == newSerialNumber);
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

        public IEnumerable<MeterChangeOutCompletionReportItem> GetCompletionsReport(ISearchMeterChangeOutCompletions search)
        {
            // NOTE: DO NOT JUST COPY/PASTE THIS REPORT FROM MVC. THIS HAS ADDITIONAL FILTERING FOR THE CURRENT USER CONTRACTOR!
            MeterChangeOut meterChangeOut = null;
            MeterChangeOutContract contract = null;
            MeterChangeOutCompletionReportItem result = null;
            var query = Session.QueryOver(() => meterChangeOut);
            query.JoinAlias(x => x.Contract, () => contract, NHibernate.SqlCommand.JoinType.InnerJoin);
            query.SelectList(x => x
                .SelectGroup(y => y.CalledInByContractorMeterCrew).WithAlias(() => result.ContractorMeterCrew)
                //.SelectGroup(y => y.DateStatusChanged.Value.ToShortDateString()).WithAlias(() => result.CompletionDate)
                .Select(Projections.GroupProperty(Projections.SqlFunction("date", NHibernateUtil.Date, Projections.Property(() => meterChangeOut.DateStatusChanged)))).WithAlias(() => result.CompletionDate)
                .SelectCount(y => y.CalledInByContractorMeterCrew).WithAlias(() => result.Changed)
            );
            query.Where(x => x.MeterChangeOutStatus.Id == MeterChangeOutStatus.Indices.CHANGED);
            query.Where(() => contract.Contractor == CurrentUser.Contractor);
            query.TransformUsing(Transformers.AliasToBean<MeterChangeOutCompletionReportItem>());
            return Search(search, query);
        }

        #endregion
    }
}
