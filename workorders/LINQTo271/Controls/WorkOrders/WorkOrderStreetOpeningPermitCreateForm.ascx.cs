using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Controls;
using MMSINC.Data.Linq;
using MMSINC.Utilities;
using Permits.Data.Client;
using Permits.Data.Client.Entities;
using Permits.Data.Client.Repositories;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;
using County = Permits.Data.Client.Entities.County;
using State = Permits.Data.Client.Entities.State;

namespace LINQTo271.Controls.WorkOrders
{
    public partial class WorkOrderStreetOpeningPermitCreateForm : WorkOrderDetailControlBase
    {
        #region Constants

        public struct ViewStateKeys
        {
            public const string STATE = "State",
                                COUNTY = "County",
                                MUNICIPALITY = "Municipality";
        }

        public struct Params
        {
            public const string WORKORDER_ID = "WorkOrderID",
                                PERMIT_ID = "PermitID",
                                HAS_MET_DRAWING_REQUIREMENTS = "HasMetDrawingRequirements",
                                IS_PAID_FOR = "IsPaidFor";
        }

        public struct QueryStringParams
        {
            public const string PERMIT_ID = "permitId";
        }

        public const string 
            NO_FORMS = "No forms are available for {0}, {1}, or {2}.",
            CSS_FORMAT = "<link rel=\"stylesheet\" href=\"{0}content/api-css\" />";

        #endregion

        #region Control Declarations

        protected IDropDownList ddlForms;
        protected ILabel lblMunicipality, lblCounty, lblState, lblFlash;
        protected IPanel pnlSelect, pnlForm, pnlDrawings, pnlPayment, pnlSuccess;
        protected IButton btnCreate, btnSubmit;
        protected IPlaceHolder phPermitForm, phPaymentForm;
        protected IHiddenField hidPermitId;
        protected IObjectDataSource odsStreetOpeningPermits;

        #endregion

        #region Private Members

        private MMSINC.Data.WebApi.IRepository<Payment> _paymentRepository;
        private MMSINC.Data.WebApi.IRepository<State> _stateRepository;
        private MMSINC.Data.WebApi.IRepository<County> _countyRepository;
        private MMSINC.Data.WebApi.IRepository<Municipality> _municipalityRepository;
        private MMSINC.Data.WebApi.IRepository<Permit> _permitRepository;
        private IDateTimeProvider _dateTimeProvider;
        private int? _permitId;

        #endregion

        #region Properties

        public IDateTimeProvider DateTimeProvider
        {
            get
            {
                if (_dateTimeProvider == null)
                    _dateTimeProvider =
                        DependencyResolver.Current.GetService<IDateTimeProvider>();
                return _dateTimeProvider;
            }
        }

        /// <summary>
        /// This is used for accessing the api webservices on permits. Not used
        /// when submitting the permit.
        /// </summary>
        public string PermitsUserName
        {
            get
            {
                return SecurityService.Employee.DefaultOperatingCenter.PermitsUserName;
            }
        }

        public WorkOrder WorkOrder
        {
            get { return WorkOrderRepository.Get(WorkOrderID); }
        }
        
        public int PermitId
        {
            get
            {
                if (!_permitId.HasValue)
                    _permitId = Convert.ToInt32(hidPermitId.Value);
                return _permitId.Value;
            }
        }
        
        #region Forms/Entities

        public string FormId
        {
            get { return ddlForms.SelectedValue; }
        }

        public int StateId { get; set; }
        
        public int CountyId { get; set; }
        
        public int MunicipalityId { get; set; }

        public string State 
        { 
            get { return (string)IViewState.GetValue(ViewStateKeys.STATE); }
            set
            {
                IViewState.SetValue(ViewStateKeys.STATE, value.ToCultureTitleCase());
            } 
        }

        public string County 
        {
            get { return (string)IViewState.GetValue(ViewStateKeys.COUNTY); }
            set
            {
                IViewState.SetValue(ViewStateKeys.COUNTY, value.ToCultureTitleCase());
            }
        }

        public string Municipality 
        {
            get { return (string)IViewState.GetValue(ViewStateKeys.MUNICIPALITY); }
            set 
            { 
                IViewState.SetValue(ViewStateKeys.MUNICIPALITY, value.ToCultureTitleCase());
            }
        }

        #endregion

        #region Repositories

        public MMSINC.Data.WebApi.IRepository<State> StateRepository
        {
            get {
                return 
                    _stateRepository ?? (_stateRepository = 
                    new Permits.Data.Client.Repositories.StateRepository(PermitsUserName));
            }

            set { _stateRepository = value; }
        }

        public MMSINC.Data.WebApi.IRepository<County> CountyRepository
        {
            get {
                return 
                    _countyRepository ??(_countyRepository =
                    new CountyRepository(PermitsUserName));
            }
            set { _countyRepository = value; }
        }

        public MMSINC.Data.WebApi.IRepository<Municipality> MunicipalityRepository
        {
            get {
                return 
                    _municipalityRepository ?? (_municipalityRepository =
                    new MunicipalityRepository(PermitsUserName));
            }
            set { _municipalityRepository = value; }
        }

        /// <summary>
        /// Permits Web Site
        /// THIS MUST USE THE PermitsUserName FROM THE WORKORDER
        /// </summary>
        public MMSINC.Data.WebApi.IRepository<Permit> PermitRepository
        {
            get
            {
                return 
                    _permitRepository ?? (_permitRepository =
                    new PermitRepository(WorkOrder.OperatingCenter.PermitsUserName));
            }
            set { _permitRepository = value; }
        }

        /// <summary>
        /// THIS MUST USE THE PermitsUserName FROM THE WORKORDER
        /// </summary>
        public MMSINC.Data.WebApi.IRepository<Payment> PaymentRepository
        {
            get
            {
                return
                    _paymentRepository ?? (_paymentRepository =
                     new PaymentRepository(WorkOrder.OperatingCenter.PermitsUserName));
            }
            set { _paymentRepository = value; }
        }

        /// <summary>
        /// 271 Street Opening Permits
        /// </summary>
        public IRepository<StreetOpeningPermit> StreetOpeningPermitRepository
        {
            get
            {
                return
                    DependencyResolver.Current.GetService<IRepository<StreetOpeningPermit>>();
            }
        }

        public IRepository<WorkOrder> WorkOrderRepository
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<WorkOrder>>();
            }
        }

        #endregion

        #endregion

        #region Private Methods

        protected override void SetDataSource(int workOrderID)
        {

        }

        #region Add Forms
        // Use the webservice on permits to see if the name exists 
        // and has a valid form, if so add it to the form dropdown
        
        private State LookupState()
        {
            var states = StateRepository.Search(new NameValueCollection { { "name", State } });
            if (states == null) return null;
            var state = states.FirstOrDefault();
            if (state != null)
            {
                StateId = state.Id;

                if (state.FormId > 0)
                {
                    return state;
                }
            }

            return null;
        }

        private County LookupCounty()
        {
            // go call up a webservice on permits to see if the county name exists 
            // if a formId exists, then add it to the form dropdown
            var counties = CountyRepository.Search(new NameValueCollection { { "name", County }, { "stateId", StateId.ToString() } });
            if (counties == null) return null;
            var county = counties.FirstOrDefault();
            if (county != null)
            {
                CountyId = county.Id;

                if (county.FormId > 0)
                {
                    return county;
                }
            }

            return null;
        }

        private Municipality LookupMunicipality()
        {
            // go call up a webservice on permits to see if the muni name exists 
            // if a formId exists, then add it to the form dropdown
            var municipalities = MunicipalityRepository.Search(new NameValueCollection { { "name", Municipality }, { "countyId", CountyId.ToString() } });
            if (municipalities == null) return null;
            var municipality = municipalities.FirstOrDefault();
            if (municipality != null)
            {
                MunicipalityId = municipality.Id;

                if (municipality.FormId > 0)
                {
                    return municipality;
                }
            }

            return null;
        }

        #endregion

        #region javascripts 

        private static string ToJson(object obj)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        private void AddAutoPopulationDataScript()
        {
            var json = ToJson(new {
                LocationStreetNumber = WorkOrder.StreetNumber,
                LocationStreetName = WorkOrder.Street.FullStName,
                LocationCity = WorkOrder.Town.Name,
                LocationState = WorkOrder.Town.State.Abbreviation,
                LocationZip = WorkOrder.ZipCode,
                ArbitraryIdentifier = WorkOrder.WorkOrderID,
                PurposeOfOpening = WorkOrder.WorkDescription.Description,
            });

            var script = string.Format("var WorkOrderData = {0};", json);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "WorkOrderData-" + UniqueID, script, true);
        }

        /// <summary>
        /// Creates a client side script with the permit Id to initialize the uploader
        /// </summary>
        /// <param name="permitId"></param>
        private void CreateUploaderScript(int permitId)
        {
            var json = ToJson(new
            {
                permitId = permitId,
                workOrderId = WorkOrderID,
            });
            var script = string.Format("var UploaderInit = {0};", json);
            ScriptManager.RegisterClientScriptBlock(this, GetType(), "UploaderInitialize-" + UniqueID, script, true);
        }

        #endregion
        
        #region Panels

        private void ShowPanel(IPanel panel)
        {
            foreach (var p in Controls.OfType<IPanel>())
            {
                p.Visible = (panel.ID == p.ID);
            }
        }

        private void ShowPaymentPanel(int permitId)
        {
            var payment = PaymentRepository.New(new NameValueCollection {
                { "permitId", permitId.ToString() }
            });
            phPaymentForm.Controls.Add(new Label { Text = payment });
            hidPermitId.Value = permitId.ToString();
            ShowPanel(pnlPayment);
        }

        private void ShowDrawingPanel(int permitId)
        {
            CreateUploaderScript(permitId);
            hidPermitId.Value = permitId.ToString();
            ShowPanel(pnlDrawings);
        }

        #endregion

        #region Permit Saving/Updating

        /// <summary>
        /// Save the permit to 271
        /// </summary>
        /// <param name="permit"></param>
        private void SavePermit(Permit permit)
        {
            var newPermit = new StreetOpeningPermit
            {
                WorkOrderID = WorkOrderID,
                DateRequested = DateTimeProvider.GetCurrentDate(),
                HasMetDrawingRequirement = permit.HasMetDrawingRequirement,
                IsPaidFor = permit.IsPaidFor,
                PermitId = permit.Id,
                StreetOpeningPermitNumber = string.Empty
            };
            StreetOpeningPermitRepository.InsertNewEntity(newPermit);
        }

        /// <summary>
        /// Updates the permit in 271 from the permit values on permits.mapcall.net
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Permit UpdatePermit(int id)
        {
            var permit = PermitRepository.Find(id);

            odsStreetOpeningPermits.UpdateParameters[Params.WORKORDER_ID].DefaultValue = WorkOrderID.ToString();
            odsStreetOpeningPermits.UpdateParameters[Params.PERMIT_ID].DefaultValue = id.ToString();
            odsStreetOpeningPermits.UpdateParameters[Params.HAS_MET_DRAWING_REQUIREMENTS].DefaultValue = permit.HasMetDrawingRequirement.ToString();
            odsStreetOpeningPermits.UpdateParameters[Params.IS_PAID_FOR].DefaultValue = permit.IsPaidFor.ToString();
            odsStreetOpeningPermits.Update();

            return permit;
        }

        private void UpdatePermitAndDisplayCorrectPanel(int permitId)
        {
            var permit = UpdatePermit(permitId);

            if (permit.IsPaidFor)
                ShowPanel(pnlSuccess);
            else if (permit.HasMetDrawingRequirement)
                ShowPaymentPanel(permitId);
            else
                ShowDrawingPanel(permitId);
        }

        #endregion

        #endregion

        #region Event Handlers

        #region Initialization

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            lblFlash.Text = string.Empty;
            IPage.AddHeaderControl(
                new LiteralControl { 
                    Text = String.Format(CSS_FORMAT, Utilities.BaseAddress)
            });
        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            CheckQueryString();
        }

        /// <summary>
        /// Determine if the querystring wants to bring us directly to a permit drawing or payment step
        /// </summary>
        private void CheckQueryString()
        {
            if (IPage.IsPostBack) return;

            var qsPermitId = IRequest.IQueryString.GetValue<int?>(QueryStringParams.PERMIT_ID);
            if (qsPermitId != null)
            {
                hidPermitId.Value = qsPermitId.ToString();
                UpdatePermitAndDisplayCorrectPanel(qsPermitId.Value);
            }
        }

        protected void ddlForms_DataBinding(object sender, EventArgs e)
        {
            var state = LookupState();
            var county = LookupCounty();
            var municipality = LookupMunicipality();

            if (municipality != null)
            {
                ddlForms.Items.Add(new ListItem {
                    Text = municipality.Name,
                    Value = municipality.FormId.ToString()
                });
            }
            if (county != null)
            {
                ddlForms.Items.Add(new ListItem {
                    Text = county.Name,
                    Value = county.FormId.ToString()
                });
            }
            if (state != null)
            {
                ddlForms.Items.Add(new ListItem(state.Name,
                    state.FormId.ToString()));
            }

            if (ddlForms.Items.Count > 0)
            {
                btnCreate.Visible = ddlForms.Visible = true;
            }
            else
            {
                lblFlash.Text = String.Format(NO_FORMS, State, County,
                    Municipality);
            }
        }

        #endregion

        #region Forms

        protected void On_Create(object sender, EventArgs e)
        {
            var permit = PermitRepository.New(new NameValueCollection { { "formId", FormId } });
            phPermitForm.Controls.Add(new Label { Text = permit });
            AddAutoPopulationDataScript();
            ShowPanel(pnlForm);
        }
        
        #endregion

        #region Submit Form

        protected void On_Submit(object sender, EventArgs e)
        {
            var result = PermitRepository.Save(IRequest.Form);
            phPermitForm.Controls.Clear();
            phPermitForm.Controls.Add(new Label { Text = String.Format("Success: PermitID {0}", result.Id) });
            // we need to store the new permit Id in the mcprod db.
            // the has drawing requirement and is paid for property too
            SavePermit(result);
            // create a client side script with the permit Id to initialize the uploader
            if (result.HasMetDrawingRequirement)
            {
                ShowPaymentPanel(result.Id);
            }
            else
            {
                ShowDrawingPanel(result.Id);
            }
        }

        #endregion

        #region Drawings

        protected void On_DrawingUploaded(object sender, EventArgs e)
        {
            UpdatePermitAndDisplayCorrectPanel(PermitId);
        }
        
        #endregion

        #region Payment 
        
        protected void On_SubmitPayment(object sender, EventArgs e)
        {
            PaymentRepository.Save(new Payment { 
                PermitId = PermitId
            });
            UpdatePermitAndDisplayCorrectPanel(PermitId);
        }

        #endregion

        #endregion
    }
}