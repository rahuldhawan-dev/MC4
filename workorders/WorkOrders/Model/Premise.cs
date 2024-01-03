using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.Premises")]
    public class Premise
    {
        #region Private Members

        private int _id;
        
        private string _installation;
        private int? _operatingCenterID;
        private string _premiseNumber;
        private EntityRef<OperatingCenter> _operatingCenter;

        #endregion

        #region Properties

        [Column(Name = "Id", Storage = nameof(_id), DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int Id => _id;

        [Column(Storage = nameof(_operatingCenterID), DbType = "Int")]
        public int? OperatingCenterID => _operatingCenterID;

        [Column(Storage = nameof(_installation), DbType = "VarChar NOT NULL")]
        public string Installation => _installation;

        [Column(
            Name = "PremiseNumber", 
            Storage = "_premiseNumber",
            DbType = "VarChar(10)")]
        public string PremiseNumber
        {
            get { return _premiseNumber; }
            set { _premiseNumber = value; }
        }

        [Association(
            Name = "OperatingCenter_Premise",
            Storage = nameof(_operatingCenter),
            ThisKey = nameof(OperatingCenterID),
            IsForeignKey = true)]
        public OperatingCenter OperatingCenter =>
            _operatingCenter.Entity;

        #endregion

        #region Constructors

        public Premise()
        {
            _operatingCenter = default;
        }

        #endregion
    }
}
