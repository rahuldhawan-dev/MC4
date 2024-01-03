using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.DocumentsWorkOrders")]
    public class DocumentWorkOrder : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _DocumentWorkOrderID, _documentID, _workOrderID;
        
        private EntityRef<Document> _document;
        private EntityRef<WorkOrder> _workOrder;

        #endregion

        #region Properties

        #region Table Column Properties

        [Column(Storage = "_DocumentWorkOrderID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int DocumentWorkOrderID
        {
            get { return _DocumentWorkOrderID; }
            set
            {
                if (_DocumentWorkOrderID != value)
                {
                    SendPropertyChanging();
                    _DocumentWorkOrderID = value;
                    SendPropertyChanged("DocumentWorkOrderID");
                }
            }
        }

        [Column(Storage = "_documentID", DbType = "Int NOT NULL")]
        public int DocumentID
        {
            get { return _documentID; }
            set
            {
                if (_documentID != value)
                {
                    if (_document.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _documentID = value;
                SendPropertyChanged("DocumentID");
            }
        }

        [Column(Storage = "_workOrderID", DbType = "Int NOT NULL")]
        public int WorkOrderID
        {
            get { return _workOrderID; }
            set
            {
                if (_workOrderID != value)
                {
                    if (_workOrder.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _workOrderID = value;
                SendPropertyChanged("WorkOrderID");
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "Document_DocumentWorkOrder", Storage = "_document", ThisKey = "DocumentID", IsForeignKey = true)]
        public Document Document
        {
            get { return _document.Entity; }
            set
            {
                Document previousValue = _document.Entity;
                if ((previousValue != value)
                    || (_document.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _document.Entity = null;
                        previousValue.DocumentsWorkOrders.Remove(this);
                    }
                    _document.Entity = value;
                    if (value != null)
                    {
                        value.DocumentsWorkOrders.Add(this);
                        _documentID = value.DocumentID;
                    }
                    else
                        _documentID = default(int);
                    SendPropertyChanged("Document");
                }
            }
        }

        [Association(Name = "WorkOrder_DocumentWorkOrder", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
            set
            {
                WorkOrder previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.DocumentsWorkOrders.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.DocumentsWorkOrders.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                        _workOrderID = default(int);
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        #endregion

        #endregion

        #region Constructors

        public DocumentWorkOrder()
        {
        }

        #endregion

        #region Private Members

        protected void OnValidate(ChangeAction action)
        {
            switch(action)
            {
                case ChangeAction.Insert:
                    if (WorkOrder == null)
                        throw new DomainLogicException("Cannot link a document to a workorder without a workorder");
                    if (Document == null)
                        throw new DomainLogicException("Cannot link a document to a workorder without a workorder");

                    break;
            }
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
    }

}
