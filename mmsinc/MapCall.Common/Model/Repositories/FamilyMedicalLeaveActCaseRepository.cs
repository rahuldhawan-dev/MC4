using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Data;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface IFamilyMedicalLeaveActCaseRepository : IRepository<FamilyMedicalLeaveActCase>
    {
        IEnumerable<FamilyMedicalLeaveActCase> GetByEmployeeId(int employeeId);
    }

    public class FamilyMedicalLeaveActCaseRepository : MapCallEmployeeSecuredRepositoryBase<FamilyMedicalLeaveActCase>,
        IFamilyMedicalLeaveActCaseRepository
    {
        #region Constants

        public const RoleModules ROLE = RoleModules.OperationsManagement;

        #endregion

        #region Properties

        public override RoleModules Role => ROLE;

        #endregion

        #region Constructors

        public FamilyMedicalLeaveActCaseRepository(ISession session, IContainer container,
            IAuthenticationService<User> authenticationService, IRepository<AggregateRole> roleRepo) : base(session, container,
            authenticationService, roleRepo) { }

        #endregion

        public IEnumerable<FamilyMedicalLeaveActCase> GetByEmployeeId(int employeeId)
        {
            return (from fmla in Linq where fmla.Employee.Id == employeeId select fmla);
        }
    }
}
