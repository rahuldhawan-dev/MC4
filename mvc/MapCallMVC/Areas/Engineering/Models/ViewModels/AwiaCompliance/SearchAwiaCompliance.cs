using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using AwiaComplianceEntity = MapCall.Common.Model.Entities.AwiaCompliance;

namespace MapCallMVC.Areas.Engineering.Models.ViewModels.AwiaCompliance
{
    public class SearchAwiaCompliance : SearchSet<AwiaComplianceEntity>
    {
        #region Properties

        public int? Id { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(State))]
        public int? State { get; set; }

        [DropDown("", "OperatingCenter", "ActiveByStateIdOrAll", DependsOn = "State", PromptText = "Select a state above"),
         EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [EntityMap, EntityMustExist(typeof(PublicWaterSupply))]
        [DropDown("", "PublicWaterSupply", "ByOperatingCenterId", DependsOn = "OperatingCenter", PromptText = "Please select an one operating center above.")]
        [SearchAlias("PublicWaterSupplies", "Id"), View(AwiaComplianceEntity.Display.PUBLIC_WATER_SUPPLY)]
        public int? PublicWaterSupplies { get; set; }

        [MultiSelect, EntityMap, EntityMustExist(typeof(AwiaComplianceCertificationType))]
        public int[] CertificationType { get; set; }

        [DropDown("", "User", "GetActiveUsersByStateId", DependsOn = "State", PromptText = "Select a state above"), 
         EntityMap, EntityMustExist(typeof(User))]
        public int? CertifiedBy { get; set; }

        [DropDown("", "User", "GetActiveUsersByStateId", DependsOn = "State", PromptText = "Select a state above"), 
         EntityMap, EntityMustExist(typeof(User))]
        public int? CreatedBy { get; set; }

        public DateRange DateSubmitted { get; set; }
        
        [View(AwiaComplianceEntity.Display.DATE_ACCEPTED)]
        public DateRange DateAccepted { get; set; }
        
        public DateRange RecertificationDue { get; set; }

        #endregion
    }
}