using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.RestorationProductCodes")]
    public class RestorationProductCode : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_CODE_LENGTH = 4;

        public static string DELETING_ERROR_MESSAGE =
            "Cannot delete the Restoration Product Code. " +
            "The following WorkDescription(s) are linked to this Code: ";

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _restorationProductCodeID;

        private string _code;

        private EntitySet<WorkDescription> _primaryWorkDescriptions,
                                           _secondaryWorkDescriptions;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_restorationProductCodeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationProductCodeID
        {
            get { return _restorationProductCodeID; }
            set
            {
                if (_restorationProductCodeID != value)
                {
                    SendPropertyChanging();
                    _restorationProductCodeID = value;
                    SendPropertyChanged("RestorationProductCodeID");
                }
            }
        }

        [Column(Storage = "_code", DbType = "VarChar(4) NOT NULL")]
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

        #endregion

        #region Association Properties

        [Association(Name = "FirstRestorationProductCode_WorkDescription", Storage = "_primaryWorkDescriptions", OtherKey = "FirstRestorationProductCodeID")]
        public EntitySet<WorkDescription> PrimaryWorkDescriptions
        {
            get { return _primaryWorkDescriptions; }
            set { _primaryWorkDescriptions.Assign(value); }
        }

        [Association(Name = "SecondRestorationProductCode_WorkDescription", Storage = "_secondaryWorkDescriptions", OtherKey = "SecondRestorationProductCodeID")]
        public EntitySet<WorkDescription> SecondaryWorkDescriptions
        {
            get { return _secondaryWorkDescriptions; }
            set { _secondaryWorkDescriptions.Assign(value); }
        }

        #endregion

        #region Logical Properties

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

        public RestorationProductCode()
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
            entity.FirstRestorationProductCode = this;
        }

        private void detach_PrimaryWorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.FirstRestorationProductCode = null;
        }

        private void attach_SecondaryWorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.SecondRestorationProductCode = this;
        }

        private void detach_SecondaryWorkDescriptions(WorkDescription entity)
        {
            SendPropertyChanging();
            entity.SecondRestorationProductCode = null;
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
            return Code;
        }

        #endregion
    }
}
