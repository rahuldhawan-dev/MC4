using System;
using System.Collections.Generic;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class PositionGroupRepository : RepositoryBase<PositionGroup>, IPositionGroupRepository
    {
        private readonly IDateTimeProvider _dateTimeProvider;

        public PositionGroupRepository(ISession session, IContainer container, IDateTimeProvider dateTimeProvider) :
            base(session, container)
        {
            _dateTimeProvider = dateTimeProvider;
        }

        public IEnumerable<TrainingClassificationSumReportItem> GetTrainingClassificationSumReport(
            ISearchTrainingClassificationSum search)
        {
            var now = _dateTimeProvider.GetCurrentDate();

            TrainingClassificationSumReportItem result = null;
            PositionGroupCommonName commonName = null;
            TrainingRequirement requirement = null;
            TrainingModule module = null;
            TrainingRecord record = null;
            TrainingRecordAttendedEmployee attended = null;
            Employee employee = null;
            OperatingCenter opCntr = null;

            var query = Session.QueryOver<PositionGroup>();

            query.JoinAlias(x => x.CommonName, () => commonName);
            query.JoinAlias(_ => commonName.TrainingRequirements, () => requirement);
            query.JoinAlias(_ => requirement.TrainingModules, () => module);
            query.JoinAlias(_ => module.TrainingRecords, () => record);
            query.JoinAlias(_ => record.EmployeesAttended, () => attended);
            query.JoinAlias(_ => attended.Employee, () => employee);
            query.JoinAlias(_ => employee.OperatingCenter, () => opCntr);

            query.SelectList(x => x
                                 .SelectGroup(_ => opCntr.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => requirement.IsOSHARequirement)
                                 .WithAlias(() => result.OSHARequirement)
                                 .SelectGroup(_ => module.AmericanWaterCourseNumber).WithAlias(() => result.ClassId)
                                 .SelectGroup(_ => module.Title).WithAlias(() => result.TrainingModule)
                                 .SelectGroup(_ => commonName.Description).WithAlias(() => result.CommonName)
                                 .SelectGroup(position => position.PositionDescription).WithAlias(() => result.Position)
                                 .SelectGroup(position => position.Group).WithAlias(() => result.PositionGroup)
                                 .Select(Projections.GroupProperty(Projections.SqlFunction("year", NHibernateUtil.Int32,
                                      Projections.Property(() => record.HeldOn)))).WithAlias(() => result.Year)
                                 .Select(Projections.GroupProperty(Projections.SqlFunction("month",
                                      NHibernateUtil.Int32, Projections.Property(() => record.HeldOn))))
                                 .WithAlias(() => result.Month)
                                 .SelectCount(_ => employee.Id).WithAlias(() => result.NumberOfEmployees)
            );

            query.Where(_ => record.HeldOn != null);
            query.Where(_ => record.HeldOn < now);

            if (search.Year.HasValue)
            {
                var beginningOfYear = new DateTime(search.Year.Value, 1, 1);
                query.Where(_ => record.HeldOn >= beginningOfYear && record.HeldOn < beginningOfYear.AddYears(1));
            }

            query.TransformUsing(Transformers.AliasToBean<TrainingClassificationSumReportItem>());

            return Search(search, query);
        }
    }

    public interface IPositionGroupRepository : IRepository<PositionGroup>
    {
        IEnumerable<TrainingClassificationSumReportItem> GetTrainingClassificationSumReport(
            ISearchTrainingClassificationSum search);
    }
}
