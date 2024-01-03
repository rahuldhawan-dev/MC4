using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.DocumentType")]
    public class DocumentType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DOCUMENTTYPE_LENGTH = 50;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _documentTypeID;
        private int _dataTypeID;
        private string _documentType;
        private EntitySet<Document> _documents;
        private EntityRef<DataType> _dataType;

        #endregion

        #region Properties

        [Column(Storage = "_dataTypeID", DbType = "Int NOT NULL")]
        public int DataTypeID
        {
            get { return _dataTypeID; }
            set
            {
                if (_dataTypeID != value)
                {
                    if (_dataType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _dataTypeID = value;
                SendPropertyChanged("DataTypeID");
            }
        }
        [Column(Storage = "_documentTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DocumentTypeID
        {
            get { return _documentTypeID; }
            set
            {
                if (_documentTypeID != value)
                {
                    SendPropertyChanging();
                    _documentTypeID = value;
                    SendPropertyChanged("DocumentTypeID");
                }
            }
        }

        [Column(Name = "Document_Type", Storage = "_documentType", DbType = "VarChar(50)")]
        public string DocumentTypeName
        {
            get { return _documentType; }
            set
            {
                if (value != null && value.Length > MAX_DOCUMENTTYPE_LENGTH)
                    throw new StringTooLongException("DocumentType", MAX_DOCUMENTTYPE_LENGTH);
                if (_documentType != value)
                {
                    SendPropertyChanging();
                    _documentType = value;
                    SendPropertyChanged("DocumentType");
                }
            }
        }

        [Association(Name = "DocumentType_Document", Storage = "_documents", OtherKey = "DocumentTypeID")]
        public EntitySet<Document> Documents
        {
            get { return _documents; }
            set { _documents.Assign(value); }
        }

        [Association(Name = "DataType_DocumentType", Storage = "_dataType", ThisKey = "DataTypeID", IsForeignKey = true)]
        public DataType DataType
        {
            get { return _dataType.Entity; }
            set
            {
                DataType previousValue = _dataType.Entity;
                if ((previousValue != value)
                    || (_dataType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _dataType.Entity = null;
                        previousValue.DocumentTypes.Remove(this);
                    }
                    _dataType.Entity = value;
                    if (value != null)
                    {
                        value.DocumentTypes.Add(this);
                        _dataTypeID = value.DataTypeID;
                    }
                    else
                        _dataTypeID = default(int);
                    SendPropertyChanged("DataType");
                }
            }
        }
        #endregion

        #region Constructors

        public DocumentType()
        {
            _documents = new EntitySet<Document>(attach_Documents, detach_Documents);
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return DocumentTypeName;
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

        private void attach_Documents(Document entity)
        {
            SendPropertyChanging();
            entity.DocumentType = this;
        }

        private void detach_Documents(Document entity)
        {
            SendPropertyChanging();
            entity.DocumentType = null;
        }
        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
