using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    // NOTE: This is very very slimmed down compared to MapCall.Common's version because this is supposed to be 
    // entirely readonly on the 271 side. That means properties don't have setters and almost none of the actual
    // properties are referenced.

    [Table(Name = "dbo.JobSiteCheckLists")]
    public class JobSiteCheckList : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Members

#pragma warning disable 649
        private int _JobSiteCheckListID;
        private int? _WorkOrderID;
        private DateTime _CheckListDate;
        private string _CreatedBy;
#pragma warning restore 649

        #endregion

        #region Properties

        [Column(Storage = "_JobSiteCheckListID", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int JobSiteCheckListID
        {
            get
            {
                return _JobSiteCheckListID;
            }
        }

        [Column(Storage = "_WorkOrderID", Name = "MapCallWorkOrderId", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? WorkOrderID
        {
            get { return _WorkOrderID; }
        }

        [Column(Storage = "_CheckListDate", DbType = "datetime NOT NULL", UpdateCheck= UpdateCheck.Never)]
        public DateTime CheckListDate
        {
            get { return _CheckListDate; }
        }

        [Column(Storage="_CreatedBy", DbType="nvarchar(50) NOT NULL", UpdateCheck=UpdateCheck.Never)]
        public string CreatedBy
        {
            get { return _CreatedBy; }
        }

        #endregion

        #region Private Methods

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
