using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    /// <summary>
    /// Slimmed down because it's implemented in mvc
    /// </summary>
    [Table(Name = "dbo.WorkOrderInvoices")]
    public class WorkOrderInvoice
    {
        #region PrivateMembers

        private int _WorkOrderInvoiceID;
        private int? _WorkOrderID;
        private DateTime? _InvoiceDate, _SubmittedDate, _CanceledDate;
        private bool _IncludeMaterials;

        #endregion

        [Column(Storage = "_WorkOrderInvoiceID", Name = "Id", AutoSync = AutoSync.OnInsert, IsPrimaryKey = true, IsDbGenerated = true)]
        public int WorkOrderInvoiceID
        {
            get { return _WorkOrderInvoiceID; }
        }

        [Column(Storage = "_WorkOrderID", Name = "WorkOrderId", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? WorkOrderID
        {
            get { return _WorkOrderID; }
        }

        [Column(Storage = "_InvoiceDate", DbType = "datetime null", UpdateCheck = UpdateCheck.Never)]
        public DateTime? InvoiceDate
        {
            get { return _InvoiceDate;}
        }

        [Column(Storage = "_SubmittedDate", DbType = "datetime null", UpdateCheck = UpdateCheck.Never)]
        public DateTime? SubmittedDate
        {
            get { return _SubmittedDate;}
        }

        [Column(Storage = "_CanceledDate", DbType = "datetime null", UpdateCheck = UpdateCheck.Never)]
        public DateTime? CanceledDate
        {
            get { return _CanceledDate; }
        }

        [Column(Storage = "_IncludeMaterials", DbType = "bit not null")]
        public bool IncludeMaterials
        {
            get { return _IncludeMaterials; }
        }
    }
}