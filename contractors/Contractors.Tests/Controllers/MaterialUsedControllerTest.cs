using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Contractors.Controllers;
using Contractors.Models.ViewModels;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing.Data;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace Contractors.Tests.Controllers
{
    [TestClass]
    public class MaterialUsedControllerTest : ContractorControllerTestBase<MaterialUsedController, MaterialUsed, SecuredRepositoryBase<MaterialUsed, ContractorUser>>
    {
        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IIconSetRepository>().Use<IconSetRepository>();
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options); 

            // This is currently not making a valid entity for the current user.
            // The material never gets an operating center that the current user's contractor has.
            options.CreateValidEntity = () => {
                var workOrder = GetFactory<WorkOrderFactory>().Create();
                var material = GetEntityFactory<Material>().Create();
                material.OperatingCenters.Add(_automatedTestOperatingCenter);
                Session.Save(material);
                Session.Flush();
                return GetEntityFactory<MaterialUsed>().Create(new { Material = material, StockLocation = GetEntityFactory<StockLocation>().Create(), WorkOrder = workOrder });
            };
            options.CreateReturnsPartialShowViewOnSuccess = true;
            options.DestroyReturnsHttpStatusCodeNoContentOnSuccess = true;
            options.EditReturnsPartialView = true;
            options.ExpectedEditViewName = "_Edit";
            options.UpdateReturnsPartialShowViewOnSuccess = true;
        }

        #endregion

        #region Authorization

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                a.RequiresLoggedInUserOnly("~/MaterialUsed/New");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Create");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Edit");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Update");
                a.RequiresLoggedInUserOnly("~/MaterialUsed/Destroy");
            });
        }

        #endregion

        #region Create/New

        [TestMethod]
        public override void TestCreateReturnsNewViewWithModelIfModelStateErrorsExist()
        {
            Assert.Inconclusive("Why does this return EmptyResult?");
        }

        [TestMethod]
        public void TestCreateReturnsEmptyResultIfModelStateIsInvalid()
        {
            _target.ModelState.AddModelError("nope", "nuh uh");
            var result = _target.Create(_viewModelFactory.Build<CreateMaterialUsed>());
            Assert.IsInstanceOfType(result, typeof(EmptyResult));
        }

        [TestMethod]
        public void TestCreateCreatesAndShowsMaterialUsedIfModelStateIsValid()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            operatingCenter.Contractors.Add(_currentUser.Contractor);
            Session.Merge(operatingCenter);
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = operatingCenter
            });
            var material = GetFactory<MaterialFactory>().Create();
            operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial {
                OperatingCenter = operatingCenter, Material = material
            });
            Session.Merge(operatingCenter);
            var model = _viewModelFactory.Build<CreateMaterialUsed, MaterialUsed>(GetFactory<MaterialUsedFactory>().Build(new {
                Material = material,
                WorkOrder = order
            }));
            PartialViewResult result = null;

            MyAssert.CausesIncrease(
                () => result =
                        (PartialViewResult)_target.Create(model),
                () => Repository.GetAll().Count());
            var resultModel = (MaterialUsed)result.Model;

            Assert.IsNotNull(result);
            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreNotEqual(0, resultModel.Id);
        }

        [TestMethod]
        public override void TestNewReturnsNewViewWithNewViewModel()
        {
            // override due to parameter on New action.
            // the test below seems to cover this and more.
        }

        [TestMethod]
        public void TestNewSetsViewDataAndWorkOrderIDOfModelPassedToView()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            operatingCenter.Contractors.Add(_currentUser.Contractor);
            Session.Merge(operatingCenter);
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = operatingCenter
            });
            var material = GetFactory<MaterialFactory>().Create();
            var stockLocation = GetFactory<StockLocationFactory>().Create(new { IsActive = true });
            operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = material});
            operatingCenter.StockLocations.Add(stockLocation);
            Session.Merge(operatingCenter);

            var result = (PartialViewResult)_target.New(order.Id);

            var materials =
                ((IEnumerable<SelectListItem>)result.ViewData[MaterialUsedController.VIEWDATA_MATERIALS]).ToArray();
            var stockLocations =
                ((IEnumerable<SelectListItem>)result.ViewData[MaterialUsedController.VIEWDATA_STOCK_LOCATIONS]).
                    ToArray();

            Assert.AreEqual(1, materials.Length);
            Assert.AreEqual(1, stockLocations.Length);
            Assert.AreEqual(material.Id.ToString(), materials[0].Value);
            Assert.AreEqual(stockLocation.Id.ToString(), stockLocations[0].Value);
            Assert.AreEqual(order.Id,
                ((CreateMaterialUsed)result.Model).WorkOrder);
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public void TestEditSetsViewData()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            operatingCenter.Contractors.Add(_currentUser.Contractor);
            Session.Merge(operatingCenter);
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = operatingCenter
            });
            var material = GetFactory<MaterialFactory>().Create();
            var stockLocation = GetFactory<StockLocationFactory>().Create(new { IsActive = true });
            operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = material});
            operatingCenter.StockLocations.Add(stockLocation);
            Session.Merge(operatingCenter);
            var materialUsed = GetFactory<MaterialUsedFactory>().Create(new {
                WorkOrder = order,
                Material = material,
                StockLocation = stockLocation,
                Quantity = 2
            });

            var result = (PartialViewResult)_target.Edit(materialUsed.Id);

            var materials =
                ((IEnumerable<SelectListItem>)result.ViewData[MaterialUsedController.VIEWDATA_MATERIALS]).ToArray();
            var stockLocations =
                ((IEnumerable<SelectListItem>)result.ViewData[MaterialUsedController.VIEWDATA_STOCK_LOCATIONS]).
                    ToArray();

            Assert.AreEqual(1, materials.Length);
            Assert.AreEqual(1, stockLocations.Length);
            Assert.AreEqual(material.Id.ToString(), materials[0].Value);
            Assert.AreEqual(stockLocation.Id.ToString(), stockLocations[0].Value);
        }

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            operatingCenter.Contractors.Add(_currentUser.Contractor);
            Session.Merge(operatingCenter);
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = operatingCenter
            });
            var materials = GetFactory<MaterialFactory>().CreateArray(2);
            var stockLocations = GetFactory<StockLocationFactory>().CreateArray(2);
            materials.Each(m =>
                operatingCenter.StockedMaterials.Add(
                    new OperatingCenterStockedMaterial {
                        OperatingCenter = operatingCenter,
                        Material = m
                    }));
            stockLocations.Each(l => operatingCenter.StockLocations.Add(l));
            Session.Merge(operatingCenter);
            var materialUsed = GetFactory<MaterialUsedFactory>().Create(new {
                WorkOrder = order,
                Material = materials[0],
                StockLocation = stockLocations[0],
                Quantity = 2
            });
            var model = _viewModelFactory.BuildWithOverrides<EditMaterialUsed, MaterialUsed>(materialUsed, new {
                Material = materials[1].Id,
                StockLocation = stockLocations[1].Id,
                Quantity = 1
            });

            var result = (PartialViewResult)_target.Update(model);
            var actual = (MaterialUsed)result.Model;

            Assert.AreEqual(materials[1], actual.Material);
            Assert.AreEqual(stockLocations[1], actual.StockLocation);
            Assert.AreEqual(1, actual.Quantity);
            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(materialUsed, result.Model);
        }

        [TestMethod]
        public void TestUpdateUpdatesMaterialUsedRecordWithGivenValuesToNonStock()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            operatingCenter.Contractors.Add(_currentUser.Contractor);
            Session.Merge(operatingCenter);
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = operatingCenter
            });
            var material = GetFactory<MaterialFactory>().Create();
            var stockLocation = GetFactory<StockLocationFactory>().Create();
            operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = material});
            operatingCenter.StockLocations.Add(stockLocation);
            Session.Merge(operatingCenter);
            var materialUsed = GetFactory<MaterialUsedFactory>().Create(new {
                WorkOrder = order,
                Material = material,
                StockLocation = stockLocation,
                Quantity = 2
            });

            var result = (PartialViewResult)_target.Update(_viewModelFactory.BuildWithOverrides<EditMaterialUsed, MaterialUsed>(materialUsed, new {
                StockLocation = (int?)null,
                Material = (int?)null,
                NonStockDescription = "some place",
                Quantity = 1
            }));
            var actual = (MaterialUsed)result.Model;

            Assert.IsNull(actual.StockLocation);
            Assert.IsNull(actual.Material);
            Assert.AreEqual("some place", actual.NonStockDescription);
            Assert.AreEqual(1, actual.Quantity);
            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(materialUsed, result.Model);
        }

        [TestMethod]
        public void TestUpdateUpdatesMaterialUsedRecordWithGivenValuesFromNonStock()
        {
            var operatingCenter = GetFactory<UniqueOperatingCenterFactory>().Create();
            operatingCenter.Contractors.Add(_currentUser.Contractor);
            Session.Merge(operatingCenter);
            var order = GetFactory<FinalizationWorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor,
                OperatingCenter = operatingCenter
            });
            var material = GetFactory<MaterialFactory>().Create();
            var stockLocation = GetFactory<StockLocationFactory>().Create();
            operatingCenter.StockedMaterials.Add(new OperatingCenterStockedMaterial { OperatingCenter = operatingCenter, Material = material});
            operatingCenter.StockLocations.Add(stockLocation);
            Session.Merge(operatingCenter);
            var materialUsed = GetFactory<MaterialUsedFactory>().Create(new {
                WorkOrder = order,
                NonStockDescription = "some place",
                Quantity = 2
            });

            var result = (PartialViewResult)_target.Update(_viewModelFactory.BuildWithOverrides<EditMaterialUsed, MaterialUsed>(materialUsed, new {
                NonStockDescription = (int?)null,
                Material = material.Id,
                StockLocation = stockLocation.Id,
                Quantity = 1
            }));

            var actual = Session.Get<MaterialUsed>(materialUsed.Id);

            Assert.IsTrue(string.IsNullOrEmpty(actual.NonStockDescription),
                $"Expected NonStockDescription to be null or empty but instead was '{actual.NonStockDescription}'");
            Assert.AreEqual(material, actual.Material);
            Assert.AreEqual(stockLocation, actual.StockLocation);
            Assert.AreEqual(1, actual.Quantity);
            Assert.AreEqual("_Show", result.ViewName);
            Assert.AreEqual(materialUsed, result.Model);
        }

        [TestMethod]
        public void TestUpdateReturnsEditViewWithModelLoadedIfModelStateIsNotValid()
        {
            var workOrder = GetFactory<WorkOrderFactory>().Create(new {
                AssignedContractor = _currentUser.Contractor
            });
            var materialUsed = GetFactory<MaterialUsedFactory>().Create(new {
                WorkOrder = workOrder
            });

            _target.ModelState.AddModelError("nope", "nuh uh");

            var result = (PartialViewResult)_target.Update(_viewModelFactory.Build<EditMaterialUsed, MaterialUsed>(materialUsed));
            var resultModel = (EditMaterialUsed)result.Model;

            Assert.AreEqual("_Edit", result.ViewName);
            Assert.AreEqual(materialUsed.Id, resultModel.Id);
        }

        #endregion
    }
}
