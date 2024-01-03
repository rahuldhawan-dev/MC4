using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.DataType")]
    public class DataType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DATA_TYPE_LENGTH = 255;
        private const short MAX_TABLE_NAME_LENGTH = 255;
        private const short MAX_TABLE_ID_LENGTH = 255;
        
        public const short WORK_ORDER_TYPE_ID = 127;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _DataTypeID;

        private string _data_Type, _table_Name, _table_ID;

        private EntitySet<DocumentType> _documentTypes;

        #endregion

        #region Properties

        [Column(Storage = "_DataTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DataTypeID
        {
            get { return _DataTypeID; }
            set
            {
                if (_DataTypeID != value)
                {
                    SendPropertyChanging();
                    _DataTypeID = value;
                    SendPropertyChanged("DataTypeID");
                }
            }
        }

        [Column(Storage = "_data_Type", DbType = "VarChar(255) NULL")]
        public string Data_Type
        {
            get { return _data_Type; }
            set
            {
                if (value != null && value.Length > MAX_DATA_TYPE_LENGTH)
                    throw new StringTooLongException("Data_Type", MAX_DATA_TYPE_LENGTH);
                if (_data_Type != value)
                {
                    SendPropertyChanging();
                    _data_Type = value;
                    SendPropertyChanged("Data_Type");
                }
            }
        }

        [Column(Storage = "_table_Name", DbType = "VarChar(255) NULL")]
        public string Table_Name
        {
            get { return _table_Name; }
            set
            {
                if (value != null && value.Length > MAX_TABLE_NAME_LENGTH)
                    throw new StringTooLongException("Table_Name", MAX_TABLE_NAME_LENGTH);
                if (_table_Name != value)
                {
                    SendPropertyChanging();
                    _table_Name = value;
                    SendPropertyChanged("Table_Name");
                }
            }
        }

        [Column(Storage = "_table_ID", DbType = "VarChar(255) NULL")]
        public string Table_ID
        {
            get { return _table_ID; }
            set
            {
                if (value != null && value.Length > MAX_TABLE_ID_LENGTH)
                    throw new StringTooLongException("Table_ID", MAX_TABLE_ID_LENGTH);
                if (_table_ID != value)
                {
                    SendPropertyChanging();
                    _table_ID = value;
                    SendPropertyChanged("Table_ID");
                }
            }
        }

        [Association(Name = "DataType_DocumentType", Storage = "_documentTypes", OtherKey = "DataTypeID")]
        public EntitySet<DocumentType> DocumentTypes
        {
            get { return _documentTypes; }
            set { _documentTypes.Assign(value); }
        }


        #endregion

        #region Constructors

        public DataType()
        {
            _documentTypes = new EntitySet<DocumentType>(attach_DocumentTypes, detach_DocumentTypes);
        }

        #endregion

        #region Private Members

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

        #region Private Methods
        // ReSharper disable UnusedPrivateMember
        //private void OnValidate(ChangeAction action)
        //{
        //    switch (action)
        //    {
        //        case ChangeAction.Insert:
        //        case ChangeAction.Update:
                    
        //            break;
        //    }
        //}

        private void attach_DocumentTypes(DocumentType entity)
        {
            SendPropertyChanging();
            entity.DataType = this;
        }

        private void detach_DocumentTypes(DocumentType entity)
        {
            SendPropertyChanging();
            entity.DataType = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
