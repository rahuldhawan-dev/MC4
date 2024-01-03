using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.Repositories.Users;
using MapCall.Common.Utility.Notifications;
using MapCallScheduler.Library.JobHelpers.Sap;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SapEmployee
{
    public class SapEmployeeUpdaterService :
        SapEntityUpdaterServiceBase<SapEmployeeFileRecord, ISapEmployeeFileParser, Employee, IRepository<Employee>>,
        ISapEmployeeUpdaterService
    {
        #region Fields

        private readonly IUserRepository _userRepository;
        private readonly IPositionGroupRepository _positionGroupRepo;
        private readonly INotificationService _notificationService;
        private readonly UserType _internalUserType;
        private readonly Dictionary<string, EmployeeStatus> _employeeStatusesByName;
        private readonly Dictionary<string, SAPCompanyCode> _sapCompanyCodesByDescription;
        private readonly Dictionary<int, PersonnelArea> _personnelAreasById;
        private readonly List<PositionGroup> _positionGroups;
        private readonly Dictionary<string, State> _statesByAbbreviation;
        private readonly Dictionary<Employee, string> _employeesWithSupervisors = new Dictionary<Employee, string>();
        private readonly Dictionary<Employee, string> _employeesWithHRManagers = new Dictionary<Employee, string>();
        // internal for testing use only!
        internal readonly HashSet<string> _newEmployeeIds = new HashSet<string>();
        private readonly CommercialDriversLicenseProgramStatus _notInProgramStatus;
        private readonly List<PositionGroup> _newPositionGroupsToSave = new List<PositionGroup>();

        #endregion

        #region Constructors

        public SapEmployeeUpdaterService(ISapEmployeeFileParser parser, IRepository<Employee> repository,
            IEmployeeStatusRepository empStatRepo, IPersonnelAreaRepository paRepo, 
            IUserRepository userRepo, IUserTypeRepository userTypeRepo, IPositionGroupRepository posGroupRepo,
            ICommercialDriversLicenseProgramStatusRepository cdlRepo, IRepository<State> stateRepo, 
            IRepository<SAPCompanyCode> sapCompanyCodeRepo, INotificationService notificationService, ILog log) : base(parser, repository, log)
        {
            _userRepository = userRepo;
            _positionGroupRepo = posGroupRepo;
            _notificationService = notificationService;

            // These are things we're not gonna want to requery a thousand times.
            _internalUserType = userTypeRepo.GetInternalUserType();
            _employeeStatusesByName = empStatRepo.GetAll().ToDictionary(x => x.Description, x => x);
            _personnelAreasById = paRepo.GetAll().ToDictionary(x => x.PersonnelAreaId, x => x);
            _positionGroups = _positionGroupRepo.Where(x => x.SAPPositionGroupKey != null).ToList();
            _notInProgramStatus = cdlRepo.GetNotInProgramStatus();
            _statesByAbbreviation = stateRepo.GetAll().ToDictionary(x => x.Abbreviation, x => x);
            _sapCompanyCodesByDescription = sapCompanyCodeRepo.GetAll().ToDictionary(x => x.Description, x => x);
        }

        #endregion

        #region Private Methods

        private void MapEmployeeStatus(Employee employee, SapEmployeeFileRecord record)
        {
            if (_employeeStatusesByName.TryGetValue(record.Status, out EmployeeStatus status))
            {
                employee.Status = status;
            }
            else
            {
                throw new ArgumentException($"Unable to find an employee status with the description '{record.Status}'.");
            }
        }

        private void MapPositionGroup(Employee employee, SapEmployeeFileRecord record)
        {
            // MC-1701: Previously, SAP began sending us data with null(or changed) PositionGroup data. 
            // This caused the Employee.PositionGroup field to get nulled out. There are valid reasons to 
            // not send any PositionGroup information("non-employees" and possibly others) but we do not 
            // have to detect when this is valid. Rather than throw an error or null it out, we're just 
            // going to skip it.
            if (string.IsNullOrWhiteSpace(record.PositionGroupKey))
            {
                return;
            }

            // MC-1701: "99999999" is a retired/withdrawn/otherwise-no-longer-working employee.
            // They will not have a position, and there isn't enough other information to map this
            // as a valid PositionGroup record, so null it out.
            if (record.PositionGroupKey == "99999999")
            {
                employee.PositionGroup = null;
                return;
            }

            // NOTE: We need to keep track of position groups that do not exist and add them to the db 
            // and also send out a notification.
            var pg = _positionGroups.SingleOrDefault(x => x.SAPPositionGroupKey == record.PositionGroupKey);
            if (pg == null)
            {
                pg = TryCreateNewPositionGroup(record);
            }

            // We want to ensure we don't accidentally wipe out the existing PositionGroup
            // for an employee after we attempt to create a new PositionGroup if a new PositionGroup
            // is not able to be created.
            if (pg != null)
            {
                employee.PositionGroup = pg;
            }
        }

        private PositionGroup TryCreateNewPositionGroup(SapEmployeeFileRecord record)
        {
            // Same as above in regards to SAP suddenly sending us null data. We want to throw
            // an exception if any of the required fields for a new PositionGroup are missing.
            // Except for PositionGroupGroup, see next comment.
            if (string.IsNullOrWhiteSpace(record.PositionGroupBusinessUnit)
                || string.IsNullOrWhiteSpace(record.PositionGroupBusinessUnitDescription)
                || string.IsNullOrWhiteSpace(record.PositionGroupCompanyCode)
                || string.IsNullOrWhiteSpace(record.PositionGroupState))
            {
                throw new ArgumentException($"Invalid data from SAP for employee id {record.EmployeeId}. One of the many position group fields needed to create a new PositionGroup is missing a value.");
            }

            // On rare occasions, this is null when everything else is not. When this happens,
            // the employee does not have valid position group data on SAP and we don't want to create
            // a position group for this. We don't want to throw an error here because it's
            // not really our problem.
            if (string.IsNullOrWhiteSpace(record.PositionGroupGroup))
            {
                return null;
            }

            var pg = new PositionGroup();
            pg.SAPPositionGroupKey = record.PositionGroupKey;
            pg.BusinessUnit = record.PositionGroupBusinessUnit;
            pg.BusinessUnitDescription = record.PositionGroupBusinessUnitDescription;
            pg.Group = record.PositionGroupGroup;
            pg.PositionDescription = record.PositionGroupPositionDescription;

            if (!_statesByAbbreviation.ContainsKey(record.PositionGroupState))
            {
                throw new KeyNotFoundException($"Unable to find state with abbreviation '{record.PositionGroupState}'");
            }
            pg.State = _statesByAbbreviation[record.PositionGroupState];

            if (!_sapCompanyCodesByDescription.ContainsKey(record.PositionGroupCompanyCode))
            {
                throw new KeyNotFoundException($"Unable to find SAPCompanyCode with description '{record.PositionGroupCompanyCode}'");
            }
            pg.SAPCompanyCode = _sapCompanyCodesByDescription[record.PositionGroupCompanyCode];

            // PositionGroupCommonName is of importance for NJAW employees. This field is used to determine
            // employee's training. Due to SAP considering seemingly-identical position groups as unique, we
            // need to try and set the common name based on an existing position group that already has a common name.
            pg.CommonName = _positionGroups.FirstOrDefault(x => x.BusinessUnit == pg.BusinessUnit
                                                                && x.BusinessUnitDescription == pg.BusinessUnitDescription
                                                                && x.Group == pg.Group 
                                                                && x.PositionDescription == pg.PositionDescription
                                                                && x.State == pg.State 
                                                                && x.SAPCompanyCode == pg.SAPCompanyCode
                                                                && x.CommonName != null)?.CommonName;

            _newPositionGroupsToSave.Add(pg);
            _positionGroups.Add(pg);

            return pg;
        }

        private void MapOperatingCenterAndPersonnelArea(Employee employee, SapEmployeeFileRecord record)
        {
            // Originally this threw an error if the PersonnelArea couldn't be found or if the PersonnelArea did
            // not have an OperatingCenter associated with it. It's been requested that this does not throw an error
            // and only overwrites existing Employee.OperatingCenter values if one can be found. 

            if (_personnelAreasById.TryGetValue(record.PersonnelAreaId, out var pa))
            {
                employee.PersonnelArea = pa;
                if (pa.OperatingCenter != null)
                {
                    employee.OperatingCenter = pa.OperatingCenter;
                }
            }
        }

        private void MapCommercialDrivesLicense(Employee employee)
        {
            // This isn't included in the import, but it is a non-nullable field. Needs a default value if the employee
            // does not currently have this set.
            if (employee.CommercialDriversLicenseProgramStatus == null)
            {
                employee.CommercialDriversLicenseProgramStatus = _notInProgramStatus;
            }
        }

        private void ActivateOrDeactivateMatchingUsers(Employee employee, SapEmployeeFileRecord record)
        {
            // NOTE: This method needs to be called after the employee's Status has been mapped.
            // Not all employees are users, so we can't do anything with the userless employees.
            if (employee.User == null)
            {
                return;
            }

            if (employee.User.Employee != employee)
            {
                // There is the possibility that the User/Employee link has become invalid because someone
                // entered the wrong Employee info for a User, then later updated the User's Employee. This
                // will leave the Employee record in a weird limbo where it's linked to a User that doesn't
                // link back to it. 

                throw new InvalidOperationException($"Employee#{employee.Id} links to a User#{employee.User.Id} that links to a different or null Employee(Id: {employee.User.Employee?.Id}).");
            }

            // We don't want to deactivate or activate site admin users cause that would be bad. Also we only
            // want to deactivate users that are "internal", whatever that means.
            if (employee.User.IsAdmin || employee.User.UserType != _internalUserType)
            {
                return;
            }

            employee.User.HasAccess = employee.IsActive;

            // NOTE: You're seeing a lack of any saving here because NHibernate is already doing change
            // tracking and will eventually save these changes when the employee is saved.
        }

        private void MapZipCode(Employee employee, SapEmployeeFileRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.ZipCode))
            {
                employee.ZipCode = null;
            }
            else
            {
                // SAP drops the beginning zeros from zipcodes for some reason. ex: 7720 instead of 07720. 
                int maybeZipCode;
                if (int.TryParse(record.ZipCode, out maybeZipCode))
                {
                    employee.ZipCode = maybeZipCode.ToString("00000");
                }
                else
                {
                    // If we can't parse the zipcode, set it as-is. They like to send things like "WESTINDIES"
                    // as a zipcode for somre reason.
                    employee.ZipCode = record.ZipCode;
                }
            }
        }

        protected override void MapRecord(Employee employee, SapEmployeeFileRecord record, int currentLine)
        {
            // Need to store the SAP Record's ReportsTo value with the employee so we can
            // set the employee supervisor during SecondPass. Same for LocalEmployeeRelationsBusinessPartner
            _employeesWithSupervisors[employee] = record.ReportsTo;
            _employeesWithHRManagers[employee] = record.LocalEmployeeRelationsBusinessPartner;

            // NOTE: Uniqueness of employee is validated in FindOrCreateEntity.
            MapField(employee, record, x => x.FirstName);
            MapField(employee, record, x => x.LastName);
            MapField(employee, record, x => x.EmployeeId);
            MapField(employee, record, x => x.DateHired);
            MapField(employee, record, x => x.City);
            MapField(employee, record, x => x.State);
            MapField(employee, record, x => x.EmailAddress);
            MapField(employee, record, x => x.PhoneHome);
            MapField(employee, record, x => x.Address);
            MapField(employee, record, x => x.Address2);
            
            // Temporary null check. This is only here while we wait for the SAP side 
            // to start passing us the new file with the new column. We don't want to
            // erase the existing data we have while they aren't sending us stuff.
            if (!string.IsNullOrWhiteSpace(record.PhoneCellular))
            {
                MapField(employee, record, x => x.PhoneCellular);
            }

            MapZipCode(employee, record);
            MapEmployeeStatus(employee, record);
            MapPositionGroup(employee, record);
            MapOperatingCenterAndPersonnelArea(employee, record);
            MapCommercialDrivesLicense(employee);

            ActivateOrDeactivateMatchingUsers(employee, record);
        }

        private void MapEmployeeForSecondPass(List<Employee> mappedEntities, IDictionary<Employee, string> employeesWithOtherEmployees, Action<Employee, Employee> setter, string propertyBeingSet)
        {
            var employeesByEmployeeId = mappedEntities.ToDictionary(x => x.EmployeeId, x => x);

            foreach (var kvPair in employeesWithOtherEmployees)
            {
                var emp = kvPair.Key;
                var otherEmpId = kvPair.Value;
                if (string.IsNullOrWhiteSpace(otherEmpId))
                {
                    // A handful of employees(Kirwan) don't report to anyone.
                    setter(emp, null);
                }
                else
                {
                    // First check to see if we already have the supervisor employee. If the supervisor
                    // is a new employee record, they won't exist in the database.
                    if (employeesByEmployeeId.ContainsKey(otherEmpId))
                    {
                        setter(emp, employeesByEmployeeId[otherEmpId]);
                    }
                    else
                    {
                        // If the supervisor isn't in the collection, then check the database.
                        var existing = _repository.GetByEmployeeId(otherEmpId);

                        // GetByEmployeeId will return null if there isn't a match. It was
                        // state in the bug that nulling out the ReportsTo field is fine if
                        // a match isn't found.
                        setter(emp, existing);

                        if (existing == null)
                        {
                            _log.Info($"Setting employee with employee id '{emp.EmployeeId}' {propertyBeingSet} value to null because the employee with employee id {otherEmpId} does not exist.");
                        }
                    }
                }
            }
        }

        private void MapEmployeeReportsToForSecondPass(List<Employee> mappedEntities)
        {
            MapEmployeeForSecondPass(mappedEntities, _employeesWithSupervisors, 
                (empToSet, empValue) => { empToSet.ReportsTo = empValue; }, "ReportsTo");
        }

        private void MapEmployeeHumanResourcesManagerForSecondPass(List<Employee> mappedEntities)
        {
            MapEmployeeForSecondPass(mappedEntities, _employeesWithHRManagers,
                (empToSet, empValue) => { empToSet.HumanResourcesManager = empValue; }, "HumanResourcesManager");
        }

        protected override void SecondPass(List<Employee> mappedEntities)
        {
            base.SecondPass(mappedEntities);
            MapEmployeeReportsToForSecondPass(mappedEntities);
            MapEmployeeHumanResourcesManagerForSecondPass(mappedEntities);

            foreach (var entity in mappedEntities)
            {
                if (_newEmployeeIds.Contains(entity.EmployeeId))
                {
                    _notificationService.Notify(new NotifierArgs
                    {
                        Purpose = "New Hire Email Notification",
                        Module = RoleModules.HumanResourcesEmployee,
                        Data = entity,
                        Subject = "New Hire Email Notification"
                    });
                }
            }

            // NOTE: Saving the new PositionGroups must happen before
            // saving the employee records or else all sorts of nhibernate
            // explosions occur.
            if (_newPositionGroupsToSave.Any())
            {
                _positionGroupRepo.Save(_newPositionGroupsToSave);
                _notificationService.Notify(new NotifierArgs {
                  Purpose = "Position Groups Created",
                  Module = RoleModules.HumanResourcesPositions,
                  Data = _newPositionGroupsToSave,
                  Subject = "Position Groups Created"
                });
            }
        }

        protected override Employee FindOrCreateEntity(SapEmployeeFileRecord record, int currentLine)
        {
            var existing = _repository.GetByEmployeeId(record.EmployeeId);
            if (existing == null)
            {
                // Need to do a secondary check of this since new records won't be in the database yet.
                if (_newEmployeeIds.Contains(record.EmployeeId))
                {
                    throw new ArgumentException($"More than one new employee has the Employee ID '{record.EmployeeId}'. Employee ID must be unique.");
                }

                _newEmployeeIds.Add(record.EmployeeId);
                return new Employee();
            }

            return existing;
        }

        protected override void LogRecord(SapEmployeeFileRecord record)
        {
            _log.Info($"Updating employee with employee id '{record.EmployeeId}'...");
        }

        #endregion
    }
}