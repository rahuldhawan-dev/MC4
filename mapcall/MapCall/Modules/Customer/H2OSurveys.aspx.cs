using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;
using StructureMap;

namespace MapCall.Modules.Customer
{
    public partial class H2OSurveys : TemplatedDetailsViewDataPageBase
    {
        #region Constants

        private const string FILE_NAME = "Letter.html";
        private const string FILE_NAME_NO_WATER_KIT = "NoWaterKit.html";
        private const string H2OLETTER_HTML_VIRTUAL_PATH = "~/Modules/Customer/H2OLetter.html";
        private const string H2OLETTER_NO_WATER_KIT_FOR_YOU_HTML_VIRTUAL_PATH = "~/Modules/Customer/H2OLetter-NoWaterKitForYou.html";

        #endregion

        #region Fields

        private H2OSurveyReport _report;

        #endregion

        #region Properties

        protected H2OSurveyReport Report
        {
            get
            {
                if (_report == null)
                {
                    _report = new H2OSurveyReport();
                    _report.Initialize();
                }
                return _report;
            }
        }

        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return H2O.General; }
        }

        #endregion

        #region Private Methods

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeQuickSearchLinks();
        }

        private void InitializeQuickSearchLinks()
        {
            var links = Template.HomePlaceHolder.Controls.OfType<LinkButton>();
            foreach(var l in links)
            {
                l.CommandName = "PresetSearch";
                l.Click += PresetSearchClick;
            }
        }

        protected override void LoadDataRecord(int recordId)
        {
            base.LoadDataRecord(recordId);
            dsAudit.DataBind();
        }

        private static string FormatParameter(string param)
        {
            return HttpUtility.HtmlEncode(param.Trim());
        }

        protected static string FormatAddress(string houseNum, string aptNum, string streetText, string address2, string cityText, string townSection, string stateText, string zip)
        {
            const string br = "<br />";

            // {142} {Maple Street}
            const string line1 = "{0} {1}";
            const string line2 = "APT: {0}";

            // {CityText}, {StateText} {Zip}
            const string line3 = "{0}, {1} {2}";

            // Basically, let's show the town section name if it's there, otherwise show the city name. 
            var cityName = (!string.IsNullOrWhiteSpace(townSection) ? townSection : cityText);

            // NOTE: There's an issue with spacing 

            var sb = new StringBuilder();

            // Normally we wouldn't wanna do a string.Format when there's already
            // a StringBuilder(string.format uses a StringBuilder already for its
            // AppendFormat method), but we need to in this instance to trim off
            // empty space if there's no house number.
            var addressLine1 = string.Format(line1,
                                               FormatParameter(houseNum),
                                               FormatParameter(streetText)).Trim();
            sb.Append(addressLine1).Append(br);

            if (!string.IsNullOrWhiteSpace(aptNum))
            {
                sb.AppendFormat(line2, FormatParameter(aptNum)).Append(br);
            }

            if (!string.IsNullOrWhiteSpace(address2))
            {
                sb.Append(FormatParameter(address2)).Append(br);
            }

            sb.AppendFormat(line3, FormatParameter(cityName),
                                   FormatParameter(stateText),
                                   FormatParameter(zip)).Append(br);


            return sb.ToString();
        }

        #region H2O Letter


        /// <summary>
        /// Get the text to use for the letter.
        /// </summary>
        /// <returns></returns>
        private string GetH2OLetterText(string virtualPathToLetter)
        {
            using (var fs = new FileStream(Request.MapPath(virtualPathToLetter), FileMode.Open))
            using (var sr = new StreamReader(fs))
            {
                return sr.ReadToEnd();
            }
        }

        /// <summary>
        /// Create a byte array of the file with the letter text and current row
        /// </summary>
        private static byte[] GetH2OLetterBytes(DataRow dr, string letterText)
        {
            // TODO: Instead of a DataRow, lets use the H2OSurvey repo?
            // NOTE: This is being used by both letters, we'll need to change it if
            //       another letter gets added that doesn't follow the same structure.
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream, Encoding.UTF8))
            {
                var address = FormatAddress(
                    dr["HouseNumber"].ToString(),
                    dr["ApartmentNumber"].ToString(),
                    dr["StreetText"].ToString(),
                    dr["AddressLine2"].ToString(),
                    dr["TownSectionText"].ToString(),
                    dr["CityText"].ToString(),
                    dr["StateText"].ToString(),
                    dr["Zip"].ToString());

                var letter = String.Format(letterText,
                                           DateTime.Now.ToShortDateString(),
                                           address,
                                           string.Format("{0} {1}", dr["FirstName"], dr["LastName"]),
                                           DateTime.Now.ToString("D", new CultureInfo("es-ES")));

                streamWriter.Write(letter);
                return memoryStream.GetBuffer();
            }
        }

        private void InsertDocumentLink(int documentId, int dataTypeId, int dataLinkId, int documentTypeId)
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText =
                    "INSERT INTO DocumentLink(DocumentID, DataTypeID, DataLinkID, DocumentTypeID) Values(@DocumentID, @DataTypeId, @DataLinkId, @DocumentTypeId)";
                cmd.Parameters.AddWithValue("DocumentId", documentId);
                cmd.Parameters.AddWithValue("DataTypeId", dataTypeId);
                cmd.Parameters.AddWithValue("DataLinkId", dataLinkId);
                cmd.Parameters.AddWithValue("DocumentTypeId", documentTypeId);
                cmd.ExecuteNonQuery();
            }
        }

        private void AttachDocument(byte[] letter, int dataTypeID, string dbDocumentFileName)
        {
            const int h2oDocumentTypeId = 222;

            var docDataRepo = DependencyResolver.Current.GetService<IDocumentDataRepository>();
            var docData = docDataRepo.FindByBinaryData(letter);
            if (docData == null)
            {
                docData = new DocumentData();
                docData.BinaryData = letter.ToArray();
                // FileSize and Hash are set by the repo.
            }

            var d = new Document();
            d.DocumentData = docData;
            d.FileName = dbDocumentFileName;
            d.DocumentType = DependencyResolver.Current.GetService<IDocumentTypeRepository>().Find(h2oDocumentTypeId);
            d.CreatedByStr = IUser.Name;

            var docRepo = DependencyResolver.Current.GetService<IDocumentRepository>();
            docRepo.Save(d);

            InsertDocumentLink(d.Id, dataTypeID, CurrentDataRecordId, h2oDocumentTypeId);
        }

        private void ResponseWriteFile(byte[] letter, string dbDocumentFileName)
        {
            Response.Clear();
            Response.AddHeader("Content-disposition", String.Format("attachment;filename={0}", Server.UrlEncode(dbDocumentFileName)));
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(letter);
            Response.End();
        }

        private void SaveAndPrintLetter(string virtualPathToLetter, string dbDocumentFileName)
        {
            var dv = Template.DetailsView;
            // For whatever reason, DetailsView isn't re-databinding when the button click posts back
            // so we need to manually databind. -Ross 9/30/2011
            if (dv.DataItem == null) { dv.DataBind(); }
            
            // If it's still null, let's throw a nice exception.
            if (dv.DataItem == null)
            {
                throw new NullReferenceException("Can't find DetailsView.DataItem.");
            }

            var letter = GetH2OLetterBytes(
                            ((DataRowView)dv.DataItem).Row,
                            GetH2OLetterText(virtualPathToLetter));

            AttachDocument(letter, template.DataTypeId, dbDocumentFileName);
            ResponseWriteFile(letter, dbDocumentFileName);
        }

        #endregion

        #endregion

        #region Event Handlers

        protected void PresetSearchClick(object sender, EventArgs e)
        {
            // I, Ross, vomit at the site of this method. I also wrote it.
            // Agreed. It's a horrible site. -- arr

            var lb = (LinkButton)sender;
            if (lb.CommandName != "PresetSearch")
            {
                throw new InvalidOperationException("Invalid command name.");
            }

            var fb = CreateFilterBuilder();
            string expression;

            switch (lb.CommandArgument)
            {
                case "NjSharesCountThisWeek":
                    expression = "DATEPART( wk, CustomerReceivedDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, CustomerReceivedDate) = DATEPART( yyyy, getDate()) and H2OSurveyReceivedThroughTypeID = 1";
                    break;

                case "NjSharesCountLastWeek":
                    expression = "DATEPART( wk, CustomerReceivedDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, CustomerReceivedDate) = DATEPART( yyyy, getDate()) and H2OSurveyReceivedThroughTypeID = 1";
                    break;

                case "PhoneCountThisWeek":
                    // NEEDS THE h. QUALIFIER
                    expression = "DATEPART( wk, LastContactDate) = DATEPART( wk, getDate()) and DATEPART(yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and h.H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful);
                    break;

                case "PhoneCountLastWeek":
                    // NEEDS THE h. QUALIFIER
                    expression = "DATEPART( wk, LastContactDate) = DATEPART( wk, getDate())-1 and DATEPART(yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and h.H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful);
                    break;

                case "CustomersEnrolledCountThisWeek":
                    expression = "DATEPART( wk, EnrollmentDate) = DATEPART( wk, getDate()) and DATEPART(yyyy, EnrollmentDate) = DATEPART( yyyy, getDate())";
                    break;

                case "CustomersEnrolledCountLastWeek":
                    expression = "DATEPART( wk, EnrollmentDate) = DATEPART( wk, getDate())-1 and DATEPART(yyyy, EnrollmentDate) = DATEPART( yyyy, getDate())";
                    break;

                case "WaterSavingKitsProvidedThisWeek":
                    expression = "DATEPART( wk, WaterSavingKitProvidedDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, WaterSavingKitProvidedDate) = DATEPART( yyyy, getDate())";
                    break;

                case "WaterSavingKitsProvidedLastWeek":
                    expression = "DATEPART( wk, WaterSavingKitProvidedDate) = DATEPART( wk, getDate())-1  and DATEPART( yyyy, WaterSavingKitProvidedDate) = DATEPART( yyyy, getDate())";
                    break;

                case "RenterCountThisWeek":
                    expression = "DATEPART( wk, LastContactDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and h.H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful) + " and IsHomeOwner = 0";
                    break;

                case "RenterCountLastWeek":
                    expression = "DATEPART( wk, LastContactDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and h.H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful) + " and IsHomeOwner = 0";
                    break;

                case "AuditPerformedThisWeek":
                    expression = "DATEPART( wk, AuditPerformedDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, AuditPerformedDate) = DATEPART( yyyy, getDate())";
                    break;

                case "AuditPerformedLastWeek":
                    expression = "DATEPART( wk, AuditPerformedDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, AuditPerformedDate) = DATEPART( yyyy, getDate())";
                    break;

                case "CallListCount":
                    expression = "IsNull(h.H2OSurveyContactStatusTypeID,0) <> " + ((int)H2OSurveyContactTypes.Successful) + " and DATEDIFF(d, EnrollmentDate, getdate()) >= 3 and IsDuplicate = 'false'";
                    break;

                case "WaterKitFollowUpCallListCount":
                    expression = "DATEDIFF(d, ISNULL(WaterSavingKitProvidedDate, getdate()), getdate()) >= 7  and CustomerWithHighWaterUsage = 1 and not (ISNULL(WaterSavingKitFollowupStatusTypeID, 0) = " + ((int)H2OSurveyContactTypes.Successful) + " or ISNULL(H2OSurveyWaterKitFollowUpOutcomeTypeID, 0) = " + ((int)H2OSurveyWaterKitFollowUpOutcomeTypes.UnableToContactCustomer) + ");";
                    break;

                case "PendingQualificationApprovalCount":
                    expression = "AuditPerformedDate Is Not Null and QualifiesForKit is null";
                    break;

                case "CustomersThatRequireAdditionalScheduling":
                    expression = "TechnicianNeededForWaterKitDate is not null and not (ISNULL(SiteVisitScheduleStatusTypeID, 0) = " + ((int)H2OSurveyContactTypes.Successful) + " or ISNULL(H2OSurveySiteVisitSchedulingOutcomeTypeID, 0) = " + ((int)H2OSurveySiteVisitSchedulingOutcomeTypes.UnableToContactCustomer) + ")";
                    break;

                default:
                    throw new NotSupportedException();
            }

            fb.AddExpression(new FilterBuilderExpression
                                 {
                                     CustomFilterExpression = expression
                                 });

            Search(fb);
        }

        protected void BtnPrint_Click(object sender, EventArgs e)
        {
            SaveAndPrintLetter(H2OLETTER_HTML_VIRTUAL_PATH, FILE_NAME);
        }

        protected void BtnPrintNoWaterKit_Click(object sender, EventArgs e)
        {
            SaveAndPrintLetter(H2OLETTER_NO_WATER_KIT_FOR_YOU_HTML_VIRTUAL_PATH, FILE_NAME_NO_WATER_KIT);
        }

        #endregion
       
    }

    #region Quick Search Reports

    /// <summary>
    /// Enum that matches the database values for the H2OSurveyContactStatusTypes table.
    /// </summary>
    internal enum H2OSurveyContactTypes
    {
        Successful = 6
    }

    internal enum H2OSurveyWaterKitFollowUpOutcomeTypes
    {
        UnableToContactCustomer = 5
    }

    internal enum H2OSurveySiteVisitSchedulingOutcomeTypes
    {
        UnableToContactCustomer = 1
    }

    public sealed class H2OSurveyReport
    {
        #region Private Members

        private bool _initialized;

        #endregion

        #region Properties

        public int AuditPerformedThisWeekCount { get; private set; }
        public int AuditPerformedLastWeekCount { get; private set; }
        public int CustomersEnrolledThisWeekCount { get; private set; }
        public int CustomersEnrolledLastWeekCount { get; private set; }
        public int NjSharesCountThisWeek { get; private set; }
        public int NjSharesCountLastWeek { get; private set; }
        public int PhoneCountThisWeek { get; private set; }
        public int PhoneCountLastWeek { get; private set; }
        public int RenterCountThisWeek { get; private set; }
        public int RenterCountLastWeek { get; private set; }
        public int WaterSavingKitsProvidedThisWeekCount { get; private set; }
        public int WaterSavingKitsProvidedLastWeekCount { get; private set; }
        public int CallListCount { get; private set; }
        public int WaterKitFollowUpCallListCount { get; private set; }
        public int PendingQualificationApprovalCount { get; private set; }
        public int CustomersThatRequireAdditionalSchedulingCount { get; private set; }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes and stores all the property values in one go. All property getters should call
        /// this before returning. 
        /// </summary>
        internal void Initialize()
        {
            if (!_initialized)
            {
                _initialized = true;

                using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["MCProd"].ConnectionString))
                using (var com = conn.CreateCommand())
                {
                    conn.Open();
                    var comText
                        = "select COUNT(*) as customersEnrolledThisWeek from h2osurveys where DATEPART( wk, EnrollmentDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, EnrollmentDate) = DATEPART( yyyy, getDate());"
                        + "select COUNT(*) as customersEnrolledLastWeek from h2osurveys where DATEPART( wk, EnrollmentDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, EnrollmentDate) = DATEPART( yyyy, getDate());"

                        + "select COUNT(*) as kitsProvidedCountThisWeek from h2osurveys where DATEPART( wk, WaterSavingKitProvidedDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, WaterSavingKitProvidedDate) = DATEPART( yyyy, getDate());"
                        + "select COUNT(*) as kitsProvidedCountLastWeek from h2osurveys where DATEPART( wk, WaterSavingKitProvidedDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, WaterSavingKitProvidedDate) = DATEPART( yyyy, getDate());"

                        + "select COUNT(*) as phoneCountThisWeek from h2osurveys where DATEPART( wk, LastContactDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful) + ";"
                        + "select COUNT(*) as phoneCountLastWeek from h2osurveys where DATEPART( wk, LastContactDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful) + ";"

                        + "select COUNT(*) as njSharesCountThisWeek from h2osurveys where DATEPART( wk, CustomerReceivedDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, CustomerReceivedDate) = DATEPART( yyyy, getDate()) and H2OSurveyReceivedThroughTypeID = 1;"
                        + "select COUNT(*) as njSharesCountLastWeek from h2osurveys where DATEPART( wk, CustomerReceivedDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, CustomerReceivedDate) = DATEPART( yyyy, getDate()) and H2OSurveyReceivedThroughTypeID = 1;"

                        + "select COUNT(*) as rentersThisWeekCount from h2osurveys where DATEPART( wk, LastContactDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful) + " and IsHomeOwner = 0;"
                        + "select COUNT(*) as rentersLastWeekCount from h2osurveys where DATEPART( wk, LastContactDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, LastContactDate) = DATEPART( yyyy, getDate()) and H2OSurveyContactStatusTypeID = " + ((int)H2OSurveyContactTypes.Successful) + " and IsHomeOwner = 0;"

                        + "select COUNT(*) as auditPerformedThisWeekCount from h2osurveys where DATEPART( wk, AuditPerformedDate) = DATEPART( wk, getDate()) and DATEPART( yyyy, AuditPerformedDate) = DATEPART( yyyy, getDate());"
                        + "select COUNT(*) as auditPerformedLastWeekCount from h2osurveys where DATEPART( wk, AuditPerformedDate) = DATEPART( wk, getDate())-1 and DATEPART( yyyy, AuditPerformedDate) = DATEPART( yyyy, getDate());"

                        + "select COUNT(*) as callListCount from h2osurveys where IsNull(H2OSurveyContactStatusTypeID,0) <> " + ((int)H2OSurveyContactTypes.Successful) + " and DATEDIFF(d, EnrollmentDate, getdate()) >= 3 and IsDuplicate = 'false';"
                        + "select COUNT(*) as callFollowUpCount from h2osurveys where DATEDIFF(d, ISNULL(WaterSavingKitProvidedDate, getdate()), getdate()) >= 7  and CustomerWithHighWaterUsage = 1 and not (ISNULL(WaterSavingKitFollowupStatusTypeID, 0) = " + ((int)H2OSurveyContactTypes.Successful) + " or ISNULL(H2OSurveyWaterKitFollowUpOutcomeTypeID, 0) = " + ((int)H2OSurveyWaterKitFollowUpOutcomeTypes.UnableToContactCustomer) + ");"
                        + "select COUNT(*) as pendingApprovalCount from h2osurveys where AuditPerformedDate Is Not Null and QualifiesForKit is null;"
                        + "select COUNT(*) as schedulingCount from h2osurveys where TechnicianNeededForWaterKitDate is not null and not (ISNULL(SiteVisitScheduleStatusTypeID, 0) = " + ((int)H2OSurveyContactTypes.Successful) + " or ISNULL(H2OSurveySiteVisitSchedulingOutcomeTypeID, 0) = " + ((int)H2OSurveySiteVisitSchedulingOutcomeTypes.UnableToContactCustomer) + ")";
                    com.CommandText = comText;

                    // Running this command takes roughly one whole millisecond without any
                    // indexes or anything. -Ross 4/12/2011

                    using (var r = com.ExecuteReader())
                    {
                        r.Read();
                        CustomersEnrolledThisWeekCount = r.GetInt32(r.GetOrdinal("customersEnrolledThisWeek"));

                        r.NextResult();
                        r.Read();
                        CustomersEnrolledLastWeekCount = r.GetInt32(r.GetOrdinal("customersEnrolledLastWeek"));

                        r.NextResult();
                        r.Read();
                        WaterSavingKitsProvidedThisWeekCount = r.GetInt32(r.GetOrdinal("kitsProvidedCountThisWeek"));

                        r.NextResult();
                        r.Read();
                        WaterSavingKitsProvidedLastWeekCount = r.GetInt32(r.GetOrdinal("kitsProvidedCountLastWeek"));

                        r.NextResult();
                        r.Read();
                        PhoneCountThisWeek = r.GetInt32(r.GetOrdinal("phoneCountThisWeek"));

                        r.NextResult();
                        r.Read();
                        PhoneCountLastWeek = r.GetInt32(r.GetOrdinal("phoneCountLastWeek"));

                        r.NextResult();
                        r.Read();
                        NjSharesCountThisWeek = r.GetInt32(r.GetOrdinal("njSharesCountThisWeek"));

                        r.NextResult();
                        r.Read();
                        NjSharesCountLastWeek = r.GetInt32(r.GetOrdinal("njSharesCountLastWeek"));

                        r.NextResult();
                        r.Read();
                        RenterCountThisWeek = r.GetInt32(r.GetOrdinal("rentersThisWeekCount"));

                        r.NextResult();
                        r.Read();
                        RenterCountLastWeek = r.GetInt32(r.GetOrdinal("rentersLastWeekCount"));

                        r.NextResult();
                        r.Read();
                        AuditPerformedThisWeekCount = r.GetInt32(r.GetOrdinal("auditPerformedThisWeekCount"));

                        r.NextResult();
                        r.Read();
                        AuditPerformedLastWeekCount = r.GetInt32(r.GetOrdinal("auditPerformedLastWeekCount"));

                        r.NextResult();
                        r.Read();
                        CallListCount = r.GetInt32(r.GetOrdinal("callListCount"));

                        r.NextResult();
                        r.Read();
                        WaterKitFollowUpCallListCount = r.GetInt32(r.GetOrdinal("callFollowUpCount"));

                        r.NextResult();
                        r.Read();
                        PendingQualificationApprovalCount = r.GetInt32(r.GetOrdinal("pendingApprovalCount"));

                        r.NextResult();
                        r.Read();
                        CustomersThatRequireAdditionalSchedulingCount = r.GetInt32(r.GetOrdinal("schedulingCount"));
                        
                    }
                }
            }
        }

        #endregion
    }

    #endregion
}