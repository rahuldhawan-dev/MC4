using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using ServiceCategoryEntity = MapCall.Common.Model.Entities.ServiceCategory;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services.Reports
{
    //if you update me, don't forget to update my test class,
    public class SearchMonthlyServicesInstalledByCategory : SearchSet<MonthlyServicesInstalledByCategoryViewModel>, ISearchMonthlyServicesInstalledByCategory
    {
        #region Properties

        [MultiSelect]
        public virtual int[] OperatingCenter { get; set; }

        [Required, DropDown]
        public virtual int Year { get; set; }

        // StoredProcedure limited to these: where Description in 
        // ('Fire Service Installation', 'Irrigation New', 'Sewer Service New', 
        // 'Water Service New Commercial', 'Water Service New Domestic')
        public virtual int[] ServiceCategory
        {
            get
            {
                return new[] {
                    ServiceCategoryEntity.Indices.FIRE_SERVICE_INSTALLATION,
                    ServiceCategoryEntity.Indices.IRRIGATION_NEW,
                    ServiceCategoryEntity.Indices.SEWER_SERVICE_NEW,
                    ServiceCategoryEntity.Indices.WATER_SERVICE_NEW_COMMERCIAL,
                    ServiceCategoryEntity.Indices.WATER_SERVICE_NEW_DOMESTIC
                };
            }
        }

        #endregion
    }
}
