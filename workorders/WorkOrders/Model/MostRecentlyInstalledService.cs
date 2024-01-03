using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MostRecentlyInstalledServicesView")]
    public class MostRecentlyInstalledService
    {
        #region Private Members

        private EntityRef<Premise> _premise;
        private EntityRef<ServiceMaterial> _serviceMaterial, _customerSideMaterial;
        private EntityRef<ServiceSize> _serviceSize, _customerSideSize;

        private int _serviceID, _premiseID;

        private int? _serviceMaterialID,
                     _customerSideMaterialID,
                     _serviceSizeID,
                     _customerSideSizeID;
        
        #endregion

        #region Properties

        [Column(Storage = nameof(_serviceID), DbType = "Int NOT NULL", IsPrimaryKey = true)]
        public int ServiceID => _serviceID;

        [Column(Storage = nameof(_premiseID), DbType = "Int NOT NULL")]
        public int PremiseID => _premiseID;

        [Column(Storage = nameof(_serviceMaterialID), DbType = "Int")]
        public int? ServiceMaterialID => _serviceMaterialID; 

        [Column(Storage = nameof(_serviceSizeID), DbType = "Int")]
        public int? ServiceSizeID => _serviceSizeID; 

        [Column(Storage = nameof(_customerSideMaterialID), DbType = "Int")]
        public int? CustomerSideMaterialID => _customerSideMaterialID; 

        [Column(Storage = nameof(_customerSideSizeID), DbType = "Int")]
        public int? CustomerSideSizeID => _customerSideSizeID; 

        [Association(
            Name = "Premise_MostRecentlyInstalledService",
            Storage = nameof(_premise),
            ThisKey = nameof(PremiseID),
            OtherKey = "Id",
            IsForeignKey = true)]
        public Premise Premise => _premise.Entity;

        [Association(
            Name = "ServiceMaterial_MostRecentlyInstalledService",
            Storage = nameof(_serviceMaterial),
            ThisKey = nameof(ServiceMaterialID),
            IsForeignKey = true)]
        public ServiceMaterial ServiceMaterial => _serviceMaterial.Entity;

        [Association(
            Name = "CustomerSideMaterial_MostRecentlyInstalledService",
            Storage = nameof(_customerSideMaterial),
            ThisKey = nameof(CustomerSideMaterialID),
            IsForeignKey = true)]
        public ServiceMaterial CustomerSideMaterial => _customerSideMaterial.Entity;

        [Association(
            Name = "ServiceSize_MostRecentlyInstalledService",
            Storage = nameof(_serviceSize),
            ThisKey = nameof(ServiceSizeID),
            IsForeignKey = true)]
        public ServiceSize ServiceSize => _serviceSize.Entity;

        [Association(
            Name = "CustomerSideSize_MostRecentlyInstalledService",
            Storage = nameof(_customerSideSize),
            ThisKey = nameof(CustomerSideSizeID),
            IsForeignKey = true)]
        public ServiceSize CustomerSideSize => _customerSideSize.Entity;

        #endregion

        #region Constructors

        public MostRecentlyInstalledService()
        {
            _premise = default;
            _serviceMaterial = default;
            _customerSideMaterial = default;
            _serviceSize = default;
            _customerSideSize = default;
        }

        #endregion
    }
}
