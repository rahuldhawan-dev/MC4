using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;
//using MMSINC.Metadata;
//using MMSINC.Validation;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MarkoutDamages")]
    public class MarkoutDamage 
    {
        #region Private Members

        private int _id;
        private int? _WorkOrderId;
        private string _RequestNumber;
        //private int _OperatingCenterId;
        
        private DateTime _DamageOn;
        private string _Excavator;
        private string _UtilitiesDamaged;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id => _id;

        [Column(Storage = "_WorkOrderId", Name = "WorkOrderId", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? WorkOrderId => _WorkOrderId;

        //RequestNumber
        [Column(Storage = "_RequestNumber", Name = "RequestNum", DbType = "varchar(255) NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public string RequestNumber => _RequestNumber;

        //[Column(Storage = "_OperatingCenter", Name = "OperatingCenterId", DbType = "int NOT NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        //public int OperatingCenterId => _OperatingCenterId;

        [Column(Storage = "_DamageOn", Name = "DamageOn", DbType = "SmallDateTime NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public DateTime DamageOn => _DamageOn;

        [Column(Storage = "_Excavator", Name = "Excavator", DbType = "varchar(255) NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public string Excavator => _Excavator;

        [Column(Storage = "_UtilitiesDamaged", Name = "UtilitiesDamaged", DbType = "varchar(255) NULL", UpdateCheck = UpdateCheck.Never,
            CanBeNull = true)]
        public string UtilitiesDamaged => _UtilitiesDamaged;

        #endregion

        #region Private Methods

        protected void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        protected void SendPropertyChanged(String propertyName)
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