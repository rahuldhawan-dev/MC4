using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.ViewModels
{
    public class SearchSapNotification
    {
        #region Properties

        #region Get Notification

        [Required]
        public virtual string PlanningPlant { get; set; }

        [Required]
        public virtual string DateCreatedFrom { get; set; }

        [Required]
        public virtual string DateCreatedTo { get; set; }

        public virtual string NotificationType { get; set; }
        public virtual string Priority { get; set; }
        public virtual string CodeGroup => (string.IsNullOrWhiteSpace(Code) ? null : "N-D-PUR1");
        public virtual string Code { get; set; }

        #endregion

        #region Cancel Update Notification

        public virtual string SAPNotificationNo { get; set; }
        public virtual string Complete { get; set; }
        public virtual string Cancel { get; set; }
        public virtual string Remarks { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        #endregion
    }

    public class SearchSapTechnicalMaster
    {
        #region Properties

        //search by
        //1. premise stand alone
        //2. equipment stand alone
        //3. premise number + installation type
        //4. equipment _ insytallation type
        public virtual string PremiseNumber { get; set; }
        public virtual string InstallationType { get; set; }
        public virtual string Equipment { get; set; }

        #endregion

        #region Exposed Methods

        #endregion
    }

    public class SearchSAPTechnicalMasterAccount
    {
        public string PremiseNumber { get; set; }
        public string InstallationType { get; set; }
        public string Equipment { get; set; }
    }

    public class SearchSapCustomerOrder
    {
        #region Properties

        //comments added to check branch 
        public virtual string FSR_ID { get; set; }
        public virtual string WorkOrder { get; set; }

        #endregion
    }
}
