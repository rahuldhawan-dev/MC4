using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCallMVC.Areas.Facilities.Models.ViewModels.Easements;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCallMVC.Tests.Areas.Facilities.Models.ViewModels
{
    public abstract class EasementViewModelTest<TViewModel> : ViewModelTestBase<Easement, TViewModel>
        where TViewModel : EasementViewModel
    {
        #region Validations

        [TestMethod]
        public override void TestEntityMustExistValidation()
        {
            ValidationAssert.EntityMustExist<Town>(x => x.Town)
                            .EntityMustExist<TownSection>(x => x.TownSection)
                            .EntityMustExist<Street>(x => x.Street)
                            .EntityMustExist<Street>(x => x.CrossStreet)
                            .EntityMustExist<Coordinate>(x => x.Coordinate)
                            .EntityMustExist<EasementStatus>(x => x.Status)
                            .EntityMustExist<EasementCategory>(x => x.Category)
                            .EntityMustExist<EasementReason>(x => x.Reason)
                            .EntityMustExist<EasementType>(x => x.Type)
                            .EntityMustExist<GrantorType>(x => x.GrantorType);
        }

        [TestMethod]
        public override void TestPropertiesCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.StreetNumber);
            _vmTester.CanMapBothWays(x => x.RecordNumber);
            _vmTester.CanMapBothWays(x => x.Wbs);
            _vmTester.CanMapBothWays(x => x.EasementDescription);
            _vmTester.CanMapBothWays(x => x.DateRecorded);
            _vmTester.CanMapBothWays(x => x.DeedBook);
            _vmTester.CanMapBothWays(x => x.DeedPage);
            _vmTester.CanMapBothWays(x => x.BlockLot);
            _vmTester.CanMapBothWays(x => x.OwnerName);
            _vmTester.CanMapBothWays(x => x.OwnerAddress);
            _vmTester.CanMapBothWays(x => x.OwnerPhone);
            _vmTester.CanMapBothWays(x => x.Town, GetEntityFactory<Town>().Create());
            _vmTester.CanMapBothWays(x => x.TownSection, GetEntityFactory<TownSection>().Create());
            _vmTester.CanMapBothWays(x => x.Street, GetEntityFactory<Street>().Create());
            _vmTester.CanMapBothWays(x => x.CrossStreet, GetEntityFactory<Street>().Create());
            _vmTester.CanMapBothWays(x => x.Coordinate, GetEntityFactory<Coordinate>().Create());
            _vmTester.CanMapBothWays(x => x.Status, GetEntityFactory<EasementStatus>().Create());
            _vmTester.CanMapBothWays(x => x.Category, GetEntityFactory<EasementCategory>().Create());
            _vmTester.CanMapBothWays(x => x.Reason, GetEntityFactory<EasementReason>().Create());
            _vmTester.CanMapBothWays(x => x.Type, GetEntityFactory<EasementType>().Create());
            _vmTester.CanMapBothWays(x => x.GrantorType, GetEntityFactory<GrantorType>().Create());
        }

        [TestMethod]
        public override void TestRequiredValidation()
        {
            ValidationAssert.PropertyIsRequired(x => x.Town)
                            .PropertyIsRequired(x => x.Coordinate)
                            .PropertyIsRequired(x => x.RecordNumber)
                            .PropertyIsRequired(x => x.Status)
                            .PropertyIsRequired(x => x.Category)
                            .PropertyIsRequired(x => x.EasementDescription)
                            .PropertyIsRequired(x => x.Type)
                            .PropertyIsRequired(x => x.DateRecorded)
                            .PropertyIsRequired(x => x.GrantorType);
        }

        [TestMethod]
        public override void TestStringLengthValidation()
        {
            ValidationAssert.PropertyHasMaxStringLength(x => x.RecordNumber, Easement.StringLengths.RECORD_NUMBER);
        }

        #endregion
    }

    [TestClass]
    public class CreateEasementTest : EasementViewModelTest<CreateEasement> { }
}
