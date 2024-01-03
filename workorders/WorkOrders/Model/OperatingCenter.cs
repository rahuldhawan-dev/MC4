using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.OperatingCenters")]
    public class OperatingCenter : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private bool _workOrdersEnabled, _hasWorkOrderInvoicing, _sapEnabled, _sapWorkOrdersEnabled, _isContractedOperations, _markoutsEditable;

        private string _coInfo,
                       _cSNum,
                       _faxNum,
                       _mailAdd,
                       _mailCo,
                       _mailCSZ,
                       _opCntr,
                       _opCntrName,
                       _servContactNum,
                       _hydInspFreq,
                       _hydInspFreqUnit,
                       _valLgInspFreq,
                       _valLgInspFreqUnit,
                       _valSmInspFreq,
                       _valSmInspFreqUnit,
                       _permitsOMUserName,
                       _permitsCapitalUserName,
                       _dataCollectionMapUrl;

        private int _recID;

        private int? _operatedByOperatingCenterId;

        private EntityRef<OperatingCenterSpoilRemovalCost>
            _operatingCenterSpoilRemovalCost;

        private EntityRef<OperatingCenter>
            _operatedByOperatingCenter;

        private int _stateId;
        private EntityRef<State> _state;

        private readonly EntitySet<OperatingCenterStockedMaterial> _operatingCenterStockedMaterials;

        private readonly EntitySet<OperatingCenterTown> _operatingCentersTowns;
        private readonly EntitySet<ContractorOperatingCenter> _contractorsOperatingCenters;

        private readonly EntitySet<OperatingCenterUser> _operatingCentersUsers;

        private readonly EntitySet<Crew> _crews;

        private readonly EntitySet<RestorationTypeCost> _restorationTypeCosts;

        private readonly EntitySet<OperatingCenterAssetType> _operatingCenterAssetTypes;

        private readonly EntitySet<StockLocation> _stocklocations;

        private readonly EntitySet<Employee> _employees;

        private readonly EntitySet<WorkOrder> _workOrders;

        private readonly EntitySet<SewerOpening> _seweropenings;

        private readonly EntitySet<StormCatch> _stormCatches;

        //private readonly EntitySet<Equipment> _equipments;

        private readonly EntitySet<SpoilStorageLocation> _spoilStorageLocations;

        private readonly EntitySet<SpoilFinalProcessingLocation> _spoilFinalProcessingLocations;

        private readonly EntitySet<BusinessUnit> _businessUnits;

        private readonly EntitySet<SewerOverflow> _sewerOverflows;

        private EntitySet<Town> _towns;
        private EntitySet<Contractor> _contractors;

        private EntitySet<Employee> _operatingCenterEmployees;

        private EntitySet<Hydrant> _hydrants;
        private EntitySet<Valve> _valves;

        #endregion

        #region Properties

        #region Logical Properties

        public string FullDescription => OpCntr + " - " + OpCntrName;

        public EntitySet<Town> Towns
        {
            get
            {
                if (_towns == null)
                {
                    _towns = new EntitySet<Town>(onTownsAdd, onTownsRemove);
                    _towns.SetSource(
                        OperatingCentersTowns.Select(t => t.Town).OrderBy(
                            t => t.Name));
                }
                return _towns;
            }
        }
        
        public EntitySet<Contractor> Contractors
        {
            get
            {
                if (_contractors == null)
                {
                    _contractors = new EntitySet<Contractor>(onContractorsAdd,onContractorsRemove);
                    _contractors.SetSource(
                        ContractorsOperatingCenters
                            .Select(c => c.Contractor)
                            .OrderBy(c => c.Name)
                            .Where(
                                c =>
                                c.ContractorsAccess != null &&
                                c.ContractorsAccess.Value));
                }
                return _contractors;
            }
        }


        /// <summary>
        /// List of all the employees that have a given operating center in their 
        /// OperatingCentersUsers collection.
        /// </summary>
        public EntitySet<Employee> AllEmployees
        {
            get
            {
                if (_operatingCenterEmployees == null)
                {
                    _operatingCenterEmployees = new EntitySet<Employee>(onAllEmployeesAdd, onAllEmployeesRemove);
                    _operatingCenterEmployees.SetSource(
                        OperatingCentersUsers.Select(e => e.Employee).OrderBy(
                            e => e.FullName));
                }
                return _operatingCenterEmployees;
            }
        }

        #endregion

        #region Table Column Properties

        [Column(Storage = "_operatedByOperatingCenterId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int? OperatedByOperatingCenterID
        {
            get => _operatedByOperatingCenterId;
            set
            {
                SendPropertyChanging();
                _operatedByOperatingCenterId = value;
                SendPropertyChanged("OperatedByOperatingCenterID");
            }
        }

        [Column(Storage = "_stateId", DbType = "Int", UpdateCheck = UpdateCheck.Never)]
        public int StateID
        {
            get => _stateId;
            set
            {
                SendPropertyChanging();
                _stateId = value;
                SendPropertyChanged("StateID");
            }
        }


        [Column(Storage = "_workOrdersEnabled", DbType = "Bit NOT NULL")]
        public bool WorkOrdersEnabled
        {
            get => _workOrdersEnabled;
            set
            {
                if ((_workOrdersEnabled != value))
                {
                    SendPropertyChanging();
                    _workOrdersEnabled = value;
                    SendPropertyChanged("WorkOrdersEnabled");
                }
            }
        }

        [Column(Storage = "_hasWorkOrderInvoicing", DbType = "Bit NOT NULL")]
        public bool HasWorkOrderInvoicing
        {
            get => _hasWorkOrderInvoicing;
            set
            {
                if (_hasWorkOrderInvoicing != value)
                {
                    SendPropertyChanging();
                    _hasWorkOrderInvoicing = value;
                    SendPropertyChanged("HasWorkOrderInvoicing");
                }
            }
        }

        [Column(Storage = "_sapEnabled", DbType = "Bit NOT NULL")]
        public bool SAPEnabled
        {
            get => _sapEnabled;
            set
            {
                if (_sapEnabled != value)
                {
                    SendPropertyChanging();
                    _sapEnabled = value;
                    SendPropertyChanged("SAPEnabled");
                }
            }
        }

        [Column(Storage = "_markoutsEditable", DbType = "Bit NOT NULL")]
        public bool MarkoutsEditable
        {
            get => _markoutsEditable;
            set
            {
                if (_markoutsEditable != value)
                {
                    SendPropertyChanging();
                    _markoutsEditable = value;
                    SendPropertyChanged("MarkoutsEditable");
                }
            }
        }

        //apWorkOrdersEnabled
        [Column(Storage = "_sapWorkOrdersEnabled", DbType = "Bit NOT NULL")]
        public bool SAPWorkOrdersEnabled
        {
            get => _sapWorkOrdersEnabled;
            set
            {
                if (_sapWorkOrdersEnabled != value)
                {
                    SendPropertyChanging();
                    _sapWorkOrdersEnabled = value;
                    SendPropertyChanged("SAPWorkOrdersEnabled");
                }
            }
        }

        [Column(Storage = "_isContractedOperations", DbType = "Bit NOT NULL")]
        public bool IsContractedOperations
        {
            get => _isContractedOperations;
            set
            {
                if (_isContractedOperations != value)
                {
                    SendPropertyChanging();
                    _isContractedOperations = value;
                    SendPropertyChanged("IsContractedOperations");
                }
            }
        }

        [Column(Storage = "_coInfo", DbType = "VarChar(65)")]
        public string CoInfo
        {
            get => _coInfo;
            set
            {
                if ((_coInfo != value))
                {
                    SendPropertyChanging();
                    _coInfo = value;
                    SendPropertyChanged("CoInfo");
                }
            }
        }

        [Column(Storage = "_cSNum", DbType = "VarChar(12)")]
        public string CSNum
        {
            get => _cSNum;
            set
            {
                if ((_cSNum != value))
                {
                    SendPropertyChanging();
                    _cSNum = value;
                    SendPropertyChanged("CSNum");
                }
            }
        }

        [Column(Storage = "_faxNum", DbType = "VarChar(12)")]
        public string FaxNum
        {
            get => _faxNum;
            set
            {
                if ((_faxNum != value))
                {
                    SendPropertyChanging();
                    _faxNum = value;
                    SendPropertyChanged("FaxNum");
                }
            }
        }

        [Column(Storage = "_mailAdd", DbType = "VarChar(30)")]
        public string MailAdd
        {
            get => _mailAdd;
            set
            {
                if ((_mailAdd != value))
                {
                    SendPropertyChanging();
                    _mailAdd = value;
                    SendPropertyChanged("MailAdd");
                }
            }
        }

        [Column(Storage = "_mailCo", DbType = "VarChar(30)")]
        public string MailCo
        {
            get => _mailCo;
            set
            {
                if ((_mailCo != value))
                {
                    SendPropertyChanging();
                    _mailCo = value;
                    SendPropertyChanged("MailCo");
                }
            }
        }

        [Column(Storage = "_mailCSZ", DbType = "VarChar(30)")]
        public string MailCSZ
        {
            get => _mailCSZ;
            set
            {
                if ((_mailCSZ != value))
                {
                    SendPropertyChanging();
                    _mailCSZ = value;
                    SendPropertyChanged("MailCSZ");
                }
            }
        }

        [Column(Name = "OperatingCenterCode", Storage = "_opCntr", DbType = "VarChar(4)")]
        public string OpCntr
        {
            get => _opCntr;
            set
            {
                if ((_opCntr != value))
                {
                    SendPropertyChanging();
                    _opCntr = value;
                    SendPropertyChanged("OpCntr");
                }
            }
        }

        [Column(Name = "OperatingCenterName", Storage = "_opCntrName", DbType = "VarChar(30)")]
        public string OpCntrName
        {
            get => _opCntrName;
            set
            {
                if ((_opCntrName != value))
                {
                    SendPropertyChanging();
                    _opCntrName = value;
                    SendPropertyChanged("OpCntrName");
                }
            }
        }

        [Column(Name = "OperatingCenterID", Storage = "_recID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int OperatingCenterID
        {
            get => _recID;
            set
            {
                if ((_recID != value))
                {
                    SendPropertyChanging();
                    _recID = value;
                    SendPropertyChanged("RecID");
                }
            }
        }

        [Column(Storage = "_servContactNum", DbType = "VarChar(12)")]
        public string ServContactNum
        {
            get => _servContactNum;
            set
            {
                if ((_servContactNum != value))
                {
                    SendPropertyChanging();
                    _servContactNum = value;
                    SendPropertyChanged("ServContactNum");
                }
            }
        }
        
        [Column(Storage = "_permitsOMUserName", DbType = "VarChar(50)")]
        public string PermitsOMUserName
        {
            get => _permitsOMUserName;
            set
            {
                if ((_permitsOMUserName != value))
                {
                    SendPropertyChanging();
                    _permitsOMUserName = value;
                    SendPropertyChanged("PermitsOMUserName");
                }
            }
        }

        [Column(Storage = "_permitsCapitalUserName", DbType = "VarChar(50)")]
        public string PermitsCapitalUserName
        {
            get => _permitsCapitalUserName;
            set
            {
                if ((_permitsCapitalUserName != value))
                {
                    SendPropertyChanging();
                    _permitsCapitalUserName = value;
                    SendPropertyChanged("PermitsCapitalUserName");
                }
            }
        }

        [Column(Storage = "_dataCollectionMapUrl", DbType = "VarChar(50)")]
        public string DataCollectionMapUrl
        {
            get => _dataCollectionMapUrl;
            set
            {
                if ((_dataCollectionMapUrl != value))
                {
                    SendPropertyChanging();
                    _dataCollectionMapUrl = value;
                    SendPropertyChanged("DataCollectionMapUrl");
                }
            }
        }

        #endregion

        #region Association Properties

        [Association(Name = "OperatingCenter_OperatingCenterSpoilRemovalCost", Storage = "_operatingCenterSpoilRemovalCost", OtherKey = "OperatingCenterID")]
        public OperatingCenterSpoilRemovalCost OperatingCenterSpoilRemovalCost
        {
            get => _operatingCenterSpoilRemovalCost.Entity;
            set
            {
                var previousValue = _operatingCenterSpoilRemovalCost.Entity;
                if ((previousValue != value)
                    || _operatingCenterSpoilRemovalCost.HasLoadedOrAssignedValue == false)
                {
                    SendPropertyChanging();
                    if (previousValue != null)
                    {
                        _operatingCenterSpoilRemovalCost.Entity = null;
                        previousValue.OperatingCenter = null;
                    }
                    _operatingCenterSpoilRemovalCost.Entity = value;
                    if (value != null)
                    {
                        value.OperatingCenter = this;
                    }
                    SendPropertyChanged("OperatingCenterSpoilRemovalCost");
                }

            }
        }

        [Association(Name = "OperatingCenter_OperatedByOperatingCenter",
             Storage = "_operatedByOperatingCenter",
             ThisKey = "OperatedByOperatingCenterID")]
        public OperatingCenter OperatedByOperatingCenter => _operatedByOperatingCenter.Entity;

        [Association(Name = "OperatingCenter_State", Storage = "_state", ThisKey = "StateID")]
        public State State
        {
            get => _state.Entity;
            set => _state.Entity = value;
        }


        [Association(Name = "OperatingCenter_OperatingCenterStockedMaterial", Storage = "_operatingCenterStockedMaterials", OtherKey = "OperatingCenterID")]
        public EntitySet<OperatingCenterStockedMaterial> OperatingCenterStockedMaterials
        {
            get => _operatingCenterStockedMaterials;
            set => _operatingCenterStockedMaterials.Assign(value);
        }

        [Association(Name = "OperatingCenter_OperatingCentersTowns", Storage = "_operatingCentersTowns", OtherKey="OperatingCenterID")]
        public EntitySet<OperatingCenterTown> OperatingCentersTowns
        {
            get => _operatingCentersTowns;
            set => _operatingCentersTowns.Assign(value);
        }

        [Association(Name = "OperatingCenter_ContractorsOperatingCenters", Storage = "_contractorsOperatingCenters", OtherKey="OperatingCenterID")]
        public EntitySet<ContractorOperatingCenter> ContractorsOperatingCenters
        {
            get => _contractorsOperatingCenters;
            set => _contractorsOperatingCenters.Assign(value);
        }

        [Association(Name = "OperatingCenter_OperatingCentersUsers", Storage = "_operatingCentersUsers", OtherKey = "OperatingCenterID")]
        public EntitySet<OperatingCenterUser> OperatingCentersUsers
        {
            get => _operatingCentersUsers;
            set => _operatingCentersUsers.Assign(value);
        }

        [Association(Name = "OperatingCenter_SewerOverflow",
            Storage = "_sewerOverflows", OtherKey = "OperatingCenterId")]
        public EntitySet<SewerOverflow> SewerOverflows
        {
            get => _sewerOverflows;
            set => _sewerOverflows.Assign(value);
        }

        [Association(Name = "OperatingCenter_OperatingCenterAssetType", Storage = "_operatingCenterAssetTypes", OtherKey = "OperatingCenterID")]
        public EntitySet<OperatingCenterAssetType> OperatingCenterAssetTypes
        {
            get => _operatingCenterAssetTypes;
            set => _operatingCenterAssetTypes.Assign(value);
        }

        [Association(Name = "OperatingCenter_Crew", Storage = "_crews", OtherKey = "OperatingCenterID")]
        public EntitySet<Crew> Crews
        {
            get => _crews;
            set => _crews.Assign(value);
        }

        [Association(Name = "OperatingCenter_RestorationTypeCost", Storage = "_restorationTypeCosts", OtherKey = "OperatingCenterID")]
        public EntitySet<RestorationTypeCost> RestorationTypeCosts
        {
            get => _restorationTypeCosts;
            set => _restorationTypeCosts.Assign(value);
        }

        [Association(Name = "OperatingCenter_StockLocation", Storage = "_stocklocations", OtherKey = "OperatingCenterID")]
        public EntitySet<StockLocation> StockLocations
        {
            get => _stocklocations;
            set => _stocklocations.Assign(value);
        }

        [Association(Name = "OperatingCenter_Employee", Storage = "_employees", OtherKey = "DefaultOperatingCenterID")]
        public EntitySet<Employee> Employees
        {
            get => _employees;
            set => _employees.Assign(value);
        }

        [Association(Name = "OperatingCenter_WorkOrder", Storage = "_workOrders", OtherKey = "OperatingCenterID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get => _workOrders;
            set => _workOrders.Assign(value);
        }

        [Association(Name = "OperatingCenter_SewerOpening", Storage = "_seweropenings", OtherKey = "OperatingCenterID")]
        public EntitySet<SewerOpening> SewerOpenings
        {
            get => _seweropenings;
            set => _seweropenings.Assign(value);
        }

        [Association(Name = "OperatingCenter_StormCatch", Storage = "_stormCatches", OtherKey = "OperatingCenterID")]
        public EntitySet<StormCatch> StormCatches
        {
            get => _stormCatches;
            set => _stormCatches.Assign(value);
        }

        //[Association(Name = "OperatingCenter_Equipments", Storage = "_equipments", OtherKey = "OperatingCenterID")]
        //public EntitySet<Equipment> Equipments
        //{
        //    get { return _equipments; }
        //    set { _equipments.Assign(value); }
        //}

        [Association(Name = "OperatingCenter_SpoilStorageLocation", Storage = "_spoilStorageLocations", OtherKey = "OperatingCenterID")]
        public EntitySet<SpoilStorageLocation> SpoilStorageLocations
        {
            get => _spoilStorageLocations;
            set => _spoilStorageLocations.Assign(value);
        }

        [Association(Name = "OperatingCenter_SpoilFinalProcessingLocation", Storage = "_spoilFinalProcessingLocations", OtherKey = "OperatingCenterID")]
        public EntitySet<SpoilFinalProcessingLocation> SpoilFinalProcessingLocations
        {
            get => _spoilFinalProcessingLocations;
            set => _spoilFinalProcessingLocations.Assign(value);
        }

        [Association(Name = "OperatingCenter_BusinessUnit", Storage = "_businessUnits", OtherKey="OperatingCenterID")]
        public EntitySet<BusinessUnit> BusinessUnits
        {
            get => _businessUnits;
            set => _businessUnits.Assign(value);
        }

        [Association(Name = "OperatingCenter_Hydrants", Storage = "_hydrants", OtherKey = "OperatingCenterID")]
        public EntitySet<Hydrant> Hydrants
        {
            get => _hydrants;
            set => _hydrants.Assign(value);
        }

        [Association(Name = "OperatingCenter_Valves", Storage = "_valves", OtherKey = "OperatingCenterID")]
        public EntitySet<Valve> Valves
        {
            get => _valves;
            set => _valves.Assign(value);
        }

        #endregion

        #region Logical Properties

        public virtual string PermitsUserName => !string.IsNullOrWhiteSpace(PermitsOMUserName) ? PermitsOMUserName : PermitsCapitalUserName;

        [Column(DbType = "VarChar(32)")]
        public string MapId { get; set; }

        [Column(DbType = "VarChar(32)")]
        public string ArcMobileMapId { get; set; }

        #endregion

        #endregion

        #region Constructors

        public OperatingCenter()
        {
            _sewerOverflows = new EntitySet<SewerOverflow>();
            _operatingCenterStockedMaterials =
                new EntitySet<OperatingCenterStockedMaterial>(
                    attach_OperatingCenterStockedMaterials,
                    detach_OperatingCenterStockedMaterials);
            _operatingCentersTowns = new EntitySet<OperatingCenterTown>(
                    attach_OperatingCentersTowns,
                    detach_OperatingCentersTowns
                );
            
            _contractorsOperatingCenters = new EntitySet<ContractorOperatingCenter>(
                    attach_ContractorsOperatingCenters,
                    detach_ContractorsOperatingCenters
                );
            _operatingCentersUsers = new EntitySet<OperatingCenterUser>(
                    attach_OperatingCentersUsers,
                    detach_OperatingCentersUsers
                );
            _restorationTypeCosts =
                new EntitySet<RestorationTypeCost>(
                    attach_RestorationTypeCosts,
                    detach_RestorationTypeCosts);
            _crews = new EntitySet<Crew>(attach_Crews, detach_Crews);
            _stocklocations = new EntitySet<StockLocation>(
                attach_StockLocations, detach_StockLocations);
            _employees = new EntitySet<Employee>(
                attach_Employee, detach_Employee);
            _seweropenings = new EntitySet<SewerOpening>(
                attach_SewerOpenings,
                detach_SewerOpenings);
            _stormCatches = new EntitySet<StormCatch>(
                attach_StormCatches,
                detach_StormCatches);
            //_equipments = new EntitySet<Equipment>(attach_Equipments, detach_Equipments);
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
            _operatingCenterAssetTypes =
                new EntitySet<OperatingCenterAssetType>(
                    attach_OperatingCenterAssetTypes,
                    detach_OperatingCenterAssetTypes);
            _spoilStorageLocations =
                new EntitySet<SpoilStorageLocation>(
                    attach_SpoilStorageLocations,
                    detach_SpoilStorageLocations);
            _spoilFinalProcessingLocations =
                new EntitySet<SpoilFinalProcessingLocation>(
                    attach_SpoilFinalProcessingLocations,
                    detach_SpoilFinalProcessingLocations);
            _businessUnits = 
                new EntitySet<BusinessUnit>(
                    attach_BusinessUnits, 
                    detach_BusinessUnits);
            _valves = new EntitySet<Valve>(
                attach_Valves,
                detach_Valves);
            _hydrants = new EntitySet<Hydrant>(
                attach_Hydrants,
                detach_Hydrants);
        }

        #endregion

        #region Private Methods

        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_OperatingCenterStockedMaterials(OperatingCenterStockedMaterial entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_OperatingCenterStockedMaterials(OperatingCenterStockedMaterial entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_OperatingCentersTowns(OperatingCenterTown entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_OperatingCentersTowns(OperatingCenterTown entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_ContractorsOperatingCenters(ContractorOperatingCenter entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }

        private void detach_ContractorsOperatingCenters(ContractorOperatingCenter entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_OperatingCentersUsers(OperatingCenterUser entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_OperatingCentersUsers(OperatingCenterUser entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_OperatingCenterAssetTypes(OperatingCenterAssetType entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_OperatingCenterAssetTypes(OperatingCenterAssetType entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_Crews(Crew entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_Crews(Crew entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_RestorationTypeCosts(RestorationTypeCost entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_RestorationTypeCosts(RestorationTypeCost entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_StockLocations(StockLocation entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_StockLocations(StockLocation entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_Employee(Employee entity)
        {
            SendPropertyChanging();
            entity.DefaultOperatingCenter = this;
        }
        private void detach_Employee(Employee entity)
        {
            SendPropertyChanging();
            entity.DefaultOperatingCenter = null;
        }

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_SewerOpenings(SewerOpening entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_StormCatches(StormCatch entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        //private void attach_Equipments(Equipment entity)
        //{
        //    SendPropertyChanging();
        //    entity.OperatingCenter = this;
        //}

        //private void detach_Equipments(Equipment entity)
        //{
        //    SendPropertyChanging();
        //    entity.OperatingCenter = null;
        //}

        private void attach_SpoilStorageLocations(SpoilStorageLocation entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_SpoilStorageLocations(SpoilStorageLocation entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_SpoilFinalProcessingLocations(SpoilFinalProcessingLocation entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }
        private void detach_SpoilFinalProcessingLocations(SpoilFinalProcessingLocation entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_BusinessUnits(BusinessUnit entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;   
        }
        private void detach_BusinessUnits(BusinessUnit entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void onTownsAdd(Town obj)
        {
            OperatingCentersTowns.Add(new OperatingCenterTown {
                Town = obj,
                OperatingCenter = this
            });
            SendPropertyChanged(null);
        }
        private void onTownsRemove(Town obj)
        {
            var operatingCenterTown =
                OperatingCentersTowns.FirstOrDefault(
                    x =>
                    x.TownID == obj.TownID && 
                    x.OperatingCenterID == OperatingCenterID);
            OperatingCentersTowns.Remove(operatingCenterTown);
            SendPropertyChanged(null);
        }

        private void onContractorsAdd(Contractor obj)
        {
            ContractorsOperatingCenters.Add(new ContractorOperatingCenter {
                Contractor = obj,
                OperatingCenter = this
            });
            SendPropertyChanged(null);
        }

        private void onContractorsRemove(Contractor obj)
        {
            var contractorOperatingCenter =
                ContractorsOperatingCenters.FirstOrDefault(
                    x => x.ContractorID == obj.ContractorID &&
                         x.OperatingCenterID == OperatingCenterID);
            ContractorsOperatingCenters.Remove(contractorOperatingCenter);
            SendPropertyChanged(null);
        }

        private void onAllEmployeesAdd(Employee obj)
        {
            OperatingCentersUsers.Add(new OperatingCenterUser {
                Employee = obj,
                OperatingCenter = this
            });
            SendPropertyChanged(null);
        }

        private void onAllEmployeesRemove(Employee obj)
        {
            var operatingCenterUser =
                OperatingCentersUsers.FirstOrDefault(
                    x =>
                    x.EmployeeID == obj.EmployeeID &&
                    x.OperatingCenterID == OperatingCenterID);
            OperatingCentersUsers.Remove(operatingCenterUser);
            SendPropertyChanged(null);
        }

        private void attach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }

        private void detach_Valves(Valve entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        private void attach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = this;
        }

        private void detach_Hydrants(Hydrant entity)
        {
            SendPropertyChanging();
            entity.OperatingCenter = null;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return String.Format("{0} - {1}", OpCntr, OpCntrName);
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
