using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using LINQTo271.Common;
using LINQTo271.Views.Abstract;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Controls;
using Permits.Data.Client.Repositories;
using StructureMap;
using WorkOrders;
using WorkOrders.Model;
using Permit = Permits.Data.Client.Entities.Permit;

namespace LINQTo271.Views.Reports
{
    public partial class StreetOpeningPermits : WorkOrdersReport
    {
        #region Constants

        // TODO: this is gonna come back to haunt us:
        public const int COMPANY_ID = 2;

        #endregion

        #region Private Members

        private IStreetOpeningPermitRepository _repository;
        private MMSINC.Data.WebApi.IRepository<Permit> _remoteRepository;

        #endregion

        #region Properties

        public MMSINC.Data.WebApi.IRepository<Permit> RemoteRepository
        {
            get
            {
                return _remoteRepository ??
                       (_remoteRepository =
                        new PermitRepository(
                            SecurityService.Employee.DefaultOperatingCenter.
                            PermitsOMUserName));
            }
        }

        public IStreetOpeningPermitRepository LocalRepository
        {
            get { return _repository ?? (_repository = DependencyResolver.Current.GetService<IStreetOpeningPermitRepository>()); }
        }

        #endregion

        #region Private Methods

        protected IEnumerable<DisplayPermit> GetPermits(string sortExpression)
        {
            var criteria = new NameValueCollection();
            criteria.Add("CompanyId", COMPANY_ID.ToString());
            ApplyRequestedDateFilter(drRequestedDate, criteria);

            var remotePermits = RemoteRepository
                .Search(criteria);

            return
                remotePermits.Map<Permit, DisplayPermit>(
                    p => new DisplayPermit(LocalRepository.FindByPermitId(p.Id), p));
        }

        protected void ApplyRequestedDateFilter(DateRange dr, NameValueCollection nvc)
        {
            if (!dr.Date.HasValue && !dr.StartDate.HasValue &&
                !dr.EndDate.HasValue)
            {
                return;
            }

            switch (dr.SelectedOperator)
            {
                case "=":
                    nvc.Add("DateRequestedEquals", dr.Date.Value.ToString("d"));
                    break;
                case ">":
                    nvc.Add("DateRequestedGreaterThan", dr.Date.Value.ToString("d"));
                    break;
                case ">=":
                    nvc.Add("DateRequestedGreaterThanOrEqualTo", dr.Date.Value.ToString("d"));
                    break;
                case "<":
                    nvc.Add("DateRequestedLessThan", dr.Date.Value.ToString("d"));
                    break;
                case "<=":
                    nvc.Add("DateRequestedLessThanOrEqualTo", dr.Date.Value.ToString("d"));
                    break;
                case "BETWEEN":
                    nvc.Add("DateRequestedStart", dr.StartDate.Value.ToString("d"));
                    nvc.Add("DateRequestedEnd", dr.EndDate.Value.ToString("d"));
                    break;
            }
        }

        #endregion

        #region Event Handlers

        protected void gvSearchResults_DataBinding(object sender, EventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            if (resultView.DataSource == null)
            {
                resultView.DataSource = GetPermits(null);
            }
        }

        protected void gvSearchResults_Sorting(object sender, GridViewSortEventArgs e)
        {
            var resultView = ((MvpGridView)sender);
            resultView.DataSource = GetPermits(GetSortExpression(e.SortExpression));
            resultView.DataBind();
        }

        #endregion

        public class DisplayPermit
        {
            public DisplayPermit(StreetOpeningPermit sop, Permit p)
            {
                PermitId = p.Id;
                DateRequested = p.CreatedAt;
                TotalCharged = p.TotalCharged;
                PermitFee = p.PermitFee;
                BondFee = p.BondFee;
                PermitFor = p.PermitFor;
                IsCanceled = p.IsCanceled ? "Yes" : "No";
                IsPaidFor = p.IsPaidFor ? "Yes" : "No";

                if (sop == null)
                {
                    AccountingCode = p.AccountingCode;
                }
                else
                {
                    AccountingCode =
                        String.IsNullOrEmpty(sop.WorkOrder.AccountCharged)
                            ? p.AccountingCode : sop.WorkOrder.AccountCharged;
                    WorkOrderId = sop.WorkOrderID;
                    Town = sop.WorkOrder.Town.ToString();
                    WorkDescription = sop.WorkOrder.WorkDescription.ToString();
                    AccountingType =
                        sop.WorkOrder.WorkDescription.AccountingType.ToString();
                    OperatingCenter = sop.WorkOrder.OperatingCenter.ToString();
                    DateIssued = sop.DateIssued;
                    ExpirationDate = sop.ExpirationDate;
                }
            }

            public int PermitId { get; set; }
            public int? WorkOrderId { get; set; }
            public DateTime? DateRequested { get; set; }
            public decimal PermitFee { get; set; }
            public decimal BondFee { get; set; }
            public decimal TotalCharged { get; set; }
            public string AccountingCode { get; set; }
            public string OperatingCenter { get; set; }
            public string Town { get; set; }
            public string PermitFor { get; set; }
            public string WorkDescription { get; set; }
            public string AccountingType { get; set; }
            public string IsCanceled { get; set; }

            public string IsPaidFor { get; set; }
            public DateTime? DateIssued { get; set; }
            public DateTime? ExpirationDate { get; set; }
        }
    }
}