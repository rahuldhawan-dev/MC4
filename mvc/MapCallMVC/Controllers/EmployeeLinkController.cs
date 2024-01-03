using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCallMVC.Models.ViewModels;
using MMSINC.ClassExtensions;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using MMSINC.Exceptions;
using StructureMap;

namespace MapCallMVC.Controllers
{
    public class EmployeeLinkController : ControllerBaseWithPersistence<IRepository<EmployeeLink>, EmployeeLink, User>
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#EmployeesTab";
        public const string EMPLOYEES_KEY = "EmployeeIds";

        #endregion

        #region Private Members

        private IEmployeeRepository _employeeRepository;
        private IDataTypeRepository _dataTypeRepository;

        #endregion

        #region Properties

        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                return _employeeRepository ?? (_employeeRepository = _container.GetInstance<IEmployeeRepository>());
            }
        }

        public IDataTypeRepository DataTypeRepository
        {
            get
            {
                return _dataTypeRepository ?? (_dataTypeRepository = _container.GetInstance<IDataTypeRepository>());
            }
        }

        #endregion

        #region New/Create

        [HttpGet]
        public ActionResult New(NewEmployeeLink model)
        {
            if (String.IsNullOrWhiteSpace(model.TableName))
            {
                ModelState.AddModelError("TableName", "TableName cannot be null.");
                throw new ModelValidationException(ModelState);
            }

            this.AddDynamicDropDownData<IEmployeeRepository, Employee, EmployeeDisplayItem>(EMPLOYEES_KEY, r => r.GetActiveEmployeesForSelect());
            // Filtering OpCenters by role is not required here. Alex said so.
            this.AddDynamicDropDownData<OperatingCenter, OperatingCenterDisplayItem>();

            var dataTypes = String.IsNullOrWhiteSpace(model.DataTypeName)
                ? DataTypeRepository.GetByTableName(model.TableName).ToArray()
                : DataTypeRepository.GetByTableNameAndDataTypeName(model.TableName, model.DataTypeName);

            if (dataTypes.Count() > 1)
            {
                throw new ArgumentException(String.Format("More than one DataType found for TableName {0}.", model.TableName));
            }
            if (!dataTypes.Any())
            {
                throw new ArgumentException(String.Format("No DataType found for TableName {0}.", model.TableName));
            }

            model.DataTypeId = dataTypes.First().Id;

            return View(model);
        }

        [HttpPost]
        public ActionResult Create(NewEmployeeLink model)
        {
            // Users like to submit without linking anything, resulting in a null array.
            if (model.EmployeeIds != null)
            {
                var dataType = DataTypeRepository.Find(model.DataTypeId);
                var toCreate = new List<EmployeeLink>();

                foreach (var id in model.EmployeeIds)
                {
                    var newLink = new EmployeeLink
                    {
                        LinkedId = model.LinkedId,
                        DataType = dataType,
                        LinkedOn = DateTime.Now,
                        LinkedBy = AuthenticationService.CurrentUser.FullName ?? AuthenticationService.CurrentUser.UniqueName,
                        Employee = EmployeeRepository.Find(id)
                    };

                    if (!TryValidateModel(newLink))
                    {
                        throw new ModelValidationException(ModelState);
                    }
                    ModelState.Clear();
                    toCreate.Add(newLink);
                }

                foreach (var link in toCreate)
                {
                    Repository.Save(link);
                }
            }

            return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);
        }

        #endregion

        #region Delete/Destroy

        [HttpDelete]
        public ActionResult Destroy(DeleteEmployeeLink model)
        {
            var link = Repository.Find(model.Id);

            if (link == null)
            {
                return HttpNotFound(string.Format("EmployeeLink with id '{0}' not found.", model.Id));
            }

            Repository.Delete(link);

            return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);
        }

        #endregion

        public EmployeeLinkController(ControllerBaseWithPersistenceArguments<IRepository<EmployeeLink>, EmployeeLink, User> args) : base(args) {}
    }
}