using System.Linq;
using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Reports.Controllers;
using MapCallMVC.Models.ViewModels.OperatorLicenses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Reports.Controllers
{
    [TestClass]
    public class OperatorLicenseReportControllerTest : MapCallMvcControllerTestBase<OperatorLicenseReportController, OperatorLicense, OperatorLicenseRepository>
    {
        #region Constants

        private const RoleModules ROLE = RoleModules.HumanResourcesEmployeeLimited;

        #endregion

        #region Init/Cleanup

        protected override User CreateUser()
        {
            return GetFactory<AdminUserFactory>().Create();
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                const string path = "~/Reports/OperatorLicenseReport/";
                a.RequiresRole($"{path}Search", ROLE);
                a.RequiresRole($"{path}Index", ROLE);
            });
        }

        #region Index

        [TestMethod]
        public override void TestIndexReturnsResults()
        {
            GetEntityFactory<OperatorLicense>().Create();
            GetEntityFactory<OperatorLicense>().Create();

            var result = (ViewResult)_target.Index(new SearchOperatorLicenseReport());
            var resultModel = (SearchOperatorLicenseReport)result.Model;

            MvcAssert.IsViewNamed(result, "Index");
            Assert.AreEqual(2, resultModel.Results.Count());
        }

        [TestMethod]
        public void TestIndexReturnsFilteredResults()
        {
            GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = GetEntityFactory<OperatorLicenseType>().Create(new {
                    Description = "Type 01"
                })
            });

            var operatorLicense02 = GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = GetEntityFactory<OperatorLicenseType>().Create(new {
                    Description = "Type 02"
                })
            });

            var result = (ViewResult)_target.Index(new SearchOperatorLicenseReport {
                OperatorLicenseType = operatorLicense02.Id
            });

            var resultModel = (SearchOperatorLicenseReport)result.Model;

            Assert.AreEqual(1, resultModel.Results.Count());
            Assert.AreEqual(operatorLicense02.Id, resultModel.Results.First().Id);
        }

        [TestMethod]
        public void TestIndexExportsAllResultsInExcel()
        {
            var operatorLicense01 = GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = GetEntityFactory<OperatorLicenseType>().Create(new {
                    Description = "Type 01"
                })
            });

            var operatorLicense02 = GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = GetEntityFactory<OperatorLicenseType>().Create(new {
                    Description = "Type 02"
                })
            });

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(new SearchOperatorLicenseReport());
            using (var helper = new ExcelResultTester(_container, result, true))
            {
                Assert.AreEqual(2, helper.GetRowCount("Sheet1"));
                helper.AreEqual(operatorLicense01.OperatorLicenseType, "OperatorLicenseType");
                helper.AreEqual(operatorLicense02.OperatorLicenseType, "OperatorLicenseType", 1);
            }
        }

        [TestMethod]
        public void TestIndexExportsFilteredResultsInExcel()
        {
            var operatorLicenseType01 = GetEntityFactory<OperatorLicenseType>().Create(new {
                Description = "Type 01"
            });

            var operatorLicenseType02 = GetEntityFactory<OperatorLicenseType>().Create(new {
                Description = "Type 02"
            });

            GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = operatorLicenseType01
            });

            var operatorLicense02 = GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = operatorLicenseType02
            });

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = (ExcelResult)_target.Index(new SearchOperatorLicenseReport {
                OperatorLicenseType = operatorLicenseType02.Id
            });

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                Assert.AreEqual(1, helper.GetRowCount("Sheet1"));
                helper.AreEqual(operatorLicense02.OperatorLicenseType, "OperatorLicenseType");
            }
        }

        [TestMethod]
        public void TestIndexAndExcelExportFilteredRecordCountsMatch()
        {
            var operatorLicenseType02 = GetEntityFactory<OperatorLicenseType>().Create(new {
                Description = "Type 02"
            });

            GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = GetEntityFactory<OperatorLicenseType>().Create(new {
                    Description = "Type 01"
                })
            });

            GetEntityFactory<OperatorLicense>().Create(new {
                OperatorLicenseType = operatorLicenseType02
            });

            var searchCriteria = new SearchOperatorLicenseReport {
                OperatorLicenseType = operatorLicenseType02.Id
            };

            var indexResult = (ViewResult)_target.Index(searchCriteria);
            var indexResultModel = (SearchOperatorLicenseReport)indexResult.Model;

            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] = ResponseFormatter.KnownExtensions.EXCEL_2003;

            var excelResult = (ExcelResult)_target.Index(searchCriteria);

            using (var helper = new ExcelResultTester(_container, excelResult, true))
            {
                Assert.AreEqual(indexResultModel.Results.Count(), helper.GetRowCount("Sheet1"));
            }
        }

        #endregion
    }
}
