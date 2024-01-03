using System.Web.Mvc;
using MapCall.Common.Model.Entities;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using MapCallMVC.Controllers;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Results;
using MMSINC.Testing;
using MMSINC.Utilities.Documents;
using StructureMap;

namespace MapCallMVC.Tests.Controllers
{
    [TestClass]
    public class HelpTopicControllerTest : MapCallMvcControllerTestBase<HelpTopicController, HelpTopic>
    {
        #region Constants

        private const string HELP_TOPICS = "HelpTopics";

        #endregion

        #region Setup/Teardown

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use(ctx => ctx.GetInstance<InMemoryDocumentService>());
        }

        protected override void SetAutomatedTestOptions(AutomatedTestOptions options)
        {
            base.SetAutomatedTestOptions(options);
            // default DocumentStatus=Active on search causes the help topics without an active document to not so created a HelpTopic with an Active Document
            options.CreateValidEntity = () => {
                var dataType = GetFactory<DataTypeFactory>().Create(new { TableName = HELP_TOPICS });
                var documentType = GetFactory<DocumentTypeFactory>().Create(new { DataType = dataType });
                var documentStatus = GetFactory<DocumentStatusFactory>().Create();
                var document = GetFactory<DocumentFactory>().Create(new {
                    DocumentType = documentType,
                });
                var helpTopic = GetFactory<HelpTopicFactory>().Create();
                GetFactory<DocumentLinkFactory>().Create(new {
                    document.DocumentType,
                    document.DocumentType.DataType,
                    Document = document,
                    LinkedId = helpTopic.Id,
                    DocumentStatus = documentStatus,
                    ReviewFrequency = 1,
                    ReviewFrequencyUnit = GetFactory<YearlyRecurringFrequencyUnitFactory>().Create(),
                });
                return helpTopic;
            };
        }

        #endregion

        [TestMethod]
        public override void TestControllerAuthorization()
        {
            Authorization.Assert(a => {
                var module = RoleModules.FieldServicesDataLookups;
                a.RequiresLoggedInUserOnly("~/HelpTopic/Search/");
                a.RequiresLoggedInUserOnly("~/HelpTopic/Index/");
                a.RequiresLoggedInUserOnly("~/HelpTopic/Show/");
                a.RequiresRole("~/HelpTopic/Edit/", module, RoleActions.Edit);
                a.RequiresRole("~/HelpTopic/Update/", module, RoleActions.Edit);
                a.RequiresRole("~/HelpTopic/New/", module, RoleActions.Add);
                a.RequiresRole("~/HelpTopic/Create/", module, RoleActions.Add);
                a.RequiresRole("~/HelpTopic/Destroy/", module, RoleActions.Delete);
            });
        }

        #region Index

        [TestMethod]
        public void TestIndexXLSExportsExcel()
        {
            var entity0 = GetEntityFactory<HelpTopic>().Create(new {Description = "description 0"});
            var entity1 = GetEntityFactory<HelpTopic>().Create(new {Description = "description 1"});
            var search = new SearchHelpTopic();
            _target.ControllerContext = new ControllerContext();
            _target.RouteData.Values[ResponseFormatter.ROUTE_EXTENSION_PARAMETER_NAME] =
                ResponseFormatter.KnownExtensions.EXCEL_2003;

            var result = _target.Index(search) as ExcelResult;

            using (var helper = new ExcelResultTester(_container, result, true))
            {
                helper.AreEqual(entity0.Id, "Id");
                helper.AreEqual(entity1.Id, "Id", 1);
                helper.AreEqual(entity0.Description, "Description");
                helper.AreEqual(entity1.Description, "Description", 1);
            }
        }

        #endregion

        #region Edit/Update

        [TestMethod]
        public override void TestUpdateSavesChangesWhenModelStateIsValid()
        {
            var eq = GetEntityFactory<HelpTopic>().Create();
            var expected = "description field";

            var result = _target.Update(_viewModelFactory.BuildWithOverrides<EditHelpTopic, HelpTopic>(eq, new {
                Description = expected
            }));

            Assert.AreEqual(expected, Session.Get<HelpTopic>(eq.Id).Description);
        }

        #endregion
    }
}
