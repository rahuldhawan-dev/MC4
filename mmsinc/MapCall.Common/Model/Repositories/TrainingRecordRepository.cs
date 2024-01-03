using System;
using System.Collections.Generic;
using FluentNHibernate.Utils;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data.NHibernate;
using MapCall.Common.Model.Entities;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Transform;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TrainingRecordRepository : RepositoryBase<TrainingRecord>, ITrainingRecordRepository
    {
        #region Fields

        private readonly IRepository<EmployeeLink> _employeeLinkRepository;

        #endregion

        #region Constructors

        public TrainingRecordRepository(ISession session, IContainer container,
            IRepository<EmployeeLink> employeeLinkRepository) : base(session, container)
        {
            _employeeLinkRepository = employeeLinkRepository;
        }

        #endregion

        #region Exposed Methods

        public override void Delete(TrainingRecord entity)
        {
            entity.LinkedEmployeesScheduled.Each(x => {
                var empLink = Session.Load<EmployeeLink>(x.Id);
                _employeeLinkRepository.Delete(empLink);
            });

            entity.LinkedEmployeesAttended.Each(x => {
                var empLink = Session.Load<EmployeeLink>(x.Id);
                _employeeLinkRepository.Delete(empLink);
            });

            base.Delete(entity);
        }

        public IEnumerable<TrainingTotalHoursReportItem> GetTotalTrainingHours(ISearchTrainingTotalHours search)
        {
            TrainingTotalHoursReportItem result = null;
            TrainingRecord record = null, attRcrd = null, schRcrd = null, sessRcrd = null;
            TrainingModule module = null;
            TrainingRequirement requirement = null;
            PositionGroupCommonName commonName = null;
            PositionGroup position = null;
            EmployeeLink employeeLink = null;
            Employee allEmp = null, attEmp = null, schEmp = null;
            TrainingSession session = null;
            OperatingCenter opCntr = null;

            var query = Session.QueryOver(() => record);

            query.JoinAlias(x => x.TrainingModule, () => module);
            query.JoinAlias(_ => module.TrainingRequirement, () => requirement);
            query.JoinAlias(_ => requirement.PositionGroupCommonNames, () => commonName);
            query.JoinAlias(_ => commonName.PositionGroups, () => position);
            query.JoinAlias(x => x.EmployeesScheduledOrAttended, () => employeeLink);
            query.JoinAlias(_ => employeeLink.Employee, () => allEmp);
            query.JoinAlias(_ => allEmp.OperatingCenter, () => opCntr);

            query.SelectList(x => x
                                 .SelectGroup(_ => opCntr.OperatingCenterCode).WithAlias(() => result.OperatingCenter)
                                 .SelectGroup(_ => requirement.IsOSHARequirement)
                                 .WithAlias(() => result.OSHARequirement)
                                 .SelectGroup(_ => module.AmericanWaterCourseNumber).WithAlias(() => result.ClassId)
                                 .SelectGroup(_ => module.Title).WithAlias(() => result.TrainingModule)
                                 .SelectGroup(_ => commonName.Description).WithAlias(() => result.CommonName)
                                 .SelectGroup(_ => position.PositionDescription).WithAlias(() => result.Position)
                                 .SelectGroup(_ => position.Group).WithAlias(() => result.PositionGroup)
                                 .SelectGroup(_ => module.Id)
                                 .SelectGroup(_ => position.Id)
                                 .Select(Projections.GroupProperty(
                                      Projections.SqlFunction("year", NHibernateUtil.Int32,
                                          Projections.SqlFunction("coalesce", NHibernateUtil.Int32,
                                              Projections.Property(() => record.HeldOn),
                                              Projections.Property(() => record.ScheduledDate)))))
                                 .WithAlias(() => result.Year)
                                 .SelectSubQuery(QueryOver.Of<TrainingRecordAttendedEmployee>()
                                                          .JoinAlias(y => y.TrainingRecord, () => attRcrd)
                                                          .JoinAlias(y => y.Employee, () => attEmp)
                                                          .Where(_ => attRcrd.TrainingModule.Id == module.Id)
                                                          .Where(_ => attEmp.PositionGroup.Id == position.Id)
                                                          .Select(Projections.Count<TrainingRecordAttendedEmployee>(y =>
                                                               y.Id)))
                                 .WithAlias(() => result.TotalEmployeesAttended)
                                 .SelectSubQuery(QueryOver.Of<TrainingRecordScheduledEmployee>()
                                                          .JoinAlias(y => y.TrainingRecord, () => schRcrd)
                                                          .JoinAlias(y => y.Employee, () => schEmp)
                                                          .Where(_ => schRcrd.TrainingModule.Id == module.Id)
                                                          .Where(_ => schEmp.PositionGroup.Id == position.Id)
                                                          .Select(Projections.Count<TrainingRecordScheduledEmployee>(
                                                               y => y.Id)))
                                 .WithAlias(() => result.TotalEmployeesScheduled)
                                 .SelectSubQuery(QueryOver.Of<TrainingSession>()
                                                          .JoinAlias(y => y.TrainingRecord, () => sessRcrd)
                                                          .Where(_ => sessRcrd.TrainingModule.Id == module.Id)
                                                          .Select(Projections.Sum(Projections.SqlFunction(
                                                               new DateDiffTemplatedFunction("hour", "StartDateTime",
                                                                   "EndDateTime"),
                                                               NHibernateUtil.Int32))))
                                 .WithAlias(() => result.TotalHours)
            );

            query.Where(r => r.HeldOn != null || r.ScheduledDate != null);

            if (search.Year.HasValue)
            {
                var beginningOfYear = new DateTime(search.Year.Value, 1, 1);
                var endOfYear = beginningOfYear.AddYears(1);
                query.Where(_ =>
                    (record.HeldOn != null && record.HeldOn >= beginningOfYear && record.HeldOn < endOfYear) ||
                    (record.ScheduledDate != null && record.ScheduledDate >= beginningOfYear &&
                     record.ScheduledDate < endOfYear));
            }

            query.OrderByAlias(() => opCntr.OperatingCenterCode);
            query.OrderBy(
                Projections.SqlFunction("year", NHibernateUtil.Int32,
                    Projections.SqlFunction("coalesce", NHibernateUtil.Int32,
                        Projections.Property(() => record.HeldOn),
                        Projections.Property(() => record.ScheduledDate))));

            query.TransformUsing(Transformers.AliasToBean<TrainingTotalHoursReportItem>());

            return Search(search, query);
        }

        #endregion
    }

    public interface ITrainingRecordRepository : IRepository<TrainingRecord>
    {
        IEnumerable<TrainingTotalHoursReportItem> GetTotalTrainingHours(ISearchTrainingTotalHours search);
    }
}
