using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.RestorationTypes")]
    public class RestorationType : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Enumerations

        public enum MeasurementTypes
        {
            SquareFt,
            LinearFt
        }

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _restorationTypeID;

        private string _description;

        private MeasurementTypes? _measurementType;

        private readonly EntitySet<Restoration> _restorations;

        private EntitySet<RestorationMethodRestorationType> _restorationMethodsRestorationTypes;

        private EntitySet<RestorationTypeCost> _restorationTypeCosts;

        #endregion

        #region Properties

        [Column(Storage = "_restorationTypeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int RestorationTypeID
        {
            get { return _restorationTypeID; }
            set
            {
                if (_restorationTypeID != value)
                {
                    SendPropertyChanging();
                    _restorationTypeID = value;
                    SendPropertyChanged("RestorationTypeID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(30) NO NULL", UpdateCheck = UpdateCheck.Never)]
        public string Description
        {
            get { return _description; }
            set
            {
                if ((_description != value))
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        public MeasurementTypes MeasurementType
        {
            get
            {
                if (_measurementType == null)
                    _measurementType = DeterimeMeasurementType();
                return _measurementType.Value;
            }
        }

        public string MeasurementTypeString
        {
            get { return MeasurementTypesExtensions.ToString(MeasurementType); }
        }

        [Association(Name = "RestorationType_Restoration", Storage = "_restorations", OtherKey = "RestorationTypeID")]
        public EntitySet<Restoration> Restorations
        {
            get { return _restorations; }
            set { _restorations.Assign(value); }
        }

        [Association(Name = "RestorationType_RestorationMethodRestorationType", Storage = "_restorationMethodsRestorationTypes", OtherKey = "RestorationTypeID")]
        public EntitySet<RestorationMethodRestorationType> RestorationMethodsRestorationTypes
        {
            get { return _restorationMethodsRestorationTypes; }
            set { _restorationMethodsRestorationTypes.Assign(value); }
        }

        [Association(Name = "RestorationType_RestorationTypeCost", Storage = "_restorationTypeCosts", OtherKey = "RestorationTypeID")]
        public EntitySet<RestorationTypeCost> RestorationTypeCosts
        {
            get { return _restorationTypeCosts; }
            set { _restorationTypeCosts.Assign(value); }
        }

        #endregion

        #region Constructors

        public RestorationType()
        {
            _restorations = new EntitySet<Restoration>(attach_Restorations, detach_Restorations);
            _restorationMethodsRestorationTypes =
                new EntitySet<RestorationMethodRestorationType>(
                    attach_RestorationMethodsRestorationTypes,
                    detach_RestorationMethodsRestorationTypes);
            _restorationTypeCosts = new EntitySet<RestorationTypeCost>(attach_RestorationTypeCosts, detach_RestorationTypeCosts);
        }

        #endregion

        #region Private Methods

        protected virtual void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        private void attach_Restorations(Restoration entity)
        {
            SendPropertyChanging();
            entity.RestorationType = this;
        }

        private void detach_Restorations(Restoration entity)
        {
            SendPropertyChanging();
            entity.RestorationType = null;
        }

        private void attach_RestorationMethodsRestorationTypes(RestorationMethodRestorationType entity)
        {
            SendPropertyChanging();
            entity.RestorationType = this;
        }

        private void detach_RestorationMethodsRestorationTypes(RestorationMethodRestorationType entity)
        {
            SendPropertyChanging();
            entity.RestorationType = null;
        }

        private void attach_RestorationTypeCosts(RestorationTypeCost entity)
        {
            SendPropertyChanging();
            entity.RestorationType = this;
        }

        private void detach_RestorationTypeCosts(RestorationTypeCost entity)
        {
            SendPropertyChanging();
            entity.RestorationType = null;
        }

        private MeasurementTypes DeterimeMeasurementType()
        {
            return (Description.ToUpper().Contains("CURB"))
                       ? MeasurementTypes.LinearFt
                       : MeasurementTypes.SquareFt;
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }

    public static class MeasurementTypesExtensions
    {
        #region Constants

        public struct MeasurementTypeStrings
        {
            public const string SQUARE_FEET = "Square Ft.",
                                LINEAR_FT = "Linear Ft.";
        }

        #endregion

        #region Exposed Static Methods

        public static String ToString(RestorationType.MeasurementTypes type)
        {
            return (type == RestorationType.MeasurementTypes.LinearFt)
                       ? MeasurementTypeStrings.LINEAR_FT
                       : MeasurementTypeStrings.SQUARE_FEET;
        }

        #endregion
    }
}
