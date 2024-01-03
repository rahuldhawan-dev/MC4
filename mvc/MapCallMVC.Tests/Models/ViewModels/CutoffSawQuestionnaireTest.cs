using MMSINC.Authentication;
using MMSINC.Data.NHibernate;
using MMSINC.Testing;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Models.ViewModels
{
    [TestClass]
    public class CutoffSawQuestionnaireTest : MapCallMvcInMemoryDatabaseTestBase<CutoffSawQuestionnaire>
    {
        #region Private

        private CutoffSawQuestionnaire _entity;
        private CutoffSawQuestionnaireViewModel _target;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;
        private ViewModelTester<CutoffSawQuestionnaireViewModel, CutoffSawQuestionnaire> _vmTester;
        protected Mock<IDateTimeProvider> _dateTimeProvider;

        #endregion
        
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<ICutoffSawQuestionRepository>().Use<CutoffSawQuestionRepository>();
            e.For<IWorkOrderRepository>().Use<WorkOrderRepository>();
            e.For<IRepository<WorkOrder>>().Use(ctx => ctx.GetInstance<IWorkOrderRepository>());
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IAuthenticationService<User>>().Use((_authServ = new Mock<IAuthenticationService<User>>()).Object);
            e.For<IDateTimeProvider>().Use((_dateTimeProvider = new Mock<IDateTimeProvider>()).Object);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _entity = GetFactory<CutoffSawQuestionnaireFactory>().Create(new { OperatedOn = Lambdas.GetNow()});
            _target = new CreateCutoffSawQuestionnaire(_container);
            _vmTester = new ViewModelTester<CutoffSawQuestionnaireViewModel, CutoffSawQuestionnaire>(_target, _entity);

            _user = new User { UserName = "mcadmin", IsAdmin = true };
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
        }

        #endregion

        #region Mapping

        [TestMethod]
        public void TestMappings()
        {
            _vmTester.CanMapToViewModel(x => x.Id, 13);
            _vmTester.CanMapBothWays(x => x.WorkOrderSAP, "huh?", "what?");
            _vmTester.CanMapBothWays(x => x.Comments, "ooo", "aaa");
        }

        [TestMethod]
        public void TestViewModelMapSetsPropertiesAndIds()
        {
            var lead = GetFactory<EmployeeFactory>().Create();
            var workorder = GetFactory<WorkOrderFactory>().Create();
            var sawperator = GetFactory<EmployeeFactory>().Create();
            var material = GetFactory<PipeMaterialFactory>().Create();
            var diameter = GetFactory<PipeDiameterFactory>().Create();

            var questionnaire = GetFactory<CutoffSawQuestionnaireFactory>().Create(new {
                LeadPerson = lead,
                SawOperator = sawperator, 
                WorkOrder = workorder,
                PipeMaterial = material,
                PipeDiameter = diameter
            });

            var target = new CreateCutoffSawQuestionnaire(_container);

            target.Map(questionnaire);
            Assert.AreEqual(lead.Id, target.LeadPerson);
            Assert.AreEqual(workorder.Id, target.WorkOrderID);
            Assert.AreEqual(sawperator.Id, target.SawOperator);
            Assert.AreEqual(material.Id, target.PipeMaterial);
            Assert.AreEqual(diameter.Id, target.PipeDiameter);
        }

        [TestMethod]
        public void TestViewModelMapToEntitySetsProperties()
        {
            var questionnaire = GetFactory<CutoffSawQuestionnaireFactory>().Create();
            var target = new CreateCutoffSawQuestionnaire(_container);
            var questions = GetFactory<CutoffSawQuestionFactory>().CreateList(8);
            foreach (var q in questions)
            {
                target.CutoffSawQuestions.Add(_viewModelFactory.Build<CutoffSawQuestionViewModel, CutoffSawQuestion>(q));
            }

            target.MapToEntity(questionnaire);

            Assert.AreEqual(questions.Count, questionnaire.CutoffSawQuestions.Count);
            Assert.AreEqual(_user.UserName, questionnaire.CreatedBy);
        }

        #endregion

        #region Validation

        [TestMethod]
        public void TestValidatingOnAllOfTheThingsToEnsureThatEachForeignKeyPropertyWouldMapToAnExistingObject()
        {
            ValidationAssert.EntityMustExist(_target, x => x.WorkOrderID, GetFactory<WorkOrderFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.LeadPerson, GetFactory<EmployeeFactory>().Create());
            ValidationAssert.EntityMustExist(_target, x => x.SawOperator, GetFactory<EmployeeFactory>().Create());
        }

        [TestMethod]
        public void TestForeignKeyPropertiesThatAreNullableAllowNulls()
        {
            _target.WorkOrderID = null;
            _target.PipeMaterial = null;
            _target.PipeDiameter = null;

            ValidationAssert.ModelStateIsValid(_target, "WorkOrderID");
            ValidationAssert.ModelStateIsValid(_target, "PipeMaterialId");
            ValidationAssert.ModelStateIsValid(_target, "PipeDiameterId");
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_target, x => x.LeadPerson, "The Lead Person field is required.");
            ValidationAssert.PropertyIsRequired(_target, x => x.SawOperator, "The Saw Operator field is required.");
            ValidationAssert.PropertyIsRequired(_target, x => x.OperatedOn);
        }

        [TestMethod]
        public void TestActiveQuestionIsAddedBeforeThePostAddsValidationError()
        {
            var lead = GetFactory<EmployeeFactory>().Create();
            var sawp = GetFactory<EmployeeFactory>().Create();
            var questions = GetFactory<CutoffSawQuestionFactory>().CreateList(3);
            var target = new CreateCutoffSawQuestionnaire(_container) {
                LeadPerson = lead.Id,
                SawOperator = sawp.Id, 
                OperatedOn = Lambdas.GetNow(),
                Agree = true
            };
            var newQuestion = GetFactory<CutoffSawQuestionFactory>().Create();

            ValidationAssert.ModelStateHasError(target, string.Empty, CreateCutoffSawQuestionnaire.QUESTIONS_CHANGED);
        }

        [TestMethod]
        public void TestActiveQuestionIsRemovedBeforeThePostAddsValidationError()
        {
            var lead = GetFactory<EmployeeFactory>().Create();
            var sawp = GetFactory<EmployeeFactory>().Create();
            var questions = GetFactory<CutoffSawQuestionFactory>().CreateList(3);
            questions[1].IsActive = false;
            _container.GetInstance<ICutoffSawQuestionRepository>().Save(questions[1]);
            
            var target = new CreateCutoffSawQuestionnaire
           (_container) {
                LeadPerson = lead.Id,
                SawOperator = sawp.Id,
                OperatedOn = Lambdas.GetNow(),
                Agree = true
            };
            var newQuestion = GetFactory<CutoffSawQuestionFactory>().Create();

            ValidationAssert.ModelStateHasError(target, string.Empty, CreateCutoffSawQuestionnaire.QUESTIONS_CHANGED);
        }

        #endregion
    }
}
