using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalNonComplianceEvents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace MapCallMVC.Tests.Areas.Environmental.Models.ViewModels

{
    [TestClass]
    public class EnvironmentalNonComplianceEventTest : ViewModelTestBase<EnvironmentalNonComplianceEvent, EnvironmentalNonComplianceEventViewModel>
    {
        #region Validations

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist(x => x.State, GetEntityFactory<State>().Create());
            ValidationAssert.EntityMustExist(x => x.OperatingCenter, GetEntityFactory<OperatingCenter>().Create());
            ValidationAssert.EntityMustExist(x => x.WaterType, GetEntityFactory<WaterType>().Create());
            ValidationAssert.EntityMustExist(x => x.PublicWaterSupply, GetEntityFactory<PublicWaterSupply>().Create());
            ValidationAssert.EntityMustExist(x => x.WasteWaterSystem, GetEntityFactory<WasteWaterSystem>().Create());
            ValidationAssert.EntityMustExist(x => x.Facility, GetEntityFactory<Facility>().Create());
            ValidationAssert.EntityMustExist(x => x.IssueType, GetEntityFactory<EnvironmentalNonComplianceEventType>().Create());
            ValidationAssert.EntityMustExist(x => x.IssueSubType, GetEntityFactory<EnvironmentalNonComplianceEventSubType>().Create());
            ValidationAssert.EntityMustExist(x => x.Responsibility,
                GetEntityFactory<EnvironmentalNonComplianceEventResponsibility>().Create());            
            ValidationAssert.EntityMustExist(x => x.FailureType, GetEntityFactory<EnvironmentalNonComplianceEventFailureType>().Create());
            ValidationAssert.EntityMustExist(x => x.IssueStatus, GetEntityFactory<EnvironmentalNonComplianceEventStatus>().Create());
            ValidationAssert.EntityMustExist(x => x.CountsAgainstTarget, GetEntityFactory<EnvironmentalNonComplianceEventCountsAgainstTarget>().Create());
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.State);
            _vmTester.CanMapBothWays(x => x.OperatingCenter);
            _vmTester.CanMapBothWays(x => x.PublicWaterSupply);
            _vmTester.CanMapBothWays(x => x.WasteWaterSystem);
            _vmTester.CanMapBothWays(x => x.Facility);
            _vmTester.CanMapBothWays(x => x.IssueType);
            _vmTester.CanMapBothWays(x => x.IssueSubType);
            _vmTester.CanMapBothWays(x => x.Responsibility);
            _vmTester.CanMapBothWays(x => x.FailureType);
            _vmTester.CanMapBothWays(x => x.IssueStatus);
            _vmTester.CanMapBothWays(x => x.IssuingEntity);
            _vmTester.CanMapBothWays(x => x.EventDate);
            _vmTester.CanMapBothWays(x => x.DateFinalized);
            _vmTester.CanMapBothWays(x => x.EnforcementDate);
            _vmTester.CanMapBothWays(x => x.AwarenessDate);
            _vmTester.CanMapBothWays(x => x.SummaryOfEvent);
            _vmTester.CanMapBothWays(x => x.NameOfThirdParty);
            _vmTester.CanMapBothWays(x => x.FailureTypeDescription);
            _vmTester.CanMapBothWays(x => x.FineAmount);
            _vmTester.CanMapBothWays(x => x.NameOfEntity);
            _vmTester.CanMapBothWays(x => x.IssueYear);
            _vmTester.CanMapBothWays(x => x.NOVWorkGroupReviewDate);
            _vmTester.CanMapBothWays(x => x.ChiefEnvOfficerApprovalDate);
            _vmTester.CanMapBothWays(x => x.CountsAgainstTarget);
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.State);
            ValidationAssert.PropertyIsRequired(x => x.WaterType);
            ValidationAssert.PropertyIsRequired(x => x.IssueType);
            ValidationAssert.PropertyIsRequired(x => x.RootCauses);
            ValidationAssert.PropertyIsRequired(x => x.IssueStatus);
            ValidationAssert.PropertyIsRequired(x => x.EventDate);
            ValidationAssert.PropertyIsRequired(x => x.AwarenessDate);
            ValidationAssert.PropertyIsRequired(x => x.SummaryOfEvent);
            ValidationAssert.PropertyIsRequired(x => x.Responsibility);

            ValidationAssert.PropertyIsRequiredWhen(x => x.PublicWaterSupply, GetFactory<PublicWaterSupplyFactory>().Create().Id, x => x.WaterType, WaterType.Indices.WATER, WaterType.Indices.WASTEWATER);
            ValidationAssert.PropertyIsRequiredWhen(x => x.WasteWaterSystem, GetFactory<WasteWaterSystemFactory>().Create().Id, x => x.WaterType, WaterType.Indices.WASTEWATER, WaterType.Indices.WATER);
            ValidationAssert.PropertyIsRequiredWhen(x => x.FailureType, GetFactory<EnvironmentalNonComplianceEventFailureTypeFactory>().Create().Id, x => x.DateFinalized, DateTime.Today, null);
            ValidationAssert.PropertyIsRequiredWhen(x => x.FailureTypeDescription, "Failure Description", x => x.FailureType, 1, null);
            ValidationAssert.PropertyIsRequiredWhen(x => x.IssuingEntity, GetFactory<EnvironmentalNonComplianceEventEntityLevelFactory>().Create().Id, x => x.IssueStatus, EnvironmentalNonComplianceEventStatus.Indices.CONFIRMED);
            ValidationAssert.PropertyIsRequiredWhen(x => x.NameOfThirdParty, "Anyone", x => x.Responsibility, EnvironmentalNonComplianceEventResponsibility.Indices.THIRD_PARTY);
            ValidationAssert.PropertyIsRequiredWhen(x => x.NameOfEntity, "Anyone", x => x.IssuingEntity, EnvironmentalNonComplianceEventEntityLevel.Indices.OTHER);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.NameOfThirdParty, EnvironmentalNonComplianceEvent.StringLengths.RESPONSIBILITY_NAME);
            ValidationAssert.PropertyHasMaxStringLength(x => x.FailureTypeDescription, EnvironmentalNonComplianceEvent.StringLengths.FAILURE_TYPE_DESCRIPTION);
            ValidationAssert.PropertyHasMaxStringLength(x => x.NameOfEntity, EnvironmentalNonComplianceEvent.StringLengths.ISSUING_ENTITY_NAME);
        }

        [TestMethod]
        public void TestRootCauseCanMapBothWays()
        {
            var rootCauses = GetEntityFactory<EnvironmentalNonComplianceEventRootCause>().CreateList(4);
            _entity.RootCauses = new List<EnvironmentalNonComplianceEventRootCause>();
            _entity.RootCauses.Add(rootCauses[0]);
            _entity.RootCauses.Add(rootCauses[2]);

            _viewModel.Map(_entity);

            Assert.AreEqual(2, _viewModel.RootCauses.Length);
            Assert.AreEqual(_entity.RootCauses[0].Id, _viewModel.RootCauses[0]);
            Assert.AreEqual(_entity.RootCauses[1].Id, _viewModel.RootCauses[1]);
        }

        #endregion
    }
}
