using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using MMSINC.Exceptions;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Restorations")]
    public class Restoration : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_PARTIAL_RESTORATION_INVOICE_NUMBER_LENGTH = 12,
                            MAX_PARTIAL_RESTORATION_COMPLETED_BY_LENGTH = 25,
                            MAX_FINAL_RESTORATION_INVOICE_NUMBER_LENGTH = 12,
                            MAX_FINAL_RESTORATION_COMPLETED_BY_LENGTH = 25;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs _emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _restorationID,
                    _workOrderID,
                    _restorationTypeID;

        private string _restorationNotes,
                       _partialRestorationInvoiceNumber,
                       _partialRestorationCompletedBy,
                       _finalRestorationInvoiceNumber,
                       _finalRestorationCompletedBy;

        private DateTime? _finalRestorationApprovedAt,
                          _dateRejected,
                          _partialRestorationDate,
                          _finalRestorationDate;

        private int? _partialRestorationMethodID,
                     _partialPavingBreakOutEightInches,
                     _partialPavingBreakOutTenInches,
                     _partialSawCutting,
                     _partialPavingSquareFootage,
                     _daysToPartialPaveHole,
                     _trafficControlHoursPartialRestoration,
                     _finalRestorationMethodID,
                     _finalPavingBreakOutEightInches,
                     _finalPavingBreakOutTenInches,
                     _finalSawCutting,
                     _finalPavingSquareFootage,
                     _daysToFinalPaveHole,
                     _trafficControlHoursFinalRestoration,
                     _approvedByID,
                     _rejectedByID,
                     _responsePriorityID,
                     _assignedContractorID;

        private bool _eightInchStabilizeBaseByCompanyForces,
                      _sawCutByCompanyForces;

        private decimal? _accrualVariance,
                         _partialRestorationActualCost,
                         _finalRestorationActualCost,
                         _totalAccruedCost,
                         _pavingSquareFootage,
                         _linearFeetOfCurb,
                         _restorationSize;

        private EntityRef<RestorationType> _restorationType;
        private EntityRef<WorkOrder> _workOrder;

        private EntityRef<RestorationMethod> _partialRestorationMethod,
                                             _finalRestorationMethod;

        private EntityRef<Employee> _approvedBy, _rejectedBy;

        private EntityRef<RestorationResponsePriority> _responsePriority;

        private RestorationTypeCost _restorationTypeCost;
        private EntityRef<Contractor> _assignedContractor;

        #endregion

        #region Properties

        #region Logical Properties

        public decimal RestorationSize
        {
            get
            {
                if (_restorationSize == null)
                    _restorationSize = (RestorationType.MeasurementType ==
                                        RestorationType.MeasurementTypes.
                                            LinearFt)
                                           ? LinearFeetOfCurb
                                           : PavingSquareFootage;
                return (_restorationSize == null) ? 0 : _restorationSize.Value;
            }
        }

        public decimal? AccrualVariance
        {
            get
            {
                if (_accrualVariance == null)
                    _accrualVariance = CalculateAccrualVariance();
                return _accrualVariance;
            }
        }

        public decimal? AccrualValue
        {
            get { return CalculateAccrualValue(); }
        }

        #endregion

        #region Table Column Properties

        [Column(Storage = "_restorationID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationID
        {
            get { return _restorationID; }
            set
            {
                if (_restorationID != value)
                {
                    SendPropertyChanging();
                    _restorationID = value;
                    SendPropertyChanged("RestorationID");
                }
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

        [Column(Storage = "_restorationTypeID", DbType = "Int NOT NULL")]
        public int RestorationTypeID
        {
            get { return _restorationTypeID; }
            set
            {
                if (_restorationTypeID != value)
                {
                    if (_restorationType.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _restorationTypeID = value;
                SendPropertyChanged("RestorationTypeID");
            }
        }

        [Column(Storage = "_pavingSquareFootage", DbType = "Decimal(9,2)")]
        public decimal? PavingSquareFootage
        {
            get { return _pavingSquareFootage; }
            set
            {
                if (_pavingSquareFootage != value)
                {
                    SendPropertyChanging();
                    _pavingSquareFootage = value;
                    SendPropertyChanged("PavingSquareFootage");
                }
            }
        }

        [Column(Storage = "_linearFeetOfCurb", DbType = "Decimal(9,2)")]
        public decimal? LinearFeetOfCurb
        {
            get { return _linearFeetOfCurb; }
            set
            {
                if (_linearFeetOfCurb != value)
                {
                    SendPropertyChanging();
                    _linearFeetOfCurb = value;
                    SendPropertyChanged("LinearFeetOfCurb");
                }
            }
        }

        [Column(Storage = "_restorationNotes", DbType = "Text", UpdateCheck = UpdateCheck.Never)]
        public string RestorationNotes
        {
            get { return _restorationNotes; }
            set
            {
                if (_restorationNotes != value)
                {
                    SendPropertyChanging();
                    _restorationNotes = value;
                    SendPropertyChanged("RestorationNotes");
                }
            }
        }

        //[Column(Storage = "_partialRestorationMethodID", DbType = "Int")]
        //public int? PartialRestorationMethodID
        //{
        //    get { return _partialRestorationMethodID; }
        //    set
        //    {
        //        if (_partialRestorationMethodID != value)
        //        {
        //            if (_partialRestorationMethod.HasLoadedOrAssignedValue)
        //                throw new ForeignKeyReferenceAlreadyHasValueException();
        //        }
        //        SendPropertyChanging();
        //        _partialRestorationMethodID = value;
        //        SendPropertyChanged("PartialRestorationMethodID");
        //    }
        //}

        [Column(Storage = "_partialRestorationInvoiceNumber", DbType = "VarChar(12) NOT NULL")]
        public string PartialRestorationInvoiceNumber
        {
            get { return _partialRestorationInvoiceNumber; }
            set
            {
                if (value != null && value.Length > MAX_PARTIAL_RESTORATION_INVOICE_NUMBER_LENGTH)
                    throw new StringTooLongException("PartialRestorationInvoiceNumber", MAX_PARTIAL_RESTORATION_INVOICE_NUMBER_LENGTH);
                if (_partialRestorationInvoiceNumber != value)
                {
                    SendPropertyChanging();
                    _partialRestorationInvoiceNumber = value;
                    SendPropertyChanged("PartialRestorationInvoiceNumber");
                }
            }
        }

        [Column(Storage = "_partialRestorationDate", DbType = "SmallDateTime")]
        public DateTime? PartialRestorationDate
        {
            get { return _partialRestorationDate; }
            set
            {
                if (_partialRestorationDate != value)
                {
                    SendPropertyChanging();
                    _partialRestorationDate = value;
                    SendPropertyChanged("PartialRestorationDate");
                }
            }
        }

        [Column(Storage = "_partialRestorationCompletedBy", DbType = "VarChar(10)")]
        public string PartialRestorationCompletedBy
        {
            get { return _partialRestorationCompletedBy; }
            set
            {
                if (value != null && value.Length > MAX_PARTIAL_RESTORATION_COMPLETED_BY_LENGTH)
                    throw new StringTooLongException("PartialRestorationCompletedBy", MAX_PARTIAL_RESTORATION_COMPLETED_BY_LENGTH);
                if (_partialRestorationCompletedBy != value)
                {
                    SendPropertyChanging();
                    _partialRestorationCompletedBy = value;
                    SendPropertyChanged("PartialRestorationCompletedBy");
                }
            }
        }

        [Column(Storage = "_partialPavingBreakOutEightInches", DbType = "Int")]
        public int? PartialPavingBreakOutEightInches
        {
            get { return _partialPavingBreakOutEightInches; }
            set
            {
                if (_partialPavingBreakOutEightInches != value)
                {
                    SendPropertyChanging();
                    _partialPavingBreakOutEightInches = value;
                    SendPropertyChanged("PartialPavingBreakOutEightInches");
                }
            }
        }

        [Column(Storage = "_partialPavingBreakOutTenInches", DbType = "Int")]
        public int? PartialPavingBreakOutTenInches
        {
            get { return _partialPavingBreakOutTenInches; }
            set
            {
                if (_partialPavingBreakOutTenInches != value)
                {
                    SendPropertyChanging();
                    _partialPavingBreakOutTenInches = value;
                    SendPropertyChanged("PartialPavingBreakOutTenInches");
                }
            }
        }

        [Column(Storage = "_partialSawCutting", DbType = "Int")]
        public int? PartialSawCutting
        {
            get { return _partialSawCutting; }
            set
            {
                if (_partialSawCutting != value)
                {
                    SendPropertyChanging();
                    _partialSawCutting = value;
                    SendPropertyChanged("PartialSawCutting");
                }
            }
        }

        [Column(Storage = "_partialPavingSquareFootage", DbType = "Int")]
        public int? PartialPavingSquareFootage
        {
            get { return _partialPavingSquareFootage; }
            set
            {
                if (_partialPavingSquareFootage != value)
                {
                    SendPropertyChanging();
                    _partialPavingSquareFootage = value;
                    SendPropertyChanged("PartialPavingSquareFootage");
                }
            }
        }

        [Column(Storage = "_daysToPartialPaveHole", DbType = "Int")]
        public int? DaysToPartialPaveHole
        {
            get { return _daysToPartialPaveHole; }
            set
            {
                if (_daysToPartialPaveHole != value)
                {
                    SendPropertyChanging();
                    _daysToPartialPaveHole = value;
                    SendPropertyChanged("DaysToPartialPaveHole");
                }
            }
        }

        [Column(Storage = "_trafficControlHoursPartialRestoration", DbType = "Int")]
        public int? TrafficControlCostPartialRestoration
        {
            get { return _trafficControlHoursPartialRestoration; }
            set
            {
                if (_trafficControlHoursPartialRestoration != value)
                {
                    SendPropertyChanging();
                    _trafficControlHoursPartialRestoration = value;
                    SendPropertyChanged("TrafficControlCostPartialRestoration");
                }
            }
        }

        //[Column(Storage = "_finalRestorationMethodID", DbType = "Int")]
        //public int? FinalRestorationMethodID
        //{
        //    get { return _finalRestorationMethodID; }
        //    set
        //    {
        //        if (_finalRestorationMethodID != value)
        //        {
        //            if (_finalRestorationMethod.HasLoadedOrAssignedValue)
        //                throw new ForeignKeyReferenceAlreadyHasValueException();
        //        }
        //        SendPropertyChanging();
        //        _finalRestorationMethodID = value;
        //        SendPropertyChanged("FinalRestorationMethodID");
        //    }
        //}

        [Column(Storage = "_finalRestorationInvoiceNumber", DbType = "VarChar(12)")]
        public string FinalRestorationInvoiceNumber
        {
            get { return _finalRestorationInvoiceNumber; }
            set
            {
                if (value != null && value.Length > MAX_FINAL_RESTORATION_INVOICE_NUMBER_LENGTH)
                    throw new StringTooLongException("FinalRestorationInvoiceNumber", MAX_FINAL_RESTORATION_INVOICE_NUMBER_LENGTH);
                if (_finalRestorationInvoiceNumber != value)
                {
                    SendPropertyChanging();
                    _finalRestorationInvoiceNumber = value;
                    SendPropertyChanged("FinalRestorationInvoiceNumber");
                }
            }
        }

        [Column(Storage = "_finalRestorationDate", DbType = "SmallDateTime")]
        public DateTime? FinalRestorationDate
        {
            get { return _finalRestorationDate; }
            set
            {
                if (_finalRestorationDate != value)
                {
                    SendPropertyChanging();
                    _finalRestorationDate = value;
                    SendPropertyChanged("FinalRestorationDate");
                }
            }
        }

        [Column(Storage = "_finalRestorationCompletedBy", DbType = "VarChar(10)")]
        public string FinalRestorationCompletedBy
        {
            get { return _finalRestorationCompletedBy; }
            set
            {
                if (value != null && value.Length > MAX_FINAL_RESTORATION_COMPLETED_BY_LENGTH)
                    throw new StringTooLongException("FinalRestorationCompletedBy", MAX_FINAL_RESTORATION_COMPLETED_BY_LENGTH);
                if (_finalRestorationCompletedBy != value)
                {
                    SendPropertyChanging();
                    _finalRestorationCompletedBy = value;
                    SendPropertyChanged("FinalRestorationCompletedBy");
                }
            }
        }

        [Column(Storage = "_finalPavingBreakOutEightInches", DbType = "Int")]
        public int? FinalPavingBreakOutEightInches
        {
            get { return _finalPavingBreakOutEightInches; }
            set
            {
                if (_finalPavingBreakOutEightInches != value)
                {
                    SendPropertyChanging();
                    _finalPavingBreakOutEightInches = value;
                    SendPropertyChanged("FinalPavingBreakOutEightInches");
                }
            }
        }

        [Column(Storage = "_finalPavingBreakOutTenInches", DbType = "Int")]
        public int? FinalPavingBreakOutTenInches
        {
            get { return _finalPavingBreakOutTenInches; }
            set
            {
                if (_finalPavingBreakOutTenInches != value)
                {
                    SendPropertyChanging();
                    _finalPavingBreakOutTenInches = value;
                    SendPropertyChanged("FinalPavingBreakOutTenInches");
                }
            }
        }

        [Column(Storage = "_finalSawCutting", DbType = "Int")]
        public int? FinalSawCutting
        {
            get { return _finalSawCutting; }
            set
            {
                if (_finalSawCutting != value)
                {
                    SendPropertyChanging();
                    _finalSawCutting = value;
                    SendPropertyChanged("FinalSawCutting");
                }
            }
        }

        [Column(Storage = "_finalPavingSquareFootage", DbType = "Int")]
        public int? FinalPavingSquareFootage
        {
            get { return _finalPavingSquareFootage; }
            set
            {
                if (_finalPavingSquareFootage != value)
                {
                    SendPropertyChanging();
                    _finalPavingSquareFootage = value;
                    SendPropertyChanged("FinalPavingSquareFootage");
                }
            }
        }

        [Column(Storage = "_daysToFinalPaveHole", DbType = "Int")]
        public int? DaysToFinalPaveHole
        {
            get { return _daysToFinalPaveHole; }
            set
            {
                if (_daysToFinalPaveHole != value)
                {
                    SendPropertyChanging();
                    _daysToFinalPaveHole = value;
                    SendPropertyChanged("DaysToFinalPaveHole");
                }
            }
        }

        [Column(Storage = "_trafficControlHoursFinalRestoration", DbType = "Int")]
        public int? TrafficControlCostFinalRestoration
        {
            get { return _trafficControlHoursFinalRestoration; }
            set
            {
                if (_trafficControlHoursFinalRestoration != value)
                {
                    SendPropertyChanging();
                    _trafficControlHoursFinalRestoration = value;
                    SendPropertyChanged("TrafficControlCostFinalRestoration");
                }
            }
        }

        [Column(Storage = "_finalRestorationApprovedAt", DbType = "SmallDateTime")]
        public DateTime? FinalRestorationApprovedAt
        {
            get { return _finalRestorationApprovedAt; }
            set
            {
                if (_finalRestorationApprovedAt != value)
                {
                    SendPropertyChanging();
                    _finalRestorationApprovedAt = value;
                    SendPropertyChanged("FinalRestorationApprovedAt");
                }
            }
        }

        [Column(Storage = "_approvedByID", DbType = "Int")]
        public int? ApprovedByID
        {
            get { return _approvedByID; }
            set
            {
                if (_approvedByID != value)
                {
                    if (_approvedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _approvedByID = value;
                SendPropertyChanged("ApprovedByID");
            }
        }

        [Column(Storage = "_rejectedByID", DbType = "Int")]
        public int? RejectedByID
        {
            get { return _rejectedByID; }
            set
            {
                if (_rejectedByID != value)
                {
                    if (_rejectedBy.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _rejectedByID = value;
                SendPropertyChanged("RejectedByID");
            }
        }

        [Column(Storage = "_dateRejected", DbType = "SmallDateTime")]
        public DateTime? DateRejected
        {
            get { return _dateRejected; }
            set
            {
                if (_dateRejected != value)
                {
                    SendPropertyChanging();
                    _dateRejected = value;
                    SendPropertyChanged("DateRejected");
                }
            }
        }

        [Column(Storage = "_eightInchStabilizeBaseByCompanyForces", DbType = "Bit NOT NULL")]
        public bool EightInchStabilizeBaseByCompanyForces
        {
            get { return _eightInchStabilizeBaseByCompanyForces; }
            set
            {
                if (_eightInchStabilizeBaseByCompanyForces != value)
                {
                    SendPropertyChanging();
                    _eightInchStabilizeBaseByCompanyForces = value;
                    SendPropertyChanged("EightInchStabilizeBaseByCompanyForces");
                }
            }
        }

        [Column(Storage = "_sawCutByCompanyForces", DbType = "Bit NOT NULL")]
        public bool SawCutByCompanyForces
        {
            get { return _sawCutByCompanyForces; }
            set
            {
                if (_sawCutByCompanyForces != value)
                {
                    SendPropertyChanging();
                    _sawCutByCompanyForces = value;
                    SendPropertyChanged("SawCutByCompanyForces");
                }
            }
        }

        [Column(Storage="_totalAccruedCost", DbType="Decimal(18,2) NULL")]
        public decimal? TotalAccruedCost
        {
            get
            {
                return _totalAccruedCost;
            }
            set
            {
                if (_totalAccruedCost != value)
                {
                    SendPropertyChanging();
                    _totalAccruedCost = value;
                    SendPropertyChanged("TotalAccruedCost");
                }
            }
        }

        [Column(Storage= "_partialRestorationActualCost", DbType="Decimal(18,2) NULL")]
        public decimal? PartialRestorationActualCost
        {
            get { return _partialRestorationActualCost; }
            set
            {
                if (_partialRestorationActualCost != value)
                {
                    SendPropertyChanging();
                    _partialRestorationActualCost = value;
                    SendPropertyChanged("PartialRestorationActualCost");
                }
            }
        }

        [Column(Storage = "_finalRestorationActualCost", DbType = "Decimal(18,2) NULL")]
        public decimal? FinalRestorationActualCost
        {
            get { return _finalRestorationActualCost; }
            set
            {
                if (_finalRestorationActualCost != value)
                {
                    SendPropertyChanging();
                    _finalRestorationActualCost = value;
                    SendPropertyChanged("FinalRestorationActualCost");
                }
            }
        }

        [Column(Storage = "_responsePriorityID", DbType = "Int")]
        public int? ResponsePriorityID
        {
            get { return _responsePriorityID; }
            set
            {
                if (_responsePriorityID != value)
                {
                    if (_responsePriority.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _responsePriorityID = value;
                SendPropertyChanged("ResponsePriorityID");
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "RestorationType_Restoration", Storage = "_restorationType", ThisKey = "RestorationTypeID", IsForeignKey = true)]
        public RestorationType RestorationType
        {
            get { return _restorationType.Entity; }
            set
            {
                var previousValue = _restorationType.Entity;
                if ((previousValue != value)
                    || (_restorationType.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _restorationType.Entity = null;
                        previousValue.Restorations.Remove(this);
                    }
                    _restorationType.Entity = value;
                    if (value != null)
                    {
                        value.Restorations.Add(this);
                        _restorationTypeID = value.RestorationTypeID;
                    }
                    else
                        _restorationTypeID = default(int);
                    SendPropertyChanged("RestorationType");
                }
            }
        }

        [Association(Name = "WorkOrder_Restoration", Storage = "_workOrder", ThisKey = "WorkOrderID", IsForeignKey = true)]
        public WorkOrder WorkOrder
        {
            get { return _workOrder.Entity; }
            set
            {
                var previousValue = _workOrder.Entity;
                if ((previousValue != value)
                    || (_workOrder.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _workOrder.Entity = null;
                        previousValue.Restorations.Remove(this);
                    }
                    _workOrder.Entity = value;
                    if (value != null)
                    {
                        value.Restorations.Add(this);
                        _workOrderID = value.WorkOrderID;
                    }
                    else
                        _workOrderID = default(int);
                    SendPropertyChanged("WorkOrder");
                }
            }
        }

        //[Association(Name = "PartialRestorationMethod_Restoration", Storage = "_partialRestorationMethod", ThisKey = "PartialRestorationMethodID", IsForeignKey = true)]
        //public RestorationMethod PartialRestorationMethod
        //{
        //    get { return _partialRestorationMethod.Entity; }
        //    set
        //    {
        //        var previousValue = _partialRestorationMethod.Entity;
        //        if ((previousValue != value)
        //            || (_partialRestorationMethod.HasLoadedOrAssignedValue == false))
        //        {
        //            SendPropertyChanging();
        //            if (previousValue != null)
        //            {
        //                _partialRestorationMethod.Entity = null;
        //                previousValue.PartialRestorations.Remove(this);
        //            }
        //            _partialRestorationMethod.Entity = value;
        //            if (value != null)
        //            {
        //                value.PartialRestorations.Add(this);
        //                _partialRestorationMethodID = value.RestorationMethodID;
        //            }
        //            else
        //                _partialRestorationMethodID = default(int);
        //            SendPropertyChanged("PartialRestorationMethod");
        //        }
        //    }
        //}

        //[Association(Name = "FinalRestorationMethod_Restoration", Storage = "_finalRestorationMethod", ThisKey = "FinalRestorationMethodID", IsForeignKey = true)]
        //public RestorationMethod FinalRestorationMethod
        //{
        //    get { return _finalRestorationMethod.Entity; }
        //    set
        //    {
        //        var previousValue = _finalRestorationMethod.Entity;
        //        if ((previousValue != value)
        //            || (_finalRestorationMethod.HasLoadedOrAssignedValue == false))
        //        {
        //            SendPropertyChanging();
        //            if (previousValue != null)
        //            {
        //                _finalRestorationMethod.Entity = null;
        //                previousValue.FinalRestorations.Remove(this);
        //            }
        //            _finalRestorationMethod.Entity = value;
        //            if (value != null)
        //            {
        //                value.FinalRestorations.Add(this);
        //                _finalRestorationMethodID = value.RestorationMethodID;
        //            }
        //            else
        //                _finalRestorationMethodID = default(int);
        //            SendPropertyChanged("FinalRestorationMethod");
        //        }
        //    }
        //}

        [Association(Name = "ApprovedBy_Restoration", Storage = "_approvedBy", ThisKey = "ApprovedByID", IsForeignKey = true)]
        public Employee ApprovedBy
        {
            get { return _approvedBy.Entity; }
            set
            {
                Employee previousValue = _approvedBy.Entity;
                if ((previousValue != value)
                    || (_approvedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _approvedBy.Entity = null;
                        previousValue.RestorationsApproved.Remove(this);
                    }
                    _approvedBy.Entity = value;
                    if (value != null)
                    {
                        value.RestorationsApproved.Add(this);
                        _approvedByID = value.EmployeeID;
                    }
                    else
                        _approvedByID = default(int);
                    SendPropertyChanged("ApprovedBy");
                }
            }
        }

        [Association(Name = "RejectedBy_Restoration", Storage = "_rejectedBy", ThisKey = "RejectedByID", IsForeignKey = true)]
        public Employee RejectedBy
        {
            get { return _rejectedBy.Entity; }
            set
            {
                Employee previousValue = _rejectedBy.Entity;
                if ((previousValue != value)
                    || (_rejectedBy.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _rejectedBy.Entity = null;
                        previousValue.RestorationsRejected.Remove(this);
                    }
                    _rejectedBy.Entity = value;
                    if (value != null)
                    {
                        value.RestorationsRejected.Add(this);
                        _rejectedByID = value.EmployeeID;
                    }
                    else
                        _rejectedByID = default(int);
                    SendPropertyChanged("RejectedBy");
                }
            }
        }

        [Association(Name = "ResponsePriority_Restoration", Storage = "_responsePriority", ThisKey = "ResponsePriorityID", IsForeignKey = true)]
        public RestorationResponsePriority ResponsePriority
        {
            get { return _responsePriority.Entity; }
            set
            {
                RestorationResponsePriority previousValue = _responsePriority.Entity;
                if ((previousValue != value)
                    || (_responsePriority.HasLoadedOrAssignedValue == false))
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _responsePriority.Entity = null;
                        previousValue.Restorations.Remove(this);
                    }
                    _responsePriority.Entity = value;
                    if (value != null)
                    {
                        value.Restorations.Add(this);
                        _responsePriorityID = value.RestorationResponsePriorityID;
                    }
                    else
                        _responsePriorityID = default(int);
                    SendPropertyChanged("ResponsePriority");
                }
            }
        }

        public RestorationTypeCost RestorationTypeCost
        {
            get
            {
                if (_restorationTypeCost == null)
                    _restorationTypeCost = (from rtc in RestorationType.RestorationTypeCosts
                                            where rtc != null && rtc.OperatingCenterID == WorkOrder.OperatingCenterID
                                            select rtc).FirstOrDefault();
                return _restorationTypeCost;
            }
            set { _restorationTypeCost = value; }
        }

        [Column(Storage = "_assignedContractorID", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? AssignedContractorID
        {
            get { return _assignedContractorID; }
            set
            {
                if (_assignedContractorID != value)
                {
                    if (_assignedContractor.HasLoadedOrAssignedValue)
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                }
                SendPropertyChanging();
                _assignedContractorID = value;
                SendPropertyChanged("AssignedContractorID");
            }
        }

        [Association(Name = "AssignedContractor_Restoration", Storage = "_assignedContractor", ThisKey = "AssignedContractorID", IsForeignKey = true)]
        public Contractor AssignedContractor
        {
            get { return _assignedContractor.Entity; }
            //set
            //{
            //    var previousValue = _assignedContractor.Entity;
            //    if ((previousValue != value) || _assignedContractor.HasLoadedOrAssignedValue == false)
            //    {
            //        SendPropertyChanging();
            //        if (previousValue != null)
            //        {
            //            _assignedContractor.Entity = null;
            //            previousValue.AssignedWorkOrders.Remove(this);
            //        }
            //        _assignedContractor.Entity = value;
            //        if (value != null)
            //        {
            //            value.AssignedWorkOrders.Add(this);
            //            _assignedContractorID = value.ContractorID;
            //        }
            //        else
            //            _assignedContractorID = default(int);
            //        SendPropertyChanged("AssignedContractor");
            //    }
            //}
        }

        #endregion

        #endregion

        #region Constructors

        public Restoration()
        {
        }

        #endregion

        #region Private Methods
        
        protected void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (WorkOrder == null)
                        throw new DomainLogicException("Cannot save a Restoration record without a WorkOrder.");
                    // Always recalculate and save the Total Accrued Cost.
                    if (RestorationType != null && RestorationType.Description != null)
                        TotalAccruedCost = CalculateAccruedCost();
                    if (ResponsePriorityID == null || ResponsePriorityID == default(int))
                        _responsePriorityID =
                            RestorationResponsePriorityRepository.Indices.
                                STANDARD;
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

        private decimal CalculateAccruedCost()
        {
            var baseCost = (RestorationType.MeasurementType ==
                            RestorationType.MeasurementTypes.LinearFt)
                               ? LinearFeetOfCurb : PavingSquareFootage;
            baseCost = baseCost ?? 0;
            var multiplier = (from rtc in RestorationType.RestorationTypeCosts
                              where
                                  rtc != null &&
                                  rtc.OperatingCenterID ==
                                  WorkOrder.OperatingCenterID
                              select rtc).FirstOrDefault();
            if (multiplier != null)
                return baseCost.Value * (decimal)multiplier.Cost;
            return 0;
        }
        
        private decimal? CalculateAccrualVariance()
        {
            return (TotalAccruedCost -
                    (PartialRestorationActualCost + FinalRestorationActualCost));
        }

        private decimal? CalculateAccrualValue()
        {
            if (PartialRestorationActualCost == null ||
                PartialRestorationActualCost == 0)
            {
                return TotalAccruedCost;
            }

            // Bug 3757 requested this be calculated as follows:
            if (PartialPavingSquareFootage <= 0)
            {
                return RestorationSize * RestorationTypeCost.FinalCost;
            }
            else
            {
                return PartialPavingSquareFootage * RestorationTypeCost.FinalCost;
            }
        }

        #endregion

        #region Exposed Static Methods

        // TODO: Test this, or put a bullet in it
        // it only exists for the accrual report.  they don't want the
        // fields to actually have default values, they just don't
        // want empty fields on the report.
        public static decimal GetAccrualVariance(Restoration restoration)
        {
            return ((restoration.TotalAccruedCost ?? 0) -
                    ((restoration.PartialRestorationActualCost ?? 0) +
                     (restoration.FinalRestorationActualCost ?? 0)));
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
