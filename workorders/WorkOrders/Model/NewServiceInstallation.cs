using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name="dbo.ServiceInstallations")]
    public class NewServiceInstallation
    {
        #region Private Members

        private int _id;
        private int? _workOrderId;
        private string _meterManufacturerSerialNumber,
            _meterLocationInformation;

        #endregion

        #region Properties

        [Column(Storage = "_id", Name = "Id", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int Id
        {
            get { return _id; }
        }

        [Column(Storage = "_workOrderId", Name = "WorkOrderId", DbType = "Int NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public int? WorkOrderId
        {
            get { return _workOrderId; }
        }

        [Column(Storage = "_meterManufacturerSerialNumber", Name = "MeterManufacturerSerialNumber", DbType = "NVARCHAR(18) NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public string MeterManufacturerSerialNumber
        {
            get { return _meterManufacturerSerialNumber; }
        }

        [Column(Storage = "_meterLocationInformation", Name = "MeterLocationInformation", DbType = "NVARCHAR(18) NULL", UpdateCheck = UpdateCheck.Never, CanBeNull = true)]
        public string MeterLocationInformation
        {
            get { return _meterLocationInformation; }
        }
        
        #endregion
    }
}