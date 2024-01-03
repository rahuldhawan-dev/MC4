using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MMSINC.Common;
using MMSINC.Utilities;
using MapCall.Common;
using MapCall.Common.Model.Entities.Customers;
using MapCall.Common.Model.Repositories.Customers;
using MapCall.Common.Utility.Permissions.Modules;
using MapCall.Controls;
using StructureMap;

namespace MapCall.Reports.Customer
{
    // NOTE: I'm not comfortable with the "Customers Remaining in Queue" value because
    //       there's no way to actually calculate that after a given month has ended. 
    //       A customer may end up being contacted in a later month, which then messes
    //       up the remaining queue total. 
    //       This value may need to be calculated manually by Evaristo or someone.
    //       Someone that isn't Ross
    public partial class H2OSurveyMonthlyReport : MvpPage
    {
        #region Consts

        // Needs to have one white space so that no text gets added. Otherwise it'll generate "Column1" as a name.
        private const string EMPTY_HEADER = " ";
        private static readonly DateTime MINIMUM_START_DATE = new DateTime(2011, 4, 1);

        #endregion

        #region Structs

        private struct QUERY
        {
            public const string START_MONTH = "startMonth";
            public const string START_YEAR = "startYear";
            public const string END_MONTH = "endMonth";
            public const string END_YEAR = "endYear";
        }

        #endregion

        #region Fields

        private readonly Type _objType = typeof(object);

        private bool _hasSearchQuery;
        private bool _searchControlHasBadValues;

        #endregion

        #region Properties

        public DateTime StartMonth { get; set; }
        public DateTime EndMonth { get; set; }

        #endregion

        #region Private Methods

        #region Life Cycle

        private void VerifyAccess()
        {
            var role = new MMSINC.DataPages.Permissions.RoleBasedDataPagePermissions(H2O.General, IUser);
            if (!role.ReadAccess.IsAllowed)
            {
                IResponse.Write("Access Denied - " + role.PermissionName);
                IResponse.End();
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            VerifyAccess();
            var qs = Request.QueryString;

            _hasSearchQuery = RequestHasSearchValues();

            if (_hasSearchQuery)
            {
                StartMonth = new DateTime(int.Parse(qs[QUERY.START_YEAR]), int.Parse(qs[QUERY.START_MONTH]), 1);
                EndMonth = new DateTime(int.Parse(qs[QUERY.END_YEAR]), int.Parse(qs[QUERY.END_MONTH]), 1);
            }
            else
            {
                // Bug #: 1145: Default to start of the current year. 
                StartMonth = new DateTime(DateTime.Now.Year, 1, 1); 
                EndMonth = DateTime.Now;
            }

            // Do this so we don't have a hundred empty columns showing up cause someone decided
            // to search for something in 2009.
            StartMonth = (StartMonth >= MINIMUM_START_DATE ? StartMonth : MINIMUM_START_DATE);

        }

        private bool RequestHasSearchValues()
        {
            var qs = Request.QueryString;
            Func<string, bool> hasValue = (query) =>
                                              {
                                                  var value = qs[query];
                                                  if (string.IsNullOrWhiteSpace(value))
                                                  {
                                                      return false;
                                                  }

                                                  int valInt;
                                                  return int.TryParse(value, out valInt);
                                              };

            return (hasValue(QUERY.START_MONTH)
                    && hasValue(QUERY.START_YEAR)
                    && hasValue(QUERY.END_MONTH)
                    && hasValue(QUERY.END_YEAR));
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            searchDates.StartDate = StartMonth;
            searchDates.EndDate = EndMonth;
            searchDates.SelectedOperatorType = OperatorTypes.Between;

            gridCustomersCompleted.RowDataBound += OnGridRowDataBound;
            gridCompletionOutcomes.RowDataBound += OnGridRowDataBound;
            gridCustomersRemaining.RowDataBound += OnGridRowDataBound;
            gridIntake.RowDataBound += OnGridRowDataBound;
            gridMisc.RowDataBound += OnGridRowDataBound;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // We're doing this in the PreRender so we're not rendering the results when
            // the search button is clicked. It'd be a waste of tiem since the search
            // does a Response.Redirect.
            if (_hasSearchQuery && !_searchControlHasBadValues)
            {
                phResults.Visible = true;
                DoTheReportThing();
            }
            else
            {
                phResults.Visible = false;
            }
            lblError.Visible = _searchControlHasBadValues;

        }

        #endregion

        private void DoTheReportThing()
        {
            var repo = DependencyResolver.Current.GetService<IH2OSurveyRepository>();

            var surveys = repo.GetAllSurveys().ToArray();
            var reportItems = new List<IH2OSurveyReportItem>();

            var cur = StartMonth;
            while (cur <= EndMonth)
            {
                reportItems.Add(new H2OSurveyMonthlyReportItem(cur, surveys));
                cur = cur.AddMonths(1);
            }

            // Need our totals to be the last item in the list.
            // Also, make a copy of the report items because we'll be adding the
            // totals instance to the same list, which would end up with a stackoverflow
            // when it tries to calculate.
            var totals = new H2OSurveyTotalReportItem(reportItems.ToList());
            reportItems.Add(totals);

            gridIntake.DataSource = GetIntakeDataSource(reportItems);
            gridIntake.DataBind();

            gridCustomersCompleted.DataSource = GetCustomersCompletedDataSource(reportItems);
            gridCustomersCompleted.DataBind();

            gridCustomersRemaining.DataSource = GetCustomersRemainingDataSource(reportItems);
            gridCustomersRemaining.DataBind();

            gridCompletionOutcomes.DataSource = GetCompletionOutcomesDataSource(reportItems);
            gridCompletionOutcomes.DataBind();

            gridMisc.DataSource = GetMiscDataSource(reportItems);
            gridMisc.DataBind();

        }


        #region DataSet crap

        private DataSet GetIntakeDataSource(IEnumerable<IH2OSurveyReportItem> reportItems)
        {
            var ds = new DataSet();
            var dt = new DataTable("");
            var c = dt.Columns;
            var cMonth = c.Add(EMPTY_HEADER, _objType);
            var cCustomersFromNJShares = c.Add(SubHeadingWrap("Customers from NJ Shares"), _objType);
            var cCustomersFromNJAW = c.Add(SubHeadingWrap("Customers from NJAW"), _objType);
            var cCustomersEnrolledInProgram = c.Add(SubHeadingWrap("Customers Enrolled in Program"), _objType);
            var cDuplicateCustomers = c.Add(SubHeadingWrap("Duplicate Customers"), _objType);
            var cTotalUniqueCustomersInProgram = c.Add(HeadingWrap("Net Customers in Program"), _objType);

            foreach (var ri in reportItems)
            {
                var dr = dt.NewRow();
                dr[cMonth] = GetMonthHeader(ri);
                dr[cCustomersFromNJShares] = SubValueWrap(ri.CustomersFromNJShares);
                dr[cCustomersFromNJAW] = SubValueWrap(ri.CustomersFromNJAW);
                dr[cCustomersEnrolledInProgram] = SubValueWrap(ri.CustomersEnrolledInProgram);
                dr[cDuplicateCustomers] = SubValueWrap(ri.DuplicateCustomers);
                dr[cTotalUniqueCustomersInProgram] = ValueWrap(ri.TotalUniqueCustomersInProgram);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            return FlipDataSet(ds);
        }

        private DataSet GetCustomersCompletedDataSource(IEnumerable<IH2OSurveyReportItem> reportItems)
        {
            var ds = new DataSet();
            var dt = new DataTable("");
            var c = dt.Columns;
            var cMonth = c.Add(EMPTY_HEADER, _objType);
            var cDisqualifiedCustomers = c.Add(SubHeadingWrap("Disqualified Customers"), _objType);
            var cAuditedByMMSI = c.Add(SubHeadingWrap("Audited by MMSI"), _objType);
            var cAuditedByNJAW = c.Add(SubHeadingWrap("Audited By NJAW"), _objType);
            var cTotalCustomersAudited = c.Add(HeadingWrap("Total Customers Audited"), _objType);

            foreach (var ri in reportItems)
            {
                var dr = dt.NewRow();
                dr[cMonth] = GetMonthHeader(ri);
                dr[cDisqualifiedCustomers] = SubValueWrap(ri.DisqualifiedCustomers);
                dr[cAuditedByMMSI] = SubValueWrap(ri.AuditedByMMSI);
                dr[cAuditedByNJAW] = SubValueWrap(ri.AuditedByNJAW);
                dr[cTotalCustomersAudited] = ValueWrap(ri.TotalCustomersCompleted);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            return FlipDataSet(ds);
        }

        private DataSet GetCompletionOutcomesDataSource(IEnumerable<IH2OSurveyReportItem> reportItems)
        {
            var ds = new DataSet();
            var dt = new DataTable("");
            var c = dt.Columns;
            var cMonth = c.Add(EMPTY_HEADER, _objType);
            var cDisqualifiedCustomers = c.Add(SubHeadingWrap("Disqualified Customers"), _objType);
            var cCustomersThatWereDisqualifiedRenters = c.Add(SubSubHeadingWrap("Called and Disqualified (Renters pre 7/8/11)"), _objType);
            var cCustomersNotInterestedInParticipating = c.Add(SubSubHeadingWrap("Customers Not Interested in Participating"), _objType);
            var cCustomersGetWaterKits = c.Add(SubHeadingWrap("Customers Get Water Kits"), _objType);
            var cCustomersDidNotGetWaterKits = c.Add(SubHeadingWrap("Customers Did Not Get Water Kits"), _objType);
            var cTotalOutcomes = c.Add(HeadingWrap("Total Outcomes"), _objType);

            foreach (var ri in reportItems)
            {
                var dr = dt.NewRow();
                dr[cMonth] = GetMonthHeader(ri);
                dr[cDisqualifiedCustomers] = SubValueWrap(ri.DisqualifiedCustomers);
                dr[cCustomersThatWereDisqualifiedRenters] = SubSubValueWrap(ri.CustomersThatWereDisqualifiedRenters);
                dr[cCustomersNotInterestedInParticipating] = SubSubValueWrap(ri.CustomersNotInterestedInParticipating);
                dr[cCustomersGetWaterKits] = SubValueWrap(ri.CustomersGetWaterKits);
                dr[cCustomersDidNotGetWaterKits] = SubValueWrap(ri.CustomersDidNotGetWaterKits);
                dr[cTotalOutcomes] = ValueWrap(ri.TotalOutcomes);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            return FlipDataSet(ds);
        }

        private DataSet GetCustomersRemainingDataSource(IEnumerable<IH2OSurveyReportItem> reportItems)
        {
            var ds = new DataSet();
            var dt = new DataTable("");
            var c = dt.Columns;
            var cMonth = c.Add(EMPTY_HEADER, _objType);
            var cCustomersRemainingInQueue = c.Add(HeadingWrap("Customers Remaining in Queue"), _objType);

            foreach (var ri in reportItems)
            {
                var dr = dt.NewRow();
                dr[cMonth] = GetMonthHeader(ri);
                dr[cCustomersRemainingInQueue] = ValueWrap(ri.CustomersRemainingInQueue);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            return FlipDataSet(ds);
        }

        private DataSet GetMiscDataSource(IEnumerable<IH2OSurveyReportItem> reportItems)
        {
            var ds = new DataSet();
            var dt = new DataTable("");
            var c = dt.Columns;
            var cMonth = c.Add(EMPTY_HEADER, _objType);
            var cSiteVisits = c.Add(HeadingWrap("Site Visits"), _objType);
            var cHighWaterUsage = c.Add(HeadingWrap("Customers with High Water Usage"), _objType);

            foreach (var ri in reportItems)
            {
                var dr = dt.NewRow();
                dr[cMonth] = GetMonthHeader(ri);
                dr[cSiteVisits] = ValueWrap(ri.SiteVisits);
                dr[cHighWaterUsage] = ValueWrap(ri.HighWaterUsageCustomers);
                dt.Rows.Add(dr);
            }

            ds.Tables.Add(dt);
            return FlipDataSet(ds);
        }

        private static DataSet FlipDataSet(DataSet my_DataSet)
        {
            var ds = new DataSet();
            foreach (DataTable dt in my_DataSet.Tables)
            {
                var table = new DataTable();
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    table.Columns.Add(Convert.ToString(i));
                }
                for (int k = 0; k < dt.Columns.Count; k++)
                {
                    var r = table.NewRow();
                    r[0] = dt.Columns[k].ToString();
                    for (int j = 1; j <= dt.Rows.Count; j++)
                        r[j] = dt.Rows[j - 1][k];
                    table.Rows.Add(r);
                }
                ds.Tables.Add(table);
            }

            return ds;
        }

        private static string GetMonthHeader(IH2OSurveyReportItem ri)
        {
            if (ri.GetType() == typeof(H2OSurveyTotalReportItem))
            {
                return "Totals";
            }

            var month = ((H2OSurveyMonthlyReportItem)ri).ReportMonth;
            return string.Format("{0:MMM-yy}", month);
        }

        private static string SpanWrap(object value, string cssClass)
        {
            const string format = "<span class=\"{0}\">{1}</span>";
            return string.Format(format, cssClass, (value != null ? value.ToString() : string.Empty));
        }

        private static string HeadingWrap(string name)
        {
            return SpanWrap(name, "row-heading");
        }

        private static string SubHeadingWrap(string name)
        {
            return SpanWrap(name, "row-heading sub-row-heading");
        }

        // Yeah.
        private static string SubSubHeadingWrap(string name)
        {
            return SpanWrap(name, "row-heading sub-sub-row-heading");
        }

        private static string ValueWrap(object value)
        {
            return SpanWrap(value, "value");
        }

        private static string SubValueWrap(object value)
        {
            return SpanWrap(value, "sub-value");
        }

        // Yeah I know. Jim needs more indentation. -Ross 9/16/11
        private static string SubSubValueWrap(object value)
        {
            return SpanWrap(value, "sub-sub-value");
        }

        #endregion

        #endregion

        #region Exporting

        protected void RenderGridViewToExcel()
        {
            var resp = Page.Response;
            resp.Clear();
            resp.AddHeader("content-disposition", "attachment;filename=Data.xls");
            resp.Charset = "";
            resp.ContentType = "application/vnd.ms-excel";

            using (var sw = new StringWriter())
            using (var htw = new HtmlTextWriter(sw))
            {
                DoTheReportThing();
                phExportable.RenderControl(htw);
                resp.Write(sw.ToString());
                resp.End();
            }
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            // Kill for Excel.
        }


        #endregion

        #region Event Handlers

        protected void btnExportOnClick(object sender, EventArgs e)
        {
            RenderGridViewToExcel();
        }

        protected void btnSearchOnClick(object sender, EventArgs e)
        {
            if (!searchDates.StartDate.HasValue || !searchDates.EndDate.HasValue)
            {
                _searchControlHasBadValues = true;
                return;
            }

            var query = new Dictionary<string, object>();
            query.Add(QUERY.START_MONTH, searchDates.StartDate.Value.Month);
            query.Add(QUERY.START_YEAR, searchDates.StartDate.Value.Year);
            query.Add(QUERY.END_MONTH, searchDates.EndDate.Value.Month);
            query.Add(QUERY.END_YEAR, searchDates.EndDate.Value.Year);

            var url = QueryStringHelper.BuildFromDictionary(IRequest.Url, query);
            Response.Redirect(url, true);

        }


        protected void OnGridRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType != DataControlRowType.DataRow) { return; }
            foreach (TableCell cell in e.Row.Cells)
            {
                cell.Text = Server.HtmlDecode(cell.Text);
            }
        }

        #endregion

        #region Helper Classes

        private interface IH2OSurveyReportItem
        {
            int CustomersFromNJShares { get; }
            int CustomersFromNJAW { get; }
            int CustomersNotInterestedInParticipating { get; }
            int CustomersThatWereDisqualifiedRenters { get; }
            int CustomersGetWaterKits { get; }
            int CustomersDidNotGetWaterKits { get; }
            int CustomersRemainingInQueue { get; }
            int DuplicateCustomers { get; }
            int CustomersEnrolledInProgram { get; }
            int TotalUniqueCustomersInProgram { get; }
            int AuditedByNJAW { get; }
            int AuditedByMMSI { get; }
            int TotalCustomersCompleted { get; }
            int DisqualifiedCustomers { get; }
            int SiteVisits { get; }
            int HighWaterUsageCustomers { get; }
            int TotalOutcomes { get; }
        }

        private class H2OSurveyMonthlyReportItem : IH2OSurveyReportItem
        {

            #region Enums

            // NOTE: All these enums are bound directly to database values. So don't touch them unless those values change!
            private enum AuditPerformedByType
            {
                MMSI = 1,
                NJAW = 2
            }

            private enum SurveyContactStatusType
            {
                Successful = 6
            }

            private enum SurveyReceivedThroughType
            {
                NJSHARES = 1,
                NJAW = 2
            }

            #endregion

            #region Fields

            private int? _disqualifiedCustomers;
            private int? _totalOutcomes;

            #endregion

            #region Properties

            /// <summary>
            ///  Set to true to ignore the Month for counts and just count all of them.
            /// </summary>
            public DateTime ReportMonth { get; private set; }
            public IEnumerable<H2OSurvey> Surveys { get; private set; }
            public int CustomersFromNJShares
            {
                get
                {
                    return (from s in Surveys
                            where IsInReportMonth(s.EnrollmentDate) // Why it's not CustomerRecievedDate is beyond me.
                            && s.H2OSurveyReceivedThroughTypeID == (int)SurveyReceivedThroughType.NJSHARES
                            select s).Count();
                }
            }

            public int CustomersFromNJAW
            {
                get
                {
                    return (from s in Surveys
                            where IsInReportMonth(s.EnrollmentDate)
                            && s.H2OSurveyReceivedThroughTypeID == (int)SurveyReceivedThroughType.NJAW
                            select s).Count();
                }
            }

            public int DuplicateCustomers
            {
                get
                {
                    return (from s in Surveys
                            where
                            IsInReportMonth(s.CustomerReceivedDate)
                            && s.IsDuplicate
                            select s).Count();
                }
            }
            public int CustomersEnrolledInProgram
            {
                get
                {
                    return (from s in Surveys
                            where IsInReportMonth(s.EnrollmentDate)
                            select s).Count();
                }
            }
            public int TotalUniqueCustomersInProgram
            {
                get
                {
                    return (CustomersEnrolledInProgram - DuplicateCustomers);
                }
            }

            public int AuditedByNJAW
            {
                get
                {
                    return (from s in Surveys
                            where
                                IsInReportMonth(s.AuditPerformedDate)
                                && s.H2OSurveyAuditPerformedByTypeID.HasValue
                                && s.H2OSurveyAuditPerformedByTypeID.Value == (int)AuditPerformedByType.NJAW
                            select s).Count();
                }
            }
            public int AuditedByMMSI
            {
                get
                {
                    return (from s in Surveys
                            where
                                IsInReportMonth(s.AuditPerformedDate)
                                && s.H2OSurveyAuditPerformedByTypeID.HasValue
                                && s.H2OSurveyAuditPerformedByTypeID.Value == (int)AuditPerformedByType.MMSI
                            select s).Count();
                }
            }


            public int CustomersNotInterestedInParticipating
            {
                get
                {
                    return (from s in Surveys
                            where IsInReportMonth(s.LastContactDate)
                            && s.DoesCustomerWantToParticpate.HasValue
                            && !s.DoesCustomerWantToParticpate.Value
                            select s).Count();
                }
            }

            /// <summary>
            /// Gets customers that were disqualified because they were renters
            /// before July 8, 2011 due to NJAW changing who gets audited. 
            /// </summary>
            public int CustomersThatWereDisqualifiedRenters
            {
                get
                {
                    var renterDisqualifiedDate = new DateTime(2011, 7, 8);
                    return (from s in Surveys
                            where IsInReportMonth(s.LastContactDate)
                            && s.LastContactDate.Value < renterDisqualifiedDate
                            && s.IsHomeOwner.HasValue
                            && s.IsHomeOwner.Value == false
                            select s).Count();
                }
            }

            public int DisqualifiedCustomers
            {
                get
                {
                    if (!_disqualifiedCustomers.HasValue)
                    {
                        // The logic here is very painful. Painful indeed.
                        // Disqualified Customers are:
                        //  - Customers who have IsCustomerQualified set to false
                        //  - Customers who are renters that had their LastContactDate priot to July 8, 2011
                        //  - Customers who have DoesCustomerWantToParticipate set to false.

                        var matching = new HashSet<H2OSurvey>();

                        // This was the last day that renters were automatically
                        // disqualified. 
                        var renterDisqualifiedDate = new DateTime(2011, 7, 7);

                        foreach (var s in Surveys)
                        {
                            // Because of the data being filled in being not-so-good,
                            // we're gonna arbitrarily ignore any records where the CustomerReceivedDate
                            // isn't in our report month. There's no other way to get this number correctly.
                            if (!IsInReportMonth(s.LastContactDate))
                            {
                                continue;
                            }

                            // So they're qualified, lets match them.
                            if (s.QualifiesForKit.HasValue && s.QualifiesForKit.Value == false)
                            {
                                // This is gonna be a mess because NJAW isn't dating things. It's very annoying!
                                // So here, if they fill out the form correctly(IE: they checked the IsCustomerQualified
                                // box and selected the date), we wanna add that as a match.
                                if (IsInReportMonth(s.QualifiesForKitDate))
                                {
                                    matching.Add(s);
                                }

                                //// And here, if they failed to select a date, we'll base that on AuditPerformedDate.
                                //else if (IsInReportMonth(s.LastContactDate))
                                //{
                                //    matching.Add(s);
                                //}
                            }
                            else if (s.LastContactDate.Value <= renterDisqualifiedDate)
                            {
                                if (s.IsHomeOwner.HasValue && s.IsHomeOwner.Value == false)
                                {
                                    matching.Add(s);
                                }
                            }

                        }

                        //                        _disqualifiedCustomers = matching.Count();

                        _disqualifiedCustomers = (CustomersNotInterestedInParticipating +
                                                  CustomersThatWereDisqualifiedRenters);
                    }
                    return _disqualifiedCustomers.Value;



                }
            }

            public int CustomersRemainingInQueue
            {
                get
                {
                    // Hacking this to return 0 instead of a negative number. Negative number occurs
                    // when people dont' know how to enter things properly.
                    return Math.Max((TotalUniqueCustomersInProgram - TotalCustomersCompleted), 0);
                }
            }

            public int CustomersGetWaterKits
            {
                get
                {
                    return (from s in Surveys
                            where IsInReportMonth(s.WaterSavingKitProvidedDate)
                            select s).Count();
                }
            }

            public int CustomersDidNotGetWaterKits
            {
                get
                {
                    // Customers who have been audited and have not been
                    // provided with a water saving kit for wahtever reason.
                    return (from s in Surveys
                            where IsInReportMonth(s.AuditPerformedDate)
                            && s.WaterSavingKitProvided == false
                            select s).Count();
                }
            }

            public int SiteVisits
            {
                get
                {
                    return (from s in Surveys
                            where IsInReportMonth(s.SiteVisitDate)
                            select s).Count();
                }
            }

            public int HighWaterUsageCustomers
            {
                get { return (from s in Surveys
                                  where IsInReportMonth(s.QualifiesForKitDate)
                                  && s.CustomerWithHighWaterUsage == true
                                  select s).Count(); }
            }

            // NOTE: TotalCustomersComplete and TotalOutcomes should BE EQUAL.

            /// <summary>
            /// Represents all customers who have been contacted, audited,
            /// and includes the disqualified people.
            /// </summary>
            public int TotalCustomersCompleted
            {
                get { return (AuditedByNJAW + AuditedByMMSI + DisqualifiedCustomers); }
            }

            public int TotalOutcomes
            {
                get
                {
                    if (!_totalOutcomes.HasValue)
                    {
                        _totalOutcomes = (DisqualifiedCustomers + CustomersDidNotGetWaterKits + CustomersGetWaterKits);
                    }
                    return _totalOutcomes.Value;
                }
            }

            #endregion

            #region Construcgtor

            public H2OSurveyMonthlyReportItem(DateTime month, IEnumerable<H2OSurvey> surveys)
            {
                this.ReportMonth = month;
                this.Surveys = surveys;
            }

            #endregion

            #region Private Methods

            private bool IsInReportMonth(DateTime? date)
            {
                if (!date.HasValue) { return false; }
                return (date.Value.Year == ReportMonth.Year && date.Value.Month == ReportMonth.Month);
            }

            #endregion

        }

        private class H2OSurveyTotalReportItem : IH2OSurveyReportItem
        {
            #region Fields

            private IEnumerable<IH2OSurveyReportItem> _reports;

            #endregion

            #region Properties

            public IEnumerable<IH2OSurveyReportItem> Reports
            {
                get
                {
                    if (_reports.Contains(this))
                    {
                        throw new InvalidOperationException(
                            "I'm preventing you from having a stack overflow. TotalReportItem instance can't be in its own list of reports.");
                    }
                    return _reports;
                }
            }

            public int CustomersFromNJShares
            {
                get { return (from r in Reports select r.CustomersFromNJShares).Sum(); }
            }

            public int CustomersFromNJAW
            {
                get { return (from r in Reports select r.CustomersFromNJAW).Sum(); }
            }

            public int CustomersNotInterestedInParticipating
            {
                get { return (from r in Reports select r.CustomersNotInterestedInParticipating).Sum(); }
            }

            public int CustomersThatWereDisqualifiedRenters
            {
                get { return (from r in Reports select r.CustomersThatWereDisqualifiedRenters).Sum(); }
            }

            public int CustomersGetWaterKits
            {
                get { return (from r in Reports select r.CustomersGetWaterKits).Sum(); }
            }

            public int CustomersDidNotGetWaterKits
            {
                get { return (from r in Reports select r.CustomersDidNotGetWaterKits).Sum(); }
            }

            public int DuplicateCustomers
            {
                get { return (from r in Reports select r.DuplicateCustomers).Sum(); }
            }

            public int CustomersEnrolledInProgram
            {
                get { return (from r in Reports select r.CustomersEnrolledInProgram).Sum(); }
            }

            public int TotalUniqueCustomersInProgram
            {
                get { return (from r in Reports select r.TotalUniqueCustomersInProgram).Sum(); }
            }

            public int AuditedByNJAW
            {
                get { return (from r in Reports select r.AuditedByNJAW).Sum(); }
            }

            public int AuditedByMMSI
            {
                get { return (from r in Reports select r.AuditedByMMSI).Sum(); }
            }

            public int TotalCustomersCompleted
            {
                get { return (from r in Reports select r.TotalCustomersCompleted).Sum(); }
            }

            public int DisqualifiedCustomers
            {
                get { return (from r in Reports select r.DisqualifiedCustomers).Sum(); }
            }

            public int CustomersRemainingInQueue
            {
                get { return (from r in Reports select r.CustomersRemainingInQueue).Sum(); }
            }

            public int SiteVisits
            {
                get { return (from r in Reports select r.SiteVisits).Sum(); }
            }

            public int HighWaterUsageCustomers
            {
                get { return Reports.Sum(r => r.HighWaterUsageCustomers); }
            }

            public int TotalOutcomes
            {
                get { return (from r in Reports select r.TotalOutcomes).Sum(); }
            }

            #endregion

            #region Constructor

            public H2OSurveyTotalReportItem(IEnumerable<IH2OSurveyReportItem> reports)
            {
                _reports = reports;
            }

            #endregion
        }

        #endregion
    }
}