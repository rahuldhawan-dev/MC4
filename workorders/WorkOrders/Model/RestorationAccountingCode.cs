using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Exceptions;
using System.Linq;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.RestorationAccountingCodes")]
    public class RestorationAccountingCode : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_CODE_LENGTH = 8;
        private const short MAX_SUBCODE_LENGTH = 2;

        public static string DELETING_ERROR_MESSAGE = 
            "Cannot delete the Restoration Accounting Code. " +
            "The following work description(s) are linked to this Code: ";

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _RestorationAccountingCodeID;

        private string _code, _subCode;

        private EntitySet<WorkDescription> _primaryWorkDescriptions,
                                           _secondaryWorkDescriptions;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_RestorationAccountingCodeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationAccountingCodeID
        {
            get { return _RestorationAccountingCodeID; }
            set
            {
                if (_RestorationAccountingCodeID != value)
                {
                    SendPropertyChanging();
                    _RestorationAccountingCodeID = value;
                    SendPropertyChanged("RestorationAccountingCodeID");
                }
            }
        }

        [Column(Storage = "_code", DbType = "VarChar(8) NOT NULL")]
        public string Code
        {
            get { return _code; }
            set
            {
                if (value != null && value.Length > MAX_CODE_LENGTH)
                    throw new StringTooLongException("Code", MAX_CODE_LENGTH);
                if (_code != value)
                {
                    SendPropertyChanging();
                    _code = value;
                    SendPropertyChanged("Code");
                }
            }
        }

        [Column(Storage = "_subCode", DbType = "VarChar(2)")]
        public string SubCode
        {
            get { return _subCode; }
            set
            {
                if (value != null && value.Length > MAX_SUBCODE_LENGTH)
                    throw new StringTooLongException("SubCode", MAX_SUBCODE_LENGTH);
                if (_subCode != value)
                {
                    SendPropertyChanging();
                    _subCode = value;
                    SendPropertyChanged("SubCode");
                }
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "FirstRestorationAccountingCode_WorkDescription", Storage = "_primaryWorkDescriptions", OtherKey = "FirstRestorationAccountingCodeID")]
        public EntitySet<WorkDescription> PrimaryWorkDescriptions
        {
            get { return _primaryWorkDescriptions; }
            set { _primaryWorkDescriptions.Assign(value); }
        }

        [Association(Name = "SecondRestorationAccountingCode_WorkDescription", Storage = "_secondaryWorkDescriptions", OtherKey = "SecondRestorationAccountingCodeID")]
        public EntitySet<WorkDescription> SecondaryWorkDescriptions
        {
            get { return _secondaryWorkDescriptions; }
            set { _secondaryWorkDescriptions.Assign(value); }
        }

        #endregion

        #region Logical Properties

        public string Description
        {
            get
            {
                return (SubCode == null)
                    ? Code : String.Format("{0}.{1}", Code, SubCode);
            }
        }

        public bool CanDelete
        {
            get 
            { 
                var workDescriptions = PrimaryWorkDescriptions;
                workDescriptions.AddRange(SecondaryWorkDescriptions);
                if (workDescriptions.Count == 0)
                    return true;
                return false;
            }
        }

        public string DeletingErrorMessage
        {
            get
            {
                var workDescriptions = PrimaryWorkDescriptions;
                workDescriptions.AddRange(SecondaryWorkDescriptions);
                if (workDescriptions.Count == 0)
                    return string.Empty;
                var sb = new StringBuilder();
                sb.Append(DELETING_ERROR_MESSAGE);
                workDescriptions.Each(wd => sb.AppendFormat("{0} ", wd.WorkDescriptionID));
                return sb.ToString();
            }
        }

        #endregion

        #endregion

        #region Constructors

        public RestorationAccountingCode()
        {
            _primaryWorkDescriptions =
                new EntitySet<WorkDescription>(attach_PrimaryWorkDescriptions,
                    detach_PrimaryWorkDescriptions);
            _secondaryWorkDescriptions =
                new EntitySet<WorkDescription>(attach_SecondaryWorkDescriptions,
                    detach_SecondaryWorkDescriptions);
        }

        #endregion

        #region Private Methods

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Code))
                        throw new DomainLogicException("Code cannot be null");
                    break;
            }
        }

        private void attach_PrimaryWorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.FirstRestorationAccountingCode = this;
        }

        private void detach_PrimaryWorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.FirstRestorationAccountingCode = null;
        }

        private void attach_SecondaryWorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.SecondRestorationAccountingCode = this;
        }

        private void detach_SecondaryWorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.SecondRestorationAccountingCode = null;
        }

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, _emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
