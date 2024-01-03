using System;
using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Migrations;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using MapCall.Common.Testing.Data;
using MapCallMVC.Areas.ProjectManagement.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Authentication;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Testing;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities;
using Moq;
using StructureMap;

namespace MapCallMVC.Tests.Areas.ProjectManagement.Models.ViewModels
{
    [TestClass]
    public class RecurringProjectViewModelTest : MapCallMvcInMemoryDatabaseTestBase<RecurringProject>
    {
        #region Fields

        private ViewModelTester<RecurringProjectViewModel, RecurringProject> _vmTester;
        private RecurringProjectViewModel _viewModel;
        private RecurringProject _entity;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = new RecurringProjectViewModel(_container);
            _entity = new RecurringProject();
            _vmTester = new ViewModelTester<RecurringProjectViewModel, RecurringProject>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestPropertiesThatCanMapBothWays()
        {
            _vmTester.CanMapBothWays(x => x.ProjectTitle);
            _vmTester.CanMapBothWays(x => x.ProjectDescription);
            _vmTester.CanMapBothWays(x => x.District);
            _vmTester.CanMapBothWays(x => x.OriginationYear);
            _vmTester.CanMapBothWays(x => x.HistoricProjectID);
            _vmTester.CanMapBothWays(x => x.NJAWEstimate);
            _vmTester.CanMapBothWays(x => x.ProposedLength);
            _vmTester.CanMapBothWays(x => x.Justification);
            _vmTester.CanMapBothWays(x => x.EstimatedProjectDuration);
            _vmTester.CanMapBothWays(x => x.EstimatedInServiceDate);
            _vmTester.CanMapBothWays(x => x.ActualInServiceDate);
            _vmTester.CanMapBothWays(x => x.FinalCriteriaScore);
            _vmTester.CanMapBothWays(x => x.FinalRawScore);
            _vmTester.CanMapBothWays(x => x.WBSNumber);
        }

        [TestMethod]
        public void TestRequiredFields()
        {
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.OperatingCenter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Town);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectTitle);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProjectDescription);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AssetCategory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AssetType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.NJAWEstimate);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.RecurringProjectType);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProposedLength);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProposedDiameter);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.ProposedPipeMaterial);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.AcceleratedAssetInvestmentCategory);
            ValidationAssert.PropertyIsRequired(_viewModel, x => x.Coordinate, "A coordinate is required when infomaster mains are not selected.");
        }

        [TestMethod]
        public void TestOperatingCenterCanMapBothWays()
        {
            var oc = GetEntityFactory<OperatingCenter>().Create(new {Description = "Foo"});
            _entity.OperatingCenter = oc;

            _vmTester.MapToViewModel();

            Assert.AreEqual(oc.Id, _viewModel.OperatingCenter);

            _entity.OperatingCenter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(oc, _entity.OperatingCenter);
        }

        [TestMethod]
        public void TestTownCanMapBothWays()
        {
            var town = GetEntityFactory<Town>().Create(new { ShortName = "Foo"});
            _entity.Town = town;

            _vmTester.MapToViewModel();

            Assert.AreEqual(town.Id, _viewModel.Town);

            _entity.Town = null;
            _vmTester.MapToEntity();

            Assert.AreSame(town, _entity.Town);
        }

        [TestMethod]
        public void TestRecurringProjectTypeCanMapBothWays()
        {
            var rpt = GetEntityFactory<RecurringProjectType>().Create(new {Description = "Foo"});
            _entity.RecurringProjectType = rpt;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rpt.Id, _viewModel.RecurringProjectType);

            _entity.RecurringProjectType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rpt, _entity.RecurringProjectType);
        }

        [TestMethod]
        public void TestPipeDiameterCanMapBothWays()
        {
            var pd = GetEntityFactory<PipeDiameter>().Create(new {Diameter = 5m});
            _entity.ProposedDiameter = pd;

            _vmTester.MapToViewModel();

            Assert.AreEqual(pd.Id, _viewModel.ProposedDiameter);

            _entity.ProposedDiameter = null;
            _vmTester.MapToEntity();

            Assert.AreSame(pd, _entity.ProposedDiameter);
        }

        [TestMethod]
        public void TestPipeMaterialCanMapBothWays()
        {
            var pm = GetEntityFactory<PipeMaterial>().Create(new {Description = "Foo"});
            _entity.ProposedPipeMaterial = pm;

            _vmTester.MapToViewModel();

            Assert.AreEqual(pm.Id, _viewModel.ProposedPipeMaterial);

            _entity.ProposedPipeMaterial = null;
            _vmTester.MapToEntity();

            Assert.AreSame(pm, _entity.ProposedPipeMaterial);
        }

        [TestMethod]
        public void TestSecondaryAssetInvestmentCategoryCanMapBothWays()
        {
            var aic = GetEntityFactory<AssetInvestmentCategory>().Create(new { Description = "Foo" });
            _entity.SecondaryAssetInvestmentCategory = aic;

            _vmTester.MapToViewModel();

            Assert.AreEqual(aic.Id, _viewModel.SecondaryAssetInvestmentCategory);

            _entity.SecondaryAssetInvestmentCategory = null;
            _vmTester.MapToEntity();

            Assert.AreSame(aic, _entity.SecondaryAssetInvestmentCategory);
        }

        [TestMethod]
        public void TestAcceleratedAssetInvestmentCategoryCanMapBothWays()
        {
            var aic = GetEntityFactory<AssetInvestmentCategory>().Create(new {Description = "Foo"});
            _entity.AcceleratedAssetInvestmentCategory = aic;

            _vmTester.MapToViewModel();

            Assert.AreEqual(aic.Id, _viewModel.AcceleratedAssetInvestmentCategory);

            _entity.AcceleratedAssetInvestmentCategory = null;
            _vmTester.MapToEntity();

            Assert.AreSame(aic, _entity.AcceleratedAssetInvestmentCategory);
        }

        [TestMethod]
        public void TestRecurringProjectStatusCanMapBothWays()
        {
            var rps = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            _entity.Status = rps;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rps.Id, _viewModel.Status);

            _entity.Status = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rps, _entity.Status);
        }

        [TestMethod]
        public void TestCoordinateCanMapBothWays()
        {
            var coord = GetEntityFactory<Coordinate>().Create();
            _entity.Coordinate = coord;

            _vmTester.MapToViewModel();

            Assert.AreEqual(coord.Id, _viewModel.Coordinate);

            _entity.Coordinate = null;
            _vmTester.MapToEntity();

            Assert.AreSame(coord, _entity.Coordinate);
        }

        [TestMethod]
        public void TestFoundationalFilingPeriodCanMapBothWays()
        {
            var ffp = GetEntityFactory<FoundationalFilingPeriod>().Create(new {Description = "Foo"});
            _entity.FoundationalFilingPeriod = ffp;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ffp.Id, _viewModel.FoundationalFilingPeriod);

            _entity.FoundationalFilingPeriod = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ffp, _entity.FoundationalFilingPeriod);
        }

        [TestMethod]
        public void TestAssetCategoryCanMapBothWays()
        {
            var ac = GetEntityFactory<AssetCategory>().Create(new {Description = "Foo"});
            _entity.AssetCategory = ac;

            _vmTester.MapToViewModel();

            Assert.AreEqual(ac.Id, _viewModel.AssetCategory);

            _entity.AssetCategory = null;
            _vmTester.MapToEntity();

            Assert.AreSame(ac, _entity.AssetCategory);
        }

        [TestMethod]
        public void TestAssetTypeCanMapBothWays()
        {
            var at = GetFactory<ValveAssetTypeFactory>().Create();
            _entity.AssetType = at;

            _vmTester.MapToViewModel();

            Assert.AreEqual(at.Id, _viewModel.AssetType);

            _entity.AssetType = null;
            _vmTester.MapToEntity();

            Assert.AreSame(at, _entity.AssetType);
        }

        [TestMethod]
        public void TestRecurringProjectRegulatoryStatusCanMapBothWays()
        {
            var rprs = GetEntityFactory<RecurringProjectRegulatoryStatus>().Create(new {Description = "Foo"});
            _entity.RegulatoryStatus = rprs;

            _vmTester.MapToViewModel();

            Assert.AreEqual(rprs.Id, _viewModel.RegulatoryStatus);

            _entity.RegulatoryStatus = null;
            _vmTester.MapToEntity();

            Assert.AreSame(rprs, _entity.RegulatoryStatus);
        }
        
        #endregion
    }

    [TestClass]
    public class CreateRecurringProjectTest : MapCallMvcInMemoryDatabaseTestBase<RecurringProject>
    {
        #region Fields

        private ViewModelTester<CreateRecurringProject, RecurringProject> _vmTester;
        private CreateRecurringProject _viewModel;
        private RecurringProject _entity;
        private User _user;
        private Mock<IDateTimeProvider> _dateTimeProvider;
        private Mock<IAuthenticationService<User>> _authServ;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {
            _user = GetFactory<UserFactory>().Create();
            _authServ = new Mock<IAuthenticationService<User>>();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _dateTimeProvider = new Mock<IDateTimeProvider>();
            _container.Inject(_authServ.Object);
            _container.Inject(_dateTimeProvider.Object);

            _viewModel = new CreateRecurringProject(_container);
            _entity = new RecurringProject();
            _vmTester = new ViewModelTester<CreateRecurringProject, RecurringProject>(_viewModel, _entity);
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestMapToEntityPopulatesPipeDataLookups()
        {
            var pipeDataValues = GetEntityFactory<PipeDataLookupValue>().CreateList(3, new {IsDefault = true, IsEnabled = true});
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            var _viewmodel = _viewModelFactory.Build<CreateRecurringProject, RecurringProject>(project);

            _viewmodel.MapToEntity(project);

            Assert.AreEqual(pipeDataValues.Count, _viewmodel.PipeDataLookupValues.Count);
        }

        [TestMethod]
        public void TestMapToEntityPopulatesHighCostFactors()
        {
            var highCostFactor = GetEntityFactory<HighCostFactor>().Create();
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            var _viewModel = _viewModelFactory.BuildWithOverrides<CreateRecurringProject, RecurringProject>(project, new { HighCostFactors = new List<int> {highCostFactor.Id} });

            _viewModel.MapToEntity(project);

            Assert.AreEqual(1, _viewModel.HighCostFactors.Count);
            Assert.AreEqual(highCostFactor.Id, _viewModel.HighCostFactors[0]);
        }

        [TestMethod]
        public void TestMapToEntityMapsGISDataInaccuracies()
        {
            var gisInaccuracyType = GetEntityFactory<GISDataInaccuracyType>().Create();
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            _viewModel = _viewModelFactory.BuildWithOverrides<CreateRecurringProject, RecurringProject>(project,
                new {GISDataInaccuracies = new List<int> {gisInaccuracyType.Id}});

            _viewModel.MapToEntity(project);

            Assert.AreEqual(1, _viewModel.GISDataInaccuracies.Count);
            Assert.AreEqual(gisInaccuracyType.Id, _viewModel.GISDataInaccuracies[0]);
        }

        [TestMethod]
        public void TestMapToEntitySetsMainBreakOrders()
        {
            var order = GetEntityFactory<WorkOrder>().Create();
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            _viewModel =
                _viewModelFactory.BuildWithOverrides<CreateRecurringProject, RecurringProject>(project,
                    new {MainBreakOrders = new List<int> {order.Id}});

            _viewModel.MapToEntity(project);

            Assert.AreEqual(1, _viewModel.MainBreakOrders.Count);
            Assert.AreEqual(order.Id, _viewModel.MainBreakOrders[0]);
        }

        [TestMethod]
        public void TestMapToEntitySetsStatusToProposed()
        {
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            var _viewModel = _viewModelFactory.Build<CreateRecurringProject, RecurringProject>(project);

            _viewModel.MapToEntity(project);

            Assert.AreEqual(RecurringProjectStatus.Indices.PROPOSED, _viewModel.Status);
        }

        [TestMethod]
        public void TestMapToEntitySetsStatusToCompleteIfActualInServiceDateExists()
        {
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            var _viewModel = _viewModelFactory.BuildWithOverrides<CreateRecurringProject, RecurringProject>(project, new {ActualInServiceDate = DateTime.Now});

            _viewModel.MapToEntity(project);

            Assert.AreEqual(RecurringProjectStatus.Indices.COMPLETE, _viewModel.Status);
        }

        [TestMethod]
        public void TestOverrideSetsNotificationToTrue()
        {
            var reasons = GetEntityFactory<OverrideInfoMasterReason>().CreateList(3);
            foreach (var reason in reasons)
            {
                var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies(new { OverrideInfoMasterReason = reason });
                var _viewModel = _viewModelFactory.BuildWithOverrides<CreateRecurringProject, RecurringProject>(project, new { ActualInServiceDate = DateTime.Now });
                
                _viewModel.MapToEntity(project);

                if (reason.Id == OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT)
                {
                    Assert.IsTrue(_viewModel.SendGISDataIncorrectOnSave);
                }
                else
                {
                    Assert.IsFalse(_viewModel.SendGISDataIncorrectOnSave);
                }
            }
        }

        [TestMethod]
        public void TestValidateRequiresMainsHaveBeenAdded()
        {
            var recurringProject = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            var target = _viewModelFactory.Build<CreateRecurringProject, RecurringProject>(recurringProject);

            ValidationAssert.ModelStateHasError(target, x => x.RecurringProjectMains, CreateRecurringProject.NO_MAINS);

            target.RecurringProjectMains.Add(new CreateRecurringProjectMain(_container));

            ValidationAssert.ModelStateIsValid(target);
        }

        [TestMethod]
        public void TestMapToEntitySetsOverrideMasterDecisionToTrueWhenTotalScoreisLessThen2Point5()
        {
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();

            var _viewModel = _viewModelFactory.Build<CreateRecurringProject, RecurringProject>(project);

            project.TotalInfoMasterScore = "1";

            _viewModel.MapToEntity(project);

            Assert.IsTrue(_viewModel.OverrideInfoMasterDecision.Value);
        }

        [TestMethod]
        public void TestMapToEntitySetsOverrideMasterDecisionToNullWhenTotalScoreisGreaterThen2Point5()
        {
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();

            var _viewModel = _viewModelFactory.Build<CreateRecurringProject, RecurringProject>(project);

            project.TotalInfoMasterScore = "3";

            _viewModel.MapToEntity(project);


            Assert.IsNull(_viewModel.OverrideInfoMasterDecision);
        }

        [TestMethod]
        public void TestMapToEntitySetsOverrideMasterDecisionToTrueWhenTotalScoreisEqualTo2Point5()
        {
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();

            var _viewModel = _viewModelFactory.Build<CreateRecurringProject, RecurringProject>(project);

            project.TotalInfoMasterScore = "2.5";

            _viewModel.MapToEntity(project);

            Assert.IsTrue(_viewModel.OverrideInfoMasterDecision.Value);
        }

        #endregion
    }

    [TestClass]
    public class EditRecurringProjectTest : MapCallMvcInMemoryDatabaseTestBase<RecurringProject>
    {
        #region Fields

        private ViewModelTester<EditRecurringProject, RecurringProject> _vmTester;
        private EditRecurringProject _viewModel;
        private RecurringProject _entity;
        private RecurringProjectRepository _repository;
        private Mock<IAuthenticationService<User>> _authServ;
        private User _user;

        #endregion

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            _authServ = e.For<IAuthenticationService<User>>().Mock();
            e.For<IEstimatingProjectRepository>().Use<EstimatingProjectRepository>();
            e.For<IRecurringProjectRepository>().Use<RecurringProjectRepository>();
            e.For<IOperatingCenterRepository>().Use<OperatingCenterRepository>();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _viewModel = _viewModelFactory.Build<EditRecurringProject>();
            _entity = new RecurringProject();
            _vmTester = new ViewModelTester<EditRecurringProject, RecurringProject>(_viewModel, _entity);
            _user = GetFactory<UserFactory>().Create();
            _authServ.Setup(x => x.CurrentUser).Returns(_user);
            _repository = _container.GetInstance<RecurringProjectRepository>();
        }

        #endregion

        #region Tests

        [TestMethod]
        public void TestChangingToCompleteFromNothingSetsNotificationToTrue()
        {
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var projectStatuses = new[] {projectStatusPending, projectStatusComplete};
            var project = GetEntityFactory<RecurringProject>().Create();
            _viewModel = _viewModelFactory.Build<EditRecurringProject, RecurringProject>(project);

            foreach (var projectStatus in projectStatuses)
            {
                _viewModel.Status = projectStatus.Id;
                _viewModel.MapToEntity(project);
                if (projectStatus.Id==RecurringProjectStatus.Indices.COMPLETE)
                    Assert.IsTrue(_viewModel.SendNotificationOnSave);
                else
                    Assert.IsFalse(_viewModel.SendNotificationOnSave);

                //reset it for the loop
                _viewModel.SendNotificationOnSave = false;
            }
        }

        [TestMethod]
        public void TestChangingFromSomethingThatIsNotCompleteToCompleteSetsNotificationToTrue()
        {
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var projectStatuses = new[] { projectStatusPending, projectStatusComplete };


            foreach (var projectStatus in projectStatuses)
            {
                if (projectStatus.Id != RecurringProjectStatus.Indices.COMPLETE)
                {
                    var project = GetEntityFactory<RecurringProject>().Create(new {Status = projectStatus});
                    _viewModel = _viewModelFactory.Build<EditRecurringProject, RecurringProject>(project);
                    _viewModel.Status = RecurringProjectStatus.Indices.COMPLETE;
                    _viewModel.MapToEntity(project);
                    Assert.IsTrue(_viewModel.SendNotificationOnSave);
                }
            }
        }

        [TestMethod]
        public void TestChangingFromCompleteToCompleteSetsNotificationToFalse()
        {
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var projectStatuses = new[] { projectStatusPending, projectStatusComplete };
            var project = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusComplete });
            Session.Flush();
            Session.Clear();
            _viewModel = _viewModelFactory.Build<EditRecurringProject, RecurringProject>(project);
            _viewModel.Status = RecurringProjectStatus.Indices.COMPLETE;

            _viewModel.MapToEntity(project);

            Assert.IsFalse(_viewModel.SendNotificationOnSave);
        }

        [TestMethod]
        public void TestChangingFromNoOverrideToGisOverrideSetsNotificationToTrue()
        {
            var reasons = GetEntityFactory<OverrideInfoMasterReason>().CreateList(3);
            
            var project = GetEntityFactory<RecurringProject>().Create();
            _viewModel = _viewModelFactory.Build<EditRecurringProject, RecurringProject>(project);
            _viewModel.OverrideInfoMasterReason = OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT;
            _viewModel.MapToEntity(project);

            Assert.IsTrue(_viewModel.SendGISDataIncorrectOnSave);
        }

        [TestMethod]
        public void TestChangingFromOtherOverrideToGisOverrideSetsNotificationToTrue()
        {
            var reasons = GetEntityFactory<OverrideInfoMasterReason>().CreateList(3);
            foreach (var reason in reasons)
            {
                var project = GetEntityFactory<RecurringProject>().Create(new { OverrideInfoMasterReason = reason});
                _viewModel = _viewModelFactory.Build<EditRecurringProject, RecurringProject>(project);
                _viewModel.OverrideInfoMasterReason = OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT;
                _viewModel.MapToEntity(project);

                if (reason.Id != OverrideInfoMasterReason.Indices.GIS_DATA_INCORRECT)
                {
                    Assert.IsTrue(_viewModel.SendGISDataIncorrectOnSave);
                }
                else
                {
                    Assert.IsFalse(_viewModel.SendGISDataIncorrectOnSave);
                }
            }
        }

        [TestMethod]
        public void TestMapToEntityPopulatesHighCostFactors()
        {
            var highCostFactor = GetEntityFactory<HighCostFactor>().Create();
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            _viewModel = _viewModelFactory.BuildWithOverrides<EditRecurringProject, RecurringProject>(project, new { HighCostFactors = new List<int>() { highCostFactor.Id } });

            _viewModel.MapToEntity(project);

            Assert.AreEqual(1, _viewModel.HighCostFactors.Count);
            Assert.AreEqual(highCostFactor.Id, _viewModel.HighCostFactors[0]);
        }

        [TestMethod]
        public void TestMapToEntitySetsToCompletedIfActualInServiceDateExists()
        {
            var project = GetEntityFactory<RecurringProject>().BuildWithConcreteDependencies();
            _viewModel = _viewModelFactory.BuildWithOverrides<EditRecurringProject, RecurringProject>(project, new { ActualInServiceDate = DateTime.Now });

            _viewModel.MapToEntity(project);

            Assert.AreEqual(RecurringProjectStatus.Indices.COMPLETE, _viewModel.Status);
        }

        [TestMethod]
        public void TestValidateRequiresMainsHaveBeenAddedWhenIdIsGreaterThanX()
        {
            var recurringProject = GetFactory<RecurringProjectFactory>().Create();
            var target = _viewModelFactory.Build<EditRecurringProject, RecurringProject>(recurringProject);

            ValidationAssert.ModelStateIsValid(target);

            target.Id = EditRecurringProject.MAIN_CUTOFF_ID+1;

            ValidationAssert.ModelStateHasError(target, x => x.RecurringProjectMains, EditRecurringProject.NO_MAINS);

            target.RecurringProjectMains.Add(new EditRecurringProjectMain(_container));

            ValidationAssert.ModelStateIsValid(target);
        }

        [TestMethod]
        public void TestChangingStatusToAPApprovedUpdatesScores()
        {
            _user.IsAdmin = true;
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var projectStatusAPApproved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
            var projectStatuses = new[] { projectStatusPending, projectStatusComplete, projectStatusAPApproved };
            var project = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusComplete });
            var pdlv1 = GetEntityFactory<PipeDataLookupValue>().Create(new { VariableScore = 10m, PriorityWeightedScore = 15m });
            var pdlv2 = GetEntityFactory<PipeDataLookupValue>().Create(new { VariableScore = 20m, PriorityWeightedScore = 30m });
            project.PipeDataLookupValues.Add(pdlv1);
            project.PipeDataLookupValues.Add(pdlv2);
            _viewModel = _viewModelFactory.BuildWithOverrides<EditRecurringProject, RecurringProject>(project, new {Status = RecurringProjectStatus.Indices.AP_APPROVED});
            _viewModel.MapToEntity(project);

            Assert.AreEqual(15, _viewModel.FinalCriteriaScore);
            Assert.AreEqual(22.5m, _viewModel.FinalRawScore);
        }

        [TestMethod]
        public void TestChangingStatusDoesNotUpdatesScores()
        {
            _user.IsAdmin = true;
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var projectStatusAPApproved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
            var projectStatuses = new[] { projectStatusPending, projectStatusComplete, projectStatusAPApproved };
            var project = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusAPApproved, FinalCriteriaScore = 15m, FinalRawScore = 22.5m });
            _viewModel = _viewModelFactory.BuildWithOverrides<EditRecurringProject, RecurringProject>(project, new { Status = RecurringProjectStatus.Indices.COMPLETE });
            _viewModel.MapToEntity(project);

            Assert.AreEqual(15, _viewModel.FinalCriteriaScore);
            Assert.AreEqual(22.5m, _viewModel.FinalRawScore);
        }

        [TestMethod]
        public void TestChangingStatusDoesNotUpdatesScoresPartDuex()
        {
            _user.IsAdmin = true;
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();
            var projectStatusAPApproved = GetFactory<APApprovedRecurringProjectStatusFactory>().Create();
            var projectStatuses = new[] { projectStatusPending, projectStatusComplete, projectStatusAPApproved };
            var project = GetEntityFactory<RecurringProject>().Create(new { Status = projectStatusPending });
            _viewModel = _viewModelFactory.BuildWithOverrides<EditRecurringProject, RecurringProject>(project, new { Status = RecurringProjectStatus.Indices.COMPLETE });
            _viewModel.MapToEntity(project);

            Assert.IsNull(_viewModel.FinalCriteriaScore);
            Assert.IsNull(_viewModel.FinalRawScore);
        }

        [TestMethod]
        public void TestWBSNumberIsRequiredWhenStatusIsCompleteButOnlyWhenNOTContractedOperations()
        {
            var opcNotContracted = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = false });
            var opcContracted = GetFactory<UniqueOperatingCenterFactory>().Create(new { IsContractedOperations = true });
            var projectStatusComplete = GetFactory<CompleteRecurringProjectStatusFactory>().Create();
            var projectStatusPending = GetFactory<ProposedRecurringProjectStatusFactory>().Create();


            //_viewModel.Status = projectStatusComplete.Id;
            //_viewModel.WBSNumber = null;
            //_viewModel.OperatingCenter = opcContracted.Id;
            //ValidationAssert.ModelStateHasError(_viewModel, x => x.WBSNumber, "THe WBS Charged field is required.");

            // Should be valid because status is not set yet.
            _viewModel.Status = null;
            _viewModel.WBSNumber = null;
            _viewModel.OperatingCenter = opcNotContracted.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.WBSNumber);

            // Should be valid because status is not "Complete"
            _viewModel.Status = projectStatusPending.Id;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.WBSNumber);

            // Should not be valid because opc is not contracted and status is "Complete"
            _viewModel.Status = projectStatusComplete.Id;
            ValidationAssert.ModelStateHasError(_viewModel, x => x.WBSNumber, "The WBS Charged field is required.");

            // Should be valid because opc is not contracted, status is complete, and WBS has value
            _viewModel.WBSNumber = "123";
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.WBSNumber);

            // Should be valid because it is contracted operations, status is complete, and wbs is not set
            _viewModel.OperatingCenter = opcContracted.Id;
            _viewModel.WBSNumber = null;
            ValidationAssert.ModelStateIsValid(_viewModel, x => x.WBSNumber);
        }

        [TestMethod]
        public void TestMapSetsGISInaccuracies()
        {
            var gisInaccuracyType = GetEntityFactory<GISDataInaccuracyType>().Create();
            _entity.GISDataInaccuracies.Add(gisInaccuracyType);

            _viewModel.Map(_entity);

            Assert.AreEqual(_viewModel.GISDataInaccuracies[0], gisInaccuracyType.Id);
        }

        [TestMethod]
        public void TestMapSetsHighCostFactors()
        {
            var hcFactor = GetEntityFactory<HighCostFactor>().Create();
            _entity.HighCostFactors.Add(hcFactor);

            _viewModel.Map(_entity);

            Assert.AreEqual(_viewModel.HighCostFactors[0], hcFactor.Id);
        }

        [TestMethod]
        public void TestMapSetsMainBreakOrders()
        {
            var order = GetEntityFactory<WorkOrder>().Create();
            _entity.MainBreakOrders.Add(order);

            _viewModel.Map(_entity);

            Assert.AreEqual(_viewModel.MainBreakOrders[0], order.Id);
        }

        [TestMethod]
        public void TestMapSetsOverRideMasterDecisionToTrueWhenInfoScoreisLessThan2Point5()
        {
            _entity.TotalInfoMasterScore = "1";

            _viewModel.Map(_entity);

            Assert.IsTrue(_viewModel.OverrideInfoMasterDecision.Value);

        }

        [TestMethod]
        public void TestMapSetsOverRideMasterDecisionToNullWhenInfoScoreIsGreaterThen2Point5()
        {
            _entity.TotalInfoMasterScore = "5";

            _viewModel.Map(_entity);

            Assert.IsNull(_viewModel.OverrideInfoMasterDecision);

        }

        [TestMethod]
        public void TestMapSetsOverRideMasterDecisionToTrueWhenInfoScoreisEqulto2Point5()
        {
            _entity.TotalInfoMasterScore = "2.5";

            _viewModel.Map(_entity);

            Assert.IsTrue(_viewModel.OverrideInfoMasterDecision.Value);
        }

        #endregion
    }

}

