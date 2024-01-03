using System.Web.UI.WebControls;
using MMSINC.DataPages.Permissions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MMSINC.DataPages;

namespace MMSINC.Core.WebFormsTest.DataPages
{
    /// <summary>
    /// Summary description for DetailsViewCrudTemplateTest
    /// </summary>
    [TestClass]
    public class DetailsViewCrudTemplateTest
    {
        #region Fields

        private Mock<IDataPagePermissions> _mockPermissions;
        private Mock<IPermission> _mockDeletePermission;
        private Mock<IPermission> _mockEditPermission;

        #endregion

        [TestInitialize]
        public void DetailsViewCrudTemplateTestInitialize()
        {
            _mockPermissions = new Mock<IDataPagePermissions>();
            _mockDeletePermission = new Mock<IPermission>();
            _mockEditPermission = new Mock<IPermission>();

            _mockPermissions.SetupGet(x => x.DeleteAccess).Returns(_mockDeletePermission.Object);
            _mockPermissions.SetupGet(x => x.EditAccess).Returns(_mockEditPermission.Object);
        }

        private DetailsViewCrudTemplate Initialize(DetailsViewCrudTemplateType templateType)
        {
            return new DetailsViewCrudTemplate(_mockPermissions.Object, templateType);
        }

        [TestMethod]
        public void TestConstructorSetsPagePermissionsPropertyToPassedInArgument()
        {
            var target = Initialize(DetailsViewCrudTemplateType.EditTemplate);
            Assert.AreSame(_mockPermissions.Object, target.PagePermissions);
        }

        [TestMethod]
        public void TestConstructorSetsTemplateTypePropertyToPassedInArgument()
        {
            var target = Initialize(DetailsViewCrudTemplateType.InsertTemplate);
            Assert.AreEqual(DetailsViewCrudTemplateType.InsertTemplate,
                target.TemplateType);
        }

        private static void VerifyButton(LinkButton button, bool expectedIsEnabled, bool expectedCausesValidation,
            string expectedCommandName)
        {
            Assert.AreEqual(expectedIsEnabled, button.Enabled);
            Assert.AreEqual(expectedIsEnabled, button.Visible);
            Assert.AreEqual(expectedCausesValidation, button.CausesValidation);
            Assert.AreEqual(expectedCommandName, button.CommandName); // CommandName and Text use the same parameter.
            Assert.AreEqual(expectedCommandName, button.Text);
        }

        [TestMethod]
        public void TestBuildButtonCreatesLinkButtonAndSetsPropertiesBasedOnArguments()
        {
            var expectedIsEnabled = true;
            var expectedCausesValidation = false;
            var expectedCommandName = "DOOOOOOOOOOOOOOOM!";

            var resultButton =
                DetailsViewCrudTemplate.BuildButton(expectedIsEnabled,
                    expectedCausesValidation, expectedCommandName);

            VerifyButton(resultButton, expectedIsEnabled, expectedCausesValidation, expectedCommandName);
        }

        [TestMethod]
        public void TestCreateButtonSpacerReturnsLiteralWithTextThatHasOneSpaceCharacterOnly()
        {
            var expected = " ";
            var result = DetailsViewCrudTemplate.CreateButtonSpacer();

            Assert.AreEqual(result.Text, expected);
        }

        [TestMethod]
        public void TestCreateCancelButtonSetsExpectedValues()
        {
            var expectedIsEnabled = true;
            var expectedCausesValidation = false;
            var expectedCommandName = DetailsViewCrudTemplate.COMMANDS.CANCEL;

            var target = Initialize(DetailsViewCrudTemplateType.InsertTemplate);
            var resultButton = target.CreateCancelButton();

            VerifyButton(resultButton, expectedIsEnabled, expectedCausesValidation, expectedCommandName);
        }

        [TestMethod]
        public void TestCreateDeleteButtonReturnsButtonWhenPermissionsCanDeleteRecordsEqualsTrue()
        {
            _mockDeletePermission.SetupGet(x => x.IsAllowed).Returns(true);

            var expectedIsEnabled = true;
            var expectedCausesValidation = false;
            var expectedCommandName = DetailsViewCrudTemplate.COMMANDS.DELETE;

            var target = Initialize(DetailsViewCrudTemplateType.InsertTemplate);
            var resultButton = target.CreateDeleteButton();

            VerifyButton(resultButton, expectedIsEnabled, expectedCausesValidation, expectedCommandName);

            Assert.AreEqual(resultButton.OnClientClick,
                DetailsViewCrudTemplate.DELETE_CONFIRM);

            _mockPermissions.Verify();
        }

        [TestMethod]
        public void TestCreateDeleteButtonReturnsInvisibleButtonWhenPermissionsCanDeleteRecordsEqualsFalse()
        {
            _mockDeletePermission.SetupGet(x => x.IsAllowed).Returns(false);

            var target = Initialize(DetailsViewCrudTemplateType.ItemTemplate);
            var result = target.CreateDeleteButton();

            Assert.IsFalse(result.Visible);
            _mockPermissions.Verify();
        }

        [TestMethod]
        public void TestCreateEditButtonReturnsButtonWhenPermissionsCanEditRecordsEqualsTrue()
        {
            _mockEditPermission.SetupGet(x => x.IsAllowed).Returns(true);

            var expectedIsEnabled = true;
            var expectedCausesValidation = false;
            var expectedCommandName = DetailsViewCrudTemplate.COMMANDS.EDIT;

            var target = Initialize(DetailsViewCrudTemplateType.InsertTemplate);
            var resultButton = target.CreateEditButton();

            VerifyButton(resultButton, expectedIsEnabled, expectedCausesValidation, expectedCommandName);

            _mockPermissions.Verify();
        }

        [TestMethod]
        public void TestCreateEditButtonReturnsInvisibleDisabledButtonWhenPermissionsCanEditRecordsEqualsFalse()
        {
            _mockEditPermission.SetupGet(x => x.IsAllowed).Returns(false);

            var target = Initialize(DetailsViewCrudTemplateType.ItemTemplate);
            var resultButton = target.CreateEditButton();

            Assert.AreEqual(resultButton.Visible, false);
            Assert.AreEqual(resultButton.Enabled, false);

            _mockPermissions.Verify();
        }

        [TestMethod]
        public void TestCreateInsertButtonReturnsButtonWithExpectedPropertyValuesSet()
        {
            var expectedIsEnabled = true;
            var expectedCausesValidation = true;
            var expectedCommandName = DetailsViewCrudTemplate.COMMANDS.INSERT;

            var target = Initialize(DetailsViewCrudTemplateType.InsertTemplate);
            var resultButton = target.CreateInsertButton();

            VerifyButton(resultButton, expectedIsEnabled, expectedCausesValidation, expectedCommandName);
        }

        [TestMethod]
        public void TestCreateUpdateButtonReturnsButtonWithExpectedPropertyValuesSet()
        {
            var expectedIsEnabled = true;
            var expectedCausesValidation = true;
            var expectedCommandName = DetailsViewCrudTemplate.COMMANDS.UPDATE;

            var target = Initialize(DetailsViewCrudTemplateType.InsertTemplate);
            var resultButton = target.CreateUpdateButton();

            VerifyButton(resultButton, expectedIsEnabled, expectedCausesValidation, expectedCommandName);
        }
    }
}
