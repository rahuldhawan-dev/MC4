using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.DataPages;
using MMSINC.Utilities.Permissions;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;
using MapCall.Controls.Data;

namespace MapCall.Reports.Customer
{
    public partial class H2OSurveyReport : ReportPageBase 
    {
        protected override DetailsViewDataPageTemplate Template
        {
            get { return template; }
        }

        protected override IModulePermissions ModulePermissions
        {
            get { return H2O.General; }
        }

        protected override IGridView ResultsGridView
        {
            get { return repLettersSent.GridView; }
        }


        #region Private Methods

        #region Address Formatting

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

        #endregion

        #endregion

       
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var today = DateTime.Today;
            sfEnrollmentDate.StartDate = StartOfWeek(today, DayOfWeek.Sunday);
            sfEnrollmentDate.EndDate = EndOfWeek(today, DayOfWeek.Saturday);
        }

        protected override string RenderResultsGridViewToExcel()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<p>The customers listed on this page have been enrolled in the H2O Program this week.</p>");
            sb.Append(base.RenderResultsGridViewToExcel());

            sb.AppendLine("<p>The customers listed on this page have been called and identified as renters and a telephone audit was performed this week</p>");
            sb.Append(RenderGridViewToExcel(repRenters.GridView));

            sb.AppendLine("<p>The customers listed on this page have been called and are owners and a telephone audit was performed this week.</p>");
            sb.Append(RenderGridViewToExcel(repAudited.GridView));

            sb.AppendLine("<p>The customers listed on this page have been sent a water saving kit this week.</p>");
            sb.Append(RenderGridViewToExcel(repKitSentTotals.GridView));
            sb.Append(RenderGridViewToExcel(repKitSent.GridView));

            sb.AppendLine("<p>The customers listed on this page have been sent the letter for audited customers that have an average use of water.</p>");
            sb.Append(RenderGridViewToExcel(repAverageUserCustomers.GridView));

            sb.AppendLine("<p>The customers listed on this page have had a site visit.</p>");
            sb.Append(RenderGridViewToExcel(repSiteVisits.GridView));

            sb.AppendLine("<p>The customers listed on this page have been sent the letter for audited customers that have high water usage.</p>");
            sb.Append(RenderGridViewToExcel(repHighUsageCustomers.GridView));

            return sb.ToString();
        }

        protected override string RenderGridViewToExcel(IGridView gv)
        {

            // Hides the "View" fields since they're useless in an Excel sheet. 
            foreach (DataControlField col in gv.Columns)
            {
                if (col is TemplateField)
                {
                    var tf = (TemplateField)col;
                    if (tf.HeaderText == "")
                    {
                        tf.Visible = false;
                    }
                }
            }
            return base.RenderGridViewToExcel(gv);
        }

        protected override void ApplyFilterBuilder(IFilterBuilder fb)
        {
            base.ApplyFilterBuilder(fb);
            var dateExpression = fb.Expressions.First();
            var startDateParam = DateTime.Now;
            var endDateParam = DateTime.Now;

            var dfName = sfEnrollmentDate.DataFieldName;
            var startName = dfName + DateTimeRange.PARAMETER_SUFFIXES.START_DATE;
            var endName = dfName + DateTimeRange.PARAMETER_SUFFIXES.END_DATE;
            
            foreach (var p in dateExpression.Parameters)
            {
                if (p.Name == startName)
                {
                    startDateParam = (DateTime)p.Value;
                }
                else if (p.Name == endName)
                {
                    endDateParam = (DateTime)p.Value;
                }
            }
            
            ApplyRenterAuditFilter(startDateParam, endDateParam);
            ApplyHomeOwnerAuditFilter(startDateParam, endDateParam);
            ApplyKitsSentFilter(startDateParam, endDateParam);
            ApplyAverageUseCustomerFilter(startDateParam, endDateParam);
            ApplySiteVisitFilter(startDateParam, endDateParam);
            ApplyHighUsageFilter(startDateParam, endDateParam);
        }

        private IFilterBuilder GetSpecialFilterBuilder(H2OSurveyReportResult report)
        {
            var fb = CreateFilterBuilder();
            fb.SelectCommand = report.SelectCommand;
            return fb;
        }

        /// <summary>
        /// This is the quick hackery for getting the date expression. I don't feel like copying the code out of the 
        /// datetime control for this because it's gonna end up getting rewritten(I hope).
        /// </summary>
        /// <param name="fb"></param>
        /// <param name="dataFieldName"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        private void SetDateExpression(IFilterBuilder fb, string dataFieldName, DateTime startDate, DateTime endDate)
        {
            sfEnrollmentDate.DataFieldName = dataFieldName;
            sfEnrollmentDate.StartDate = startDate;
            // Subtract from endDate bceause FilterExpression is gonna add a day. It's annoying.
            sfEnrollmentDate.EndDate = endDate.AddDays(-1);
            sfEnrollmentDate.FilterExpression(fb);
        }

        private void ApplyRenterAuditFilter(DateTime startDate, DateTime endDate)
        {
            var renterFb = GetSpecialFilterBuilder(repRenters);
            SetDateExpression(renterFb, "LastContactDate", startDate, endDate);
         
            renterFb.AddExpression(new FilterBuilderExpression("IsHomeOwner", DbType.Boolean, false));
            repRenters.ApplyFilter(renterFb);
        }

        private void ApplyHomeOwnerAuditFilter( DateTime startDate, DateTime endDate)
        {
            var auditFb = GetSpecialFilterBuilder(repAudited);
            SetDateExpression(auditFb, "AuditPerformedDate", startDate, endDate);
            auditFb.AddExpression(new FilterBuilderExpression("IsHomeOwner", DbType.Boolean, true));
            repAudited.ApplyFilter(auditFb);
        }

        private void ApplyKitsSentFilter(DateTime startDate, DateTime endDate)
        {
            var kitsSentFb = GetSpecialFilterBuilder(repKitSent);
            SetDateExpression(kitsSentFb, "WaterSavingKitProvidedDate", startDate, endDate);
            repKitSent.ApplyFilter(kitsSentFb);

            var kitsSentTotals = GetSpecialFilterBuilder(repKitSentTotals);
            SetDateExpression(kitsSentTotals, "WaterSavingKitProvidedDate", startDate, endDate);
            repKitSentTotals.ApplyFilter(kitsSentTotals);
        }

        private void ApplyAverageUseCustomerFilter(DateTime startDate, DateTime endDate)
        {
            var fb = GetSpecialFilterBuilder(repAverageUserCustomers);
            SetDateExpression(fb, "QualifiesForKitDate", startDate, endDate);
            fb.AddExpression(new FilterBuilderExpression("QualifiesForKit", DbType.Boolean, false));
            repAverageUserCustomers.ApplyFilter(fb);
        }

        private void ApplySiteVisitFilter(DateTime startDate, DateTime endDate)
        {
            var fb = GetSpecialFilterBuilder(repSiteVisits);
            SetDateExpression(fb, "SiteVisitDate", startDate, endDate);
            repSiteVisits.ApplyFilter(fb);
        }


        private void ApplyHighUsageFilter(DateTime startDate, DateTime endDate)
        {
            var fb = GetSpecialFilterBuilder(repHighUsageCustomers);
            SetDateExpression(fb, "QualifiesForKitDate", startDate, endDate);
            fb.AddExpression(new FilterBuilderExpression("CustomerWithHighWaterUsage", System.Data.DbType.Boolean, true));
            repHighUsageCustomers.ApplyFilter(fb);
        }

        private static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
        {
            var diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        private static DateTime EndOfWeek(DateTime dt, DayOfWeek endOfWeek)
        {
            var diff = endOfWeek - dt.DayOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(diff).Date;
        }

    }

    #region Results view helpers

    [ParseChildren(true)]
    public class H2OSurveyReportResult : MvpPlaceHolder
    {
        // Need to be created early so that properties can be set prior to OnInit
        private MvpGridView _gridView = new MvpGridView();
        private McProdDataSource _dataSource = new McProdDataSource();
        private Label _countLabel = new Label();
        private bool _filterApplied;

        // Defaults to true
        public bool ShowRecordCount { get; set; }

        public string SelectCommand
        {
            get { return _dataSource.SelectCommand; }
            set { _dataSource.SelectCommand = value; }
        }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public DataControlFieldCollection Columns { get { return _gridView.Columns; } }

        public MvpGridView GridView { get { return _gridView; } }
        public bool AutoGenerateColumns
        {
            get { return GridView.AutoGenerateColumns; }
            set { GridView.AutoGenerateColumns = value;  }
        }

        public H2OSurveyReportResult()
        {
            ShowRecordCount = true;
        }

        private void InitControls()
        {
            _gridView.DataSource = _dataSource;

            Controls.Add(_countLabel);
            Controls.Add(_gridView);
            Controls.Add(_dataSource);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitControls();
            _dataSource.SelectCommand = SelectCommand;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            _countLabel.Visible = ShowRecordCount;
            _countLabel.Text = "Total Records: " + _gridView.Rows.Count;
        }
        
        public void ApplyFilter(IFilterBuilder fb)
        {
            if (fb == null)
            {
                throw new ArgumentNullException("fb");
            }

            // Don't apply the filter again if it's already been applied once. This
            // happens when the user tries to export.
            if (_filterApplied) { return; }

            // filt will always have the original SelectCommand at the beginning, so it
            // should never ever be null. 
            var filt = fb.BuildCompleteCommand();

            _dataSource.SelectCommand = filt;

            var sp = _dataSource.SelectParameters;
            foreach (var p in fb.BuildParameters())
            {
                sp.Add(p);
            }

            _gridView.DataBind();
            _filterApplied = true;
        }
    }
    #endregion
}