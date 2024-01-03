using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Document")]
    public class Document : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_CREATEDBY_LENGTH = 50,
                            MAX_MODIFIEDBY_LENGTH = 50,
                            MAX_FILENAME_LENGTH = 255;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _documentID,
                    _documentTypeID,
                    _documentDataID;

        private int? _createdByID, _modifiedByID;
        private string _createdby, _modifiedby, _filename;
        private DateTime _createdOn;
        private DateTime? _modifiedOn;

        private EntityRef<Employee> _employeeCreatedBy, _employeeModifiedBy;
        private EntityRef<DocumentType> _documentType;
        private EntityRef<DocumentData> _documentData;
        
        private EntitySet<DocumentWorkOrder> _documentsWorkOrders;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_documentID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DocumentID
        {
            get { return _documentID; }
            set
            {
                if (_documentID != value)
                {
                    SendPropertyChanging();
                    _documentID = value;
                    SendPropertyChanged("DocumentID");
                }
            }
        }

        [Column(Storage = "_createdby", DbType = "VarChar(50)")]
        public string CreatedBy
        {
            get { return _createdby; }
            set
            {
                if (value != null && value.Length > MAX_CREATEDBY_LENGTH)
                    throw new StringTooLongException("CreatedBy", MAX_CREATEDBY_LENGTH);
                if (_createdby != value)
                {
                    SendPropertyChanging();
                    _createdby = value;
                    SendPropertyChanged("CreatedBy");
                }
            }
        }

        [Column(Name = "CreatedAt", Storage = "_createdOn", DbType = "DateTime NOT NULL")]
        public DateTime CreatedOn
        {
            get { return _createdOn; }
            set
            {
                if (_createdOn != value)
                {
                    SendPropertyChanging();
                    _createdOn = value;
                    SendPropertyChanged("CreatedOn");
                }
            }
        }
        
        [Column(Storage = "_modifiedby", DbType = "VarChar(50)")]
        public string ModifiedBy
        {
            get { return _modifiedby; }
            set
            {
                if (value != null && value.Length > MAX_MODIFIEDBY_LENGTH)
                    throw new StringTooLongException("ModifiedBy", MAX_MODIFIEDBY_LENGTH);
                if (_modifiedby != value)
                {
                    SendPropertyChanging();
                    _modifiedby = value;
                    SendPropertyChanged("ModifiedBy");
                }
            }
        }

        [Column(Name = "UpdatedAt", Storage = "_modifiedOn", DbType = "DateTime NULL")]
        public DateTime? ModifiedOn
        {
            get { return _modifiedOn; }
            set
            {
                if (_modifiedOn != value)
                {
                    SendPropertyChanging();
                    _modifiedOn = value;
                    SendPropertyChanged("ModifiedOn");
                }
            }
        }

        [Column(Name = "File_Name", Storage = "_filename", DbType = "VarChar(255) NOT NULL")]
        public string FileName
        {
            get { return _filename; }
            set
            {
                if (value != null && value.Length > MAX_FILENAME_LENGTH)
                    throw new StringTooLongException("FileName", MAX_FILENAME_LENGTH);
                if (_filename != value)
                {
                    SendPropertyChanging();
                    _filename = value;
                    SendPropertyChanged("FileName");
                }
            }
        }

        [Column(Storage = "_createdByID", DbType = "Int NOT NULL")]
        public int? CreatedByID
        {
            get { return _createdByID; }
            set
            {
                if (_createdByID != value)
                {
                    if (_employeeCreatedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _createdByID = value;
                SendPropertyChanged("CreatedByID");
            }
        }

        [Column(Name = "UpdatedById", Storage = "_modifiedByID", DbType = "Int NULL")]
        public int? ModifiedByID
        {
            get { return _modifiedByID; }
            set
            {
                if (_modifiedByID != value)
                {
                    if (_employeeModifiedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _modifiedByID = value;
                SendPropertyChanged("ModifiedByID");
            }
        }

        [Column(Storage = "_documentTypeID", DbType = "Int NOT NULL")]
        public int DocumentTypeID
        {
            get { return _documentTypeID; }
            set
            {
                if (_documentTypeID != value)
                {
                    if (_documentType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _documentTypeID = value;
                SendPropertyChanged("DocumentTypeID");
            }
        }

        [Column(Storage = "_documentDataID", DbType = "Int NOT NULL")]
        public int DocumentDataID
        {
            get { return _documentDataID; }
            set
            {
                if (_documentDataID != value)
                {
                    if (_documentData.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _documentDataID = value;
                SendPropertyChanged("DocumentDataID");
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "CreatedBy_Document", Storage = "_employeeCreatedBy", ThisKey = "CreatedByID", IsForeignKey = true)]
        public Employee EmployeeCreatedBy
        {
            get
            {
                if (CreatedByID != null)
                    return _employeeCreatedBy.Entity;
                return null;
            }
            set
            {
                Employee previousValue = _employeeCreatedBy.Entity;
                if ((previousValue != value)
                    || (_employeeCreatedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _employeeCreatedBy.Entity = null;
                        previousValue.DocumentsAdded.Remove(this);
                    }
                    _employeeCreatedBy.Entity = value;
                    if (value != null)
                    {
                        value.DocumentsAdded.Add(this);
                        _createdByID = value.EmployeeID;
                    }
                    else
                        _createdByID = default(int);
                    SendPropertyChanged("EmployeeCreatedBy");
                }
            }
        }

        [Association(Name = "ModifiedBy_Document", Storage = "_employeeModifiedBy", ThisKey = "ModifiedByID", IsForeignKey = true)]
        public Employee EmployeeModifiedBy
        {
            get { return _employeeModifiedBy.Entity; }
            set
            {
                Employee previousValue = _employeeModifiedBy.Entity;
                if ((previousValue != value)
                    || (_employeeModifiedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _employeeModifiedBy.Entity = null;
                        previousValue.DocumentsModified.Remove(this);
                    }
                    _employeeModifiedBy.Entity = value;
                    if (value != null)
                    {
                        value.DocumentsModified.Add(this);
                        _modifiedByID = value.EmployeeID;
                    }
                    else
                        _modifiedByID = default(int);
                    SendPropertyChanged("EmployeeModifiedBy");
                }
            }
        }

        [Association(Name = "DocumentType_Document", Storage = "_documentType", ThisKey = "DocumentTypeID", IsForeignKey = true)]
        public DocumentType DocumentType
        {
            get { return _documentType.Entity; }
            set
            {
                DocumentType previousValue = _documentType.Entity;
                if ((previousValue != value)
                    || (_documentType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _documentType.Entity = null;
                        previousValue.Documents.Remove(this);
                    }
                    _documentType.Entity = value;
                    if (value != null)
                    {
                        value.Documents.Add(this);
                        _documentTypeID = value.DocumentTypeID;
                    }
                    else
                        _documentTypeID = default(int);
                    SendPropertyChanged("DocumentType");
                }
            }
        }


        [Association(Name = "DocumentData_Document", Storage = "_documentData", ThisKey = "DocumentDataID", IsForeignKey = true)]
        public DocumentData DocumentData
        {
            get { return _documentData.Entity; }
            set
            {
                DocumentData previousValue = _documentData.Entity;
                if ((previousValue != value)
                    || (_documentData.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _documentData.Entity = null;
                    }
                    _documentData.Entity = value;
                    SendPropertyChanged("DocumentData");
                }
            }
        }
        
        [Association(Name="Document_DocumentWorkOrder", Storage="_documentsWorkOrders", OtherKey="DocumentID")]
        public EntitySet<DocumentWorkOrder> DocumentsWorkOrders
        {
           get { return _documentsWorkOrders; }
           set { _documentsWorkOrders.Assign(value); }
        }

        public int? WorkOrderId
        {
            get
            {
                if (DocumentsWorkOrders.Any())
                {
                    return DocumentsWorkOrders.First().WorkOrderID;
                }

                return null;
            }
        }

        #endregion

        #endregion

        #region Constructors

        public Document()
        {
            _documentsWorkOrders =
                new EntitySet<DocumentWorkOrder>(attach_DocumentsWorkOrders,
                    detach_DocumentsWorkOrders);
        }

        #endregion

        #region Private Methods

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

        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                    if (CreatedOn == DateTime.MinValue)
                        CreatedOn = DateTime.Now;
                    if (DocumentData == null)
                        throw new DomainLogicException("Cannot save a Document object without file data.");
                    if (String.IsNullOrEmpty(FileName))
                        throw new DomainLogicException("Cannot save a Document object without a File Name.");
                    if (EmployeeCreatedBy == null)
                        throw new DomainLogicException("Cannot save a Document object without specifying who created it.");
                    if (DocumentType == null)
                        throw new DomainLogicException(("Cannot save a document object without specifying the document type"));
                    break;
                case ChangeAction.Update:
                    ModifiedOn = DateTime.Now;
                    if (EmployeeModifiedBy == null)
                        throw new DomainLogicException("Cannot update a Document objects without specifying who modified it.");
                    break;
            }
        }

        private void attach_DocumentsWorkOrders(DocumentWorkOrder entity)
        {
            SendPropertyChanging();
            entity.Document = this;
        }

        private void detach_DocumentsWorkOrders(DocumentWorkOrder entity)
        {
            SendPropertyChanging();
            entity.Document = null;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
