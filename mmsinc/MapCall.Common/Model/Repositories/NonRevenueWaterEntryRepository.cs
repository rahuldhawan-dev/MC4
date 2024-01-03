using System;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.ViewModels;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class NonRevenueWaterEntryRepository : MapCallSecuredRepositoryBase<NonRevenueWaterEntry>,
        INonRevenueWaterEntryRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.ProductionNonRevenueWaterUnbilledUsage;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        #endregion

        #region Constructors

        public NonRevenueWaterEntryRepository(ISession session,
            IContainer container,
            IAuthenticationService<User> authenticationService,
            IRepository<AggregateRole> roleRepo) : base(session,
            container,
            authenticationService,
            roleRepo) { }

        #endregion

        #region Exposed Methods

        public IQueryable<NonRevenueWaterEntryFileDumpViewModel> GetDataForNonRevenueWaterEntryFileDump(
            DateTime startDate)
        {
            var entries =
                Linq.Where(x => x.Month == startDate.Month && x.Year == startDate.Year).ToList();

            var result =
                entries.SelectMany(entry => entry.NonRevenueWaterDetails.Select(detail => new {
                            entry.OperatingCenter.Description,
                            entry.Month,
                            entry.Year,
                            detail.BusinessUnit,
                            detail.TotalGallons
                        }).Concat(entry.NonRevenueWaterAdjustments.Select(adjustment => new {
                            entry.OperatingCenter.Description,
                            entry.Month,
                            entry.Year,
                            adjustment.BusinessUnit,
                            adjustment.TotalGallons
                        })))
                       .GroupBy(data => new {
                            data.Description, data.Month, data.Year, data.BusinessUnit
                        })
                       .Select(group => new {
                            group.Key.Description,
                            group.Key.Month,
                            group.Key.Year,
                            group.Key.BusinessUnit,
                            TotalGallons = group.Sum(data => data.TotalGallons)
                        }).ToList();

            return result.Select(entry =>
                new NonRevenueWaterEntryFileDumpViewModel {
                    Year = entry.Year.ToString(),
                    Month = entry.Month.ToString(),
                    BusinessUnit = entry.BusinessUnit?.PadLeft(entry.BusinessUnit.Length + 4, '0'),
                    OperatingCenter = entry.Description,
                    Value = entry.TotalGallons.ToString()
                }).AsQueryable().OrderBy(x => x.BusinessUnit);
        }

        #endregion
    }
}
