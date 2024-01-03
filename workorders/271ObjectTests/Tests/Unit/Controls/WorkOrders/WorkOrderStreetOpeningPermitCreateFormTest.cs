using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using LINQTo271.Controls.WorkOrders;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Common;
using MMSINC.Controls;
using MMSINC.Data.WebApi;
using MMSINC.Interface;
using MMSINC.Testing.DesignPatterns;
using MMSINC.Testing.MSTest;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using Permits.Data.Client;
using Permits.Data.Client.Entities;
using Rhino.Mocks;
using StructureMap;
using WorkOrders;
using WorkOrders.Library.Permissions;
using WorkOrders.Model;
using County = Permits.Data.Client.Entities.County;
using State = Permits.Data.Client.Entities.State;

namespace _271ObjectTests.Tests.Unit.Controls.WorkOrders
{
    /// <summary>
    /// http://localhost:44148/Views/WorkOrders/PermitProcessing/PermitProcessingResourceViewRPCPage.aspx?cmd=view&arg=3
    /// </summary>
    [TestClass]
    public class WorkOrderStreetOpeningPermitCreateFormTest : EventFiringTestClass
    {
        #region Private Members

        private IDropDownList _ddlForms;
        private IButton _btnCreate;
        private IPlaceHolder _phPermitForm, _phPaymentForm;
        private MvpPanel _pnlSelect, _pnlForm, _pnlDrawings, _pnlPayment, _pnlSuccess;
        private IRepository<State> _stateRepository;
        private IRepository<County> _countyRepository;
        private IRepository<Municipality> _municipalityRepository;
        private IRepository<Permit> _permitRepository;
        private IRepository<Payment> _paymentRepository;
        private MMSINC.Data.Linq.IRepository<StreetOpeningPermit> _sopRepository;
        private MMSINC.Data.Linq.IRepository<WorkOrder> _woRepository;
        private IRequest _iRequest;
        private ILabel _lblFlash;
        private IPage _iPage;
        private IViewState _viewState;
        private IDateTimeProvider _dateTimeProvider;
        private IHiddenField _hidPermitId;
        private IObjectDataSource _iOdsStreetOpeningPermits;
        private ISecurityService _securityService;

        private TestWorkOrderStreetOpeningPermitCreateForm _target;
        private ParameterCollection _updateParams;
        private IContainer _container;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public override void EventFiringTestClassInitialize()
        {
            base.EventFiringTestClassInitialize();
            _container = new Container();

            _mocks
                .DynamicMock(out _ddlForms)
                .DynamicMock(out _btnCreate)
                .DynamicMock(out _phPermitForm)
                .DynamicMock(out _phPaymentForm)
                .DynamicMock(out _pnlSelect)
                .DynamicMock(out _pnlForm)
                .DynamicMock(out _stateRepository)
                .DynamicMock(out _countyRepository)
                .DynamicMock(out _municipalityRepository)
                .DynamicMock(out _permitRepository)
                .DynamicMock(out _paymentRepository)
                .DynamicMock(out _iRequest)
                .DynamicMock(out _lblFlash)
                .DynamicMock(out _iPage)
                .DynamicMock(out _sopRepository)
                .DynamicMock(out _woRepository)
                .DynamicMock(out _dateTimeProvider)
                .DynamicMock(out _hidPermitId)
                .DynamicMock(out _iOdsStreetOpeningPermits)
                .DynamicMock(out _securityService);

            _target = new TestWorkOrderStreetOpeningPermitCreateFormBuilder()
                .WithDdlForms(_ddlForms)
                .WithBtnCreate(_btnCreate)
                .WithPhPermitForm(_phPermitForm)
                .WithPhPaymentForm(_phPaymentForm)
                //.WithPnlSelect(_pnlSelect)
                //.WithPnlForm(_pnlForm)
                .WithStateRepository(_stateRepository)
                .WithCountyRepository(_countyRepository)
                .WithMunicipalityRepository(_municipalityRepository)
                .WithPermitRepository(_permitRepository)
                .WithPaymentRepository(_paymentRepository)
                .WithIRequest(_iRequest)
                .WithLblFlash(_lblFlash)
                .WithIPage(_iPage)
                .WithHidPermitId(_hidPermitId)
                .WithObjectDataSource(_iOdsStreetOpeningPermits)
                .WithSecurityService(_securityService);

            AddPanels();

            _updateParams = new ParameterCollection {
                new Parameter(WorkOrderStreetOpeningPermitCreateForm.Params.WORKORDER_ID),
                new Parameter(WorkOrderStreetOpeningPermitCreateForm.Params.PERMIT_ID),
                new Parameter(WorkOrderStreetOpeningPermitCreateForm.Params.HAS_MET_DRAWING_REQUIREMENTS),
                new Parameter(WorkOrderStreetOpeningPermitCreateForm.Params.IS_PAID_FOR)
            };

            _container.Inject(_woRepository);
            _container.Inject(_sopRepository);
            _container.Inject(_dateTimeProvider);

            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }

        [TestCleanup]
        public override void EventFiringTestClassCleanup()
        {
            base.EventFiringTestClassCleanup();
        }

        private void AddPanels()
        {
            _pnlSelect = new MvpPanel { ID = "pnlSelect", Visible = true };
            _target.SetPnlSelect(_pnlSelect);
            _target.Controls.Add(_pnlSelect);

            _pnlForm = new MvpPanel { ID = "pnlForm", Visible = false };
            _target.SetPnlForm(_pnlForm);
            _target.Controls.Add(_pnlForm);

            _pnlDrawings = new MvpPanel { ID = "pnlDrawings", Visible = false };
            _target.SetPnlDrawings(_pnlDrawings);
            _target.Controls.Add(_pnlDrawings);

            _pnlPayment = new MvpPanel { ID = "pnlPayment", Visible = false };
            _target.SetPnlPayment(_pnlPayment);
            _target.Controls.Add(_pnlPayment);

            _pnlSuccess = new MvpPanel { ID = "pnlSuccess", Visible = false };
            _target.SetPnlSuccess(_pnlSuccess);
            _target.Controls.Add(_pnlSuccess);
        }

        private void SetUpdateParams(Permit permit)
        {
            _updateParams[WorkOrderStreetOpeningPermitCreateForm.Params.WORKORDER_ID].DefaultValue = _target.WorkOrderID.ToString();
            _updateParams[WorkOrderStreetOpeningPermitCreateForm.Params.PERMIT_ID].DefaultValue = permit.Id.ToString();
            _updateParams[WorkOrderStreetOpeningPermitCreateForm.Params.HAS_MET_DRAWING_REQUIREMENTS].DefaultValue = permit.HasMetDrawingRequirement.ToString();
            _updateParams[WorkOrderStreetOpeningPermitCreateForm.Params.IS_PAID_FOR].DefaultValue = permit.IsPaidFor.ToString();
        }

        private void AssertPanelVisible(IPanel expectedPanel)
        {
            var panels = new[] {
                _pnlSelect,
                _pnlForm, 
                _pnlDrawings,
                _pnlPayment,
                _pnlSuccess
            };
            foreach (var panel in panels)
            {
                if (panel.ID == expectedPanel.ID)
                    Assert.IsTrue(panel.Visible);
                else
                    Assert.IsFalse(panel.Visible);
            }
        }

        #endregion

        #region Property Tests

        [TestMethod]
        public void TestFormIdReturnsValueOfddlForms()
        {
            var expected = "99";

            using (_mocks.Record())
            {
                SetupResult.For(_ddlForms.SelectedValue).Return(expected);
            }
            using (_mocks.Playback())
            {
                Assert.AreEqual(expected, _target.FormId);
            }
        }

        [TestMethod]
        public void TestSetStateSetsNameToUseCultureTitleCase()
        {
            var state = "StaTe";
            
            _target.State = state;

            Assert.AreEqual(state.ToCultureTitleCase(), _target.State);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSetCountySetsNameToUseCultureTitleCase()
        {
            var county = "CoUnTY of the NaME";

            _target.County = county;

            Assert.AreEqual(county.ToCultureTitleCase(), _target.County);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestSetMunicipalitySetsNameToUseCultureTitleCase()
        {
            var muni = "MuNiCiPaLiTy";

            _target.Municipality = muni;

            Assert.AreEqual(muni.ToCultureTitleCase(), _target.Municipality);

            _mocks.ReplayAll();
        }

        [TestMethod]
        public void TestPermitsUserNameReturnsPermitUserNameFromSecurityService()
        {
            var expected = "foo";
            var employee = new Employee {
                DefaultOperatingCenter = new OperatingCenter {
                    PermitsOMUserName = expected
                }
            };

            Expect.Call(_securityService.Employee).Return(employee);
            _mocks.ReplayAll();

            Assert.AreEqual(expected, _target.PermitsUserName);
        }

        #endregion

        #region Event Handlers

        #region Intialization
       
        [TestMethod]
        public void TestOnInitResetslblFlashAndAddsCssLink()
        {
            var lbl = new MvpLabel { Text = "Test" };
            _target.SetLblFlash(lbl);

            using (_mocks.Record())
            {
                lbl.Text = string.Empty;
            }
            using (_mocks.Playback())
            {
                _target.TestOnInit(new EventArgs());
                _iPage.AssertWasCalled(x => x.AddHeaderControl(Arg<LiteralControl>.
                    Matches(lc => lc.Text == String.Format(WorkOrderStreetOpeningPermitCreateForm.CSS_FORMAT, Utilities.BaseAddress))));
                _mocks.ReplayAll();
            }
        }

        [TestMethod]
        public void TestOnDataBindingChecksQueryStringAndDisplaysPnlSelectWhenPermitIdNotPresent()
        {
            _target.WorkOrderID = 808;
            var qs = _mocks.DynamicMock<IQueryString>();

            using(_mocks.Record())
            {
                SetupResult.For(_iRequest.IQueryString).Return(qs);
            }

            using(_mocks.Playback())
            {
                InvokeEventByName(_target, "OnDataBinding", new object[] { new EventArgs() });
                AssertPanelVisible(_pnlSelect);
            }
        }

        [TestMethod]
        public void TestOnDataBindingChecksQueryStringAndDisplaysPnlSelectWhenPostBack()
        {
            _target.WorkOrderID = 808;
            var qs = _mocks.DynamicMock<IQueryString>();

            using (_mocks.Record())
            {
                SetupResult.For(_iPage.IsPostBack).Return(true);
                SetupResult.For(_iRequest.IQueryString).Return(qs);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnDataBinding", new object[] { new EventArgs() });
                AssertPanelVisible(_pnlSelect);
            }
        }

        [TestMethod]
        public void TestOnDataBindingChecksQueryStringAndDisplaysPnlDrawingWhenPermitIdPresentAndHasNotMetDrawingRequirements()
        {
            var permitId = 2;
            var permit = new Permit { Id = permitId, HasMetDrawingRequirement = false, IsPaidFor = false };
            new Page().Controls.Add(_target); 
            _target.WorkOrderID = 808;
            var qs = _mocks.DynamicMock<IQueryString>();

            using (_mocks.Record())
            {
                SetupResult.For(_iRequest.IQueryString).Return(qs);
                SetupResult.For(qs.GetValue<int?>(
                    WorkOrderStreetOpeningPermitCreateForm.QueryStringParams.PERMIT_ID))
                        .Return(permitId);
                _hidPermitId.Value = permitId.ToString();
                SetupResult.For(_iOdsStreetOpeningPermits.UpdateParameters).Return(_updateParams);
                SetupResult.For(_iOdsStreetOpeningPermits.Update()).Return(1);
                SetupResult.For(_permitRepository.Find(permitId)).Return(permit);
                SetUpdateParams(permit);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnDataBinding", new object[] { new EventArgs() });
                AssertPanelVisible(_pnlDrawings);
            }
        }

        [TestMethod]
        public void TestOnDataBindingChecksQueryStringAndDisplaysPnlDrawingWhenPermitIdPresentAndHasMetDrawingRequirements()
        {
            var permitId = 2;
            var permit = new Permit { Id = permitId, HasMetDrawingRequirement = true, IsPaidFor = false };
            var ctrls = new ControlCollection(new PlaceHolder());
            new Page().Controls.Add(_target);
            _target.WorkOrderID = 808;
            var qs = _mocks.DynamicMock<IQueryString>();
            var payment = "payment";

            using (_mocks.Record())
            {
                SetupResult.For(_phPaymentForm.Controls).Return(ctrls);
                SetupResult.For(_iRequest.IQueryString).Return(qs);
                SetupResult.For(qs.GetValue<int?>(
                    WorkOrderStreetOpeningPermitCreateForm.QueryStringParams.PERMIT_ID))
                        .Return(permitId);
                _hidPermitId.Value = permitId.ToString();
                SetupResult.For(_paymentRepository.New(new NameValueCollection { { "permitId", permitId.ToString() } })).Return(payment).IgnoreArguments();
                SetupResult.For(_iOdsStreetOpeningPermits.UpdateParameters).Return(_updateParams);
                SetupResult.For(_iOdsStreetOpeningPermits.Update()).Return(1);
                SetupResult.For(_permitRepository.Find(permitId)).Return(permit);
                SetUpdateParams(permit);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "OnDataBinding", new object[] { new EventArgs() });
                AssertPanelVisible(_pnlPayment);
                Assert.AreEqual(payment, ((Label)ctrls[0]).Text);
            }
        }

        #endregion

        #region Forms

        [TestMethod]
        public void TestddlFormsDataBindingAddsAllFormIdsToddlForms()
        {
            var stateName = "New Jersey";
            var state = new State { FormId = 1, Id = 1, Name = stateName };
            var states = new List<State> { state };
            var county = new County { FormId = 2, Id = 11, Name = "Monmouth" };
            var counties = new List<County> { county };
            var muni = new Municipality { FormId = 3, Id = 111, Name = "Aberdeen" };
            var munis = new List<Municipality> { muni };
            var items = new ListItemCollection();
            _target.State = stateName;

            using(_mocks.Record())
            {
                SetupResult.For(_stateRepository.Search((NameValueCollection)null)).Return(states).IgnoreArguments();
                SetupResult.For(_countyRepository.Search((NameValueCollection)null)).Return(counties).IgnoreArguments();
                SetupResult.For(_municipalityRepository.Search((NameValueCollection)null)).Return(munis).IgnoreArguments();
                SetupResult.For(_ddlForms.Items).Return(items);
                _target.StateId = state.Id;
                _target.CountyId = county.Id;
                _target.MunicipalityId = muni.Id;
                _btnCreate.Visible = true;
            }

            using(_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlForms_DataBinding");
                Assert.AreEqual(items[2].Text, state.Name);
                Assert.AreEqual(items[2].Value, state.FormId.ToString());
                Assert.AreEqual(items[1].Text, county.Name);
                Assert.AreEqual(items[1].Value, county.FormId.ToString());
                Assert.AreEqual(items[0].Text, muni.Name);
                Assert.AreEqual(items[0].Value, muni.FormId.ToString());
            }
        }

        [TestMethod]
        public void TestddlFormsDataBindingDisplaysErrorWhenNoFormsAreLoaded()
        {
            _target.State = "NJ";
            _target.County = "Monmouth";
            _target.Municipality = "Aberdeen";
            using (_mocks.Record())
            {
                SetupResult.For(_stateRepository.Search((NameValueCollection)null)).Return(null).IgnoreArguments();
                SetupResult.For(_countyRepository.Search((NameValueCollection)null)).Return(null).IgnoreArguments();
                SetupResult.For(_municipalityRepository.Search((NameValueCollection)null)).Return(null).IgnoreArguments();
                SetupResult.For(_ddlForms.Items).Return(new ListItemCollection());
                _lblFlash.Text = String.Format(WorkOrderStreetOpeningPermitCreateForm.NO_FORMS, _target.State, _target.County, _target.Municipality);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "ddlForms_DataBinding");
            }
        }

        [TestMethod]
        public void TestOnCreateLoadsPermitFormFromRepositoryAndAddsToView()
        {
            var expected = "an html form";
            var ctrls = new ControlCollection(new PlaceHolder());
            new Page().Controls.Add(_target);
            _target.WorkOrderID = 1234;
            var workOrder = new WorkOrder
            {
                Street = new Street(),
                Town = new Town
                {
                    County = new global::WorkOrders.Model.County()
                    {
                        State = new global::WorkOrders.Model.State()
                    }
                },
                WorkDescription = new WorkDescription { Description = "INSTALL SERVICE" }
            };

            using (_mocks.Record())
            {
                SetupResult.For(_woRepository.Get(1234)).Return(workOrder);
                SetupResult.For(_permitRepository.New()).Return(expected).IgnoreArguments();
                SetupResult.For(_phPermitForm.Controls).Return(ctrls);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "On_Create");
                Assert.AreEqual(expected, ((Label)ctrls[0]).Text);
                AssertPanelVisible(_pnlForm);
            }
        }

        #endregion

        #region Submit Form
        
        [TestMethod]
        public void TestOnSubmitSendsRequestFormToPermitRepositoryAndSaves()
        {
            var permitId = 1;
            var ctrls = new ControlCollection(new PlaceHolder());
            var permit = new Permit {Id = permitId, HasMetDrawingRequirement = false, IsPaidFor = false };
            var form = new NameValueCollection();
            new Page().Controls.Add(_target);
            _target.WorkOrderID = 1234;

            using (_mocks.Record())
            {
                SetupResult.For(_iRequest.Form).Return(form);
                SetupResult.For(_permitRepository.Save(form)).Return(permit);
                SetupResult.For(_phPermitForm.Controls).Return(ctrls);
                _sopRepository.InsertNewEntity(null);
                LastCall.IgnoreArguments();
                _hidPermitId.Value = permit.Id.ToString();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "On_Submit");
                AssertPanelVisible(_pnlDrawings);
                Assert.AreEqual(String.Format("Success: PermitID {0}", permit.Id), ((Label)ctrls[0]).Text);
            }
        }

        /// <summary>
        /// Test it saves the returned permit information into the 271 database
        /// </summary>
        [TestMethod]
        public void TestOnSubmitSendsRequestFormToPermitRepositoryAndSavesANewPermit()
        {
            new Page().Controls.Add(_target);
            _target.WorkOrderID = 1234;
            var permit = new Permit { Id = 1, HasMetDrawingRequirement = false, IsPaidFor = false };
            var form = new NameValueCollection();
            var date = DateTime.Now;

            Expect.Call(_dateTimeProvider.GetCurrentDate()).Return(date);
            Expect.Call(_iRequest.Form).Return(form);
            Expect.Call(_phPermitForm.Controls).Return(new ControlCollection(new PlaceHolder()));
            Expect.Call(_permitRepository.Save(form)).Return(permit);
            _mocks.ReplayAll();

            InvokeEventByName(_target, "On_Submit");
            
            _sopRepository.AssertWasCalled(
                x => x.InsertNewEntity(Arg<StreetOpeningPermit>.Matches(p => 
                                        p.PermitId == permit.Id && 
                                        p.WorkOrderID == _target.WorkOrderID &&
                                        p.DateRequested == date &&
                                        p.HasMetDrawingRequirement == permit.HasMetDrawingRequirement &&
                                        p.IsPaidFor == permit.IsPaidFor &&
                                        p.StreetOpeningPermitNumber == string.Empty)));
            // if this isn't done it tries to verifyall for stuff created with the assertwascalled line
            _mocks.ReplayAll(); 
        }

        [TestMethod]
        public void TestOnSubmitSucceedsForPermitThatHasMetDrawingRequirementsAndDisplaysPaymentPanel()
        {
            var permitId = 1;
            var ctrls = new ControlCollection(new PlaceHolder());
            var paymentCtrls = new ControlCollection(new PlaceHolder());
            var permit = new Permit { Id = permitId, HasMetDrawingRequirement = true, IsPaidFor = false };
            var form = new NameValueCollection();
            new Page().Controls.Add(_target);
            _target.WorkOrderID = 1234;
            var payment = "payment";

            using (_mocks.Record())
            {
                SetupResult.For(_iRequest.Form).Return(form);
                SetupResult.For(_phPaymentForm.Controls).Return(paymentCtrls);
                SetupResult.For(_permitRepository.Save(form)).Return(permit);
                SetupResult.For(_phPermitForm.Controls).Return(ctrls);
                SetupResult.For(_paymentRepository.New()).Return(payment).IgnoreArguments();
                _sopRepository.InsertNewEntity(null);
                LastCall.IgnoreArguments();
                _hidPermitId.Value = permit.Id.ToString();
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "On_Submit");
                Assert.AreEqual(String.Format("Success: PermitID {0}", permit.Id), ((Label)ctrls[0]).Text);
                AssertPanelVisible(_pnlPayment);
                Assert.AreEqual(payment, ((Label)paymentCtrls[0]).Text);
            }
        }

        #endregion

        #region Drawings

        [TestMethod]
        public void TestOnDrawingUploadedTransfersToPaymentPanelAndAddsForm()
        {
            var permitId = 1;
            var ctrls = new ControlCollection(new PlaceHolder());
            var permit = new Permit { Id = permitId, HasMetDrawingRequirement = true, IsPaidFor = false };
            new Page().Controls.Add(_target);
            _target.WorkOrderID = 1234;
            var payment = "payment";

            using(_mocks.Record())
            {
                SetupResult.For(_phPaymentForm.Controls).Return(ctrls);
                SetupResult.For(_iOdsStreetOpeningPermits.UpdateParameters).Return(_updateParams);
                SetupResult.For(_iOdsStreetOpeningPermits.Update()).Return(1);
                SetupResult.For(_hidPermitId.Value).Return(permitId.ToString());
                SetupResult.For(_permitRepository.Find(permitId)).Return(permit);
                SetupResult.For(_paymentRepository.New(new NameValueCollection {{ "permitId", permitId.ToString()}})).Return(payment).IgnoreArguments();
                _hidPermitId.Value = permit.Id.ToString();
                SetUpdateParams(permit);
            }

            using(_mocks.Playback())
            {
                InvokeEventByName(_target, "On_DrawingUploaded");
                AssertPanelVisible(_pnlPayment);
                Assert.AreEqual(payment, ((Label)ctrls[0]).Text);
            }
        }

        [TestMethod]
        public void TestOnDrawingUploadedTransfersToDrawingPanelIfHasNotMetDrawingRequirement()
        {
            var permitId = 1;
            var permit = new Permit { Id = permitId, HasMetDrawingRequirement = false, IsPaidFor = false };
            _target.WorkOrderID = 1234;
            new Page().Controls.Add(_target);

            //permitId, workorder, permitRepository.Find(permitId) 
            using (_mocks.Record())
            {
                SetupResult.For(_iOdsStreetOpeningPermits.UpdateParameters).
                    Return(_updateParams);
                SetupResult.For(_iOdsStreetOpeningPermits.Update()).Return(1);
                SetupResult.For(_hidPermitId.Value).Return(permitId.ToString());
                SetupResult.For(_permitRepository.Find(permitId)).Return(permit);
                SetUpdateParams(permit);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "On_DrawingUploaded");
                AssertPanelVisible(_pnlDrawings);
            }
        }

        #endregion

        #region Payment

        [TestMethod]
        public void TestSuccessfulPayment()
        {
            var permitId = 1;
            var ctrls = new ControlCollection(new PlaceHolder());
            var permit = new Permit { Id = permitId, HasMetDrawingRequirement = true, IsPaidFor = true };
            _target.WorkOrderID = 1234;
            new Page().Controls.Add(_target);
            var payment = "payment";

            using (_mocks.Record())
            {
                SetupResult.For(_phPaymentForm.Controls).Return(ctrls);
                SetupResult.For(_iOdsStreetOpeningPermits.UpdateParameters).Return(_updateParams);
                SetupResult.For(_iOdsStreetOpeningPermits.Update()).Return(1);
                SetupResult.For(_hidPermitId.Value).Return(permitId.ToString());
                SetupResult.For(_paymentRepository.Save(null)).IgnoreArguments();
                SetupResult.For(_permitRepository.Find(permitId)).Return(permit);
                SetUpdateParams(permit);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "On_SubmitPayment");
                AssertPanelVisible(_pnlSuccess);
            }
        }

        [TestMethod]
        public void TestUnsuccessfulPayment()
        {
            var permitId = 1;
            var ctrls = new ControlCollection(new PlaceHolder());
            var permit = new Permit { Id = permitId, HasMetDrawingRequirement = true, IsPaidFor = false };
            _target.WorkOrderID = 1234;
            new Page().Controls.Add(_target);
            var payment = "payment";

            using (_mocks.Record())
            {
                SetupResult.For(_phPaymentForm.Controls).Return(ctrls);
                SetupResult.For(_iOdsStreetOpeningPermits.UpdateParameters).Return(_updateParams);
                SetupResult.For(_iOdsStreetOpeningPermits.Update()).Return(1);
                SetupResult.For(_hidPermitId.Value).Return(permitId.ToString());
                SetupResult.For(_paymentRepository.Save(null)).IgnoreArguments();
                SetupResult.For(_permitRepository.Find(permitId)).Return(permit);
                SetupResult.For(_paymentRepository.New(new NameValueCollection { { "permitId", permitId.ToString() } })).Return(payment).IgnoreArguments();
                ctrls.Add(new Label { Text = payment });
                _hidPermitId.Value = permit.Id.ToString();
                SetUpdateParams(permit);
            }

            using (_mocks.Playback())
            {
                InvokeEventByName(_target, "On_SubmitPayment");
                AssertPanelVisible(_pnlPayment);
            }
        }
        
        #endregion

        #endregion
    }

    internal class TestWorkOrderStreetOpeningPermitCreateFormBuilder : TestDataBuilder<TestWorkOrderStreetOpeningPermitCreateForm>
    {
        #region Private Members

        private IDropDownList _ddlForms;
        private IButton _btnCreate;
        private IPlaceHolder _phPermitForm, _phPaymentForm;
        //private IPanel _pnlSelect, _pnlForm;
        private IRepository<State> _stateRepository;
        private IRepository<County> _countyRepository;
        private IRepository<Municipality> _municipalityRepository;
        private IRepository<Permit> _permitRepository;
        private IRepository<Payment> _paymentRepository;
        private IRequest _iRequest;
        private ILabel _lblFlash;
        private IPage _iPage;
        private IHiddenField _iHidPermitId;
        private IObjectDataSource _IOdsStreetOpeningPermits;
        private ISecurityService _securityService;

        #endregion

        #region Private Methods

        private void SetIRequest(TestWorkOrderStreetOpeningPermitCreateForm view)
        {
            SetFieldValue(view, "_iRequest", _iRequest);
        }
        
        #endregion

        #region Exposed Methods

        public override TestWorkOrderStreetOpeningPermitCreateForm Build()
        {
            var obj = new TestWorkOrderStreetOpeningPermitCreateForm();
            if (_ddlForms != null)
                obj.SetddlForms(_ddlForms);
            if (_btnCreate != null)
                obj.SetBtnCreate(_btnCreate);
            if (_phPermitForm != null)
                obj.SetPhPermitForm(_phPermitForm);
            if (_phPaymentForm != null)
                obj.SetPhPaymentForm(_phPaymentForm);
            //if (_pnlSelect != null)
            //    obj.SetPnlSelect(_pnlSelect);
            //if (_pnlForm != null)
            //    obj.SetPnlForm(_pnlForm);
            if (_stateRepository != null)
                obj.SetStateRepository(_stateRepository);
            if (_countyRepository != null)
                obj.SetCountyRepository(_countyRepository);
            if (_municipalityRepository != null)
                obj.SetMunicipalityRepository(_municipalityRepository);
            if (_permitRepository != null)
                obj.SetPermitRepository(_permitRepository);
            if (_paymentRepository != null)
                obj.SetPaymentRepository(_paymentRepository);
            if (_lblFlash != null)
                obj.SetLblFlash(_lblFlash);
            if (_iPage != null)
                obj.SetIPage(_iPage);
            if (_iRequest != null)
                SetIRequest(obj);
            if (_iHidPermitId != null)
                obj.SetHidPermitId(_iHidPermitId);
            if (_IOdsStreetOpeningPermits != null)
                obj.SetObjectDataSource(_IOdsStreetOpeningPermits);
            if (_securityService != null)
                obj.SetSecurityService(_securityService);
            return obj;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithDdlForms(IDropDownList ddl)
        {
            _ddlForms = ddl;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithBtnCreate(IButton button)
        {
            _btnCreate = button;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithPhPermitForm(IPlaceHolder ph)
        {
            _phPermitForm = ph;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithPhPaymentForm(IPlaceHolder ph)
        {
            _phPaymentForm = ph;
            return this;
        }

        //public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithPnlSelect(IPanel pnl)
        //{
        //    _pnlSelect = pnl;
        //    return this;
        //}

        //public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithPnlForm(IPanel pnl)
        //{
        //    _pnlForm = pnl;
        //    return this;
        //}

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithStateRepository(IRepository<State> repository)
        {
            _stateRepository = repository;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithCountyRepository(IRepository<County> repository)
        {
            _countyRepository = repository;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithMunicipalityRepository(IRepository<Municipality> repository)
        {
            _municipalityRepository = repository;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithPermitRepository(IRepository<Permit> repository)
        {
            _permitRepository = repository;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithPaymentRepository(IRepository<Payment> repository)
        {
            _paymentRepository = repository;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithIRequest(IRequest request)
        {
            _iRequest = request;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithLblFlash(ILabel label)
        {
            _lblFlash = label;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithIPage(IPage page)
        {
            _iPage = page;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithHidPermitId(IHiddenField hidden)
        {
            _iHidPermitId = hidden;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithObjectDataSource(IObjectDataSource ods)
        {
            _IOdsStreetOpeningPermits = ods;
            return this;
        }

        public TestWorkOrderStreetOpeningPermitCreateFormBuilder WithSecurityService(ISecurityService service)
        {
            _securityService = service;
            return this;
        }

        #endregion
    }

    internal class TestWorkOrderStreetOpeningPermitCreateForm : WorkOrderStreetOpeningPermitCreateForm
    {
        #region Protected Methods

        public void TestOnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        #endregion
        
        #region Exposed Methods

        public void SetddlForms(IDropDownList ddl)
        {
            ddlForms = ddl;
        }

        public void SetBtnCreate(IButton button)
        {
            btnCreate = button;
        }

        public void SetPhPermitForm(IPlaceHolder ph)
        {
            phPermitForm = ph;
        }

        public void SetPhPaymentForm(IPlaceHolder ph)
        {
            phPaymentForm = ph;
        }

        public void SetPnlSelect(IPanel panel)
        {
            pnlSelect = panel;
        }

        public void SetPnlForm(IPanel panel)
        {
            pnlForm = panel;
        }

        public void SetPnlPayment(IPanel panel)
        {
            pnlPayment = panel;
        }

        public void SetPnlDrawings(IPanel panel)
        {
            pnlDrawings = panel;
        }

        public void SetPnlSuccess(IPanel panel)
        {
            pnlSuccess = panel;
        }

        public void SetStateRepository(IRepository<State> repository)
        {
            StateRepository = repository;
        }

        public void SetCountyRepository(IRepository<County> repository)
        {
            CountyRepository = repository;
        }

        public void SetMunicipalityRepository(IRepository<Municipality> repository)
        {
            MunicipalityRepository = repository;
        }

        public void SetPermitRepository(IRepository<Permit> repository)
        {
            PermitRepository = repository;
        }

        public void SetPaymentRepository(IRepository<Payment> repository)
        {
            PaymentRepository = repository;
        }

        public void SetLblFlash(ILabel label)
        {
            lblFlash = label;
        }

        public void SetIPage(IPage page)
        {
            _iPage = page;
        }

        public void SetHidPermitId(IHiddenField hidden)
        {
            hidPermitId = hidden;
        }

        public void SetObjectDataSource(IObjectDataSource ods)
        {
            odsStreetOpeningPermits = ods;
        }

        public void SetSecurityService(ISecurityService service)
        {
            _securityService = service;
        }

        #endregion
    }
}
