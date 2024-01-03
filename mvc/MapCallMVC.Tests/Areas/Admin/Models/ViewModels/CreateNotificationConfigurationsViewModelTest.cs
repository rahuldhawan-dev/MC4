using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.Admin.Models.ViewModels.NotificationConfigurations;

namespace MapCallMVC.Tests.Areas.Admin.Models.ViewModels
{
    // NOTE: This test can't use the ViewModelTestBase because it relies on ViewModelSet rather than ViewModel<T>.
    [TestClass]
    public class CreateNotificationConfigurationsTest : MapCallMvcInMemoryDatabaseTestBase<NotificationConfiguration>
    {
        #region Fields

        private CreateNotificationConfigurations _viewModel;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new CreateNotificationConfigurations(_container);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestItemsReturnsANotificationConfigurationForEachPossibleCombinationOfOperatingCenterAndContactWhenAppliesToAllOperatingCentersIsFalse()
        {
            var contact1 = GetEntityFactory<Contact>().Create();
            var contact2 = GetEntityFactory<Contact>().Create();
            var opc1 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var opc2 = GetFactory<UniqueOperatingCenterFactory>().Create();
            var purpose1 = GetEntityFactory<NotificationPurpose>().Create();
            var purpose2 = GetEntityFactory<NotificationPurpose>().Create();

            // This test should create *4* NotificationConfigurations. One for each contact and operating center.
            // Each configuration should include *both* purposes.

            _viewModel.Contacts = new[] { contact1.Id, contact2.Id };
            _viewModel.AppliesToAllOperatingCenters = false;
            _viewModel.OperatingCenters = new[] { opc1.Id, opc2.Id };
            _viewModel.NotificationPurposes = new[] { purpose1.Id, purpose2.Id };

            var result = _viewModel.Items.ToList();
            Assert.AreEqual(4, result.Count);

            Action<Contact, OperatingCenter> assertConfigCreated = (contact, opc) => {
                var config = result.Single(x => x.Contact == contact && x.OperatingCenter == opc);
                Assert.IsTrue(config.NotificationPurposes.Contains(purpose1));
                Assert.IsTrue(config.NotificationPurposes.Contains(purpose2));
            };

            assertConfigCreated(contact1, opc1);
            assertConfigCreated(contact1, opc2);
            assertConfigCreated(contact2, opc1);
            assertConfigCreated(contact2, opc2);
        }
        
        [TestMethod]
        public void TestItemsReturnsANotificationConfigurationForEachPossibleCombinationOfContactWhenAppliesToAllOperatingCentersIsTrue()
        {
            var contact1 = GetEntityFactory<Contact>().Create();
            var contact2 = GetEntityFactory<Contact>().Create();
            var opc1 = GetEntityFactory<OperatingCenter>().Create();
            var purpose1 = GetEntityFactory<NotificationPurpose>().Create();
            var purpose2 = GetEntityFactory<NotificationPurpose>().Create();

            // This test should create *2* NotificationConfigurations. One for each contact. OperatingCenter should be ignored even if it has values.
            // Each configuration should include *both* purposes.

            _viewModel.Contacts = new[] { contact1.Id, contact2.Id };
            _viewModel.AppliesToAllOperatingCenters = true;
            _viewModel.OperatingCenters = new[] { opc1.Id };
            _viewModel.NotificationPurposes = new[] { purpose1.Id, purpose2.Id };

            var result = _viewModel.Items.ToList();
            Assert.AreEqual(2, result.Count);

            Action<Contact, OperatingCenter> assertConfigCreated = (contact, opc) => {
                var config = result.Single(x => x.Contact == contact && x.OperatingCenter == null);
                Assert.IsTrue(config.NotificationPurposes.Contains(purpose1));
                Assert.IsTrue(config.NotificationPurposes.Contains(purpose2));
            };

            assertConfigCreated(contact1, opc1);
            assertConfigCreated(contact2, opc1);
        }

        #endregion
    }
}
