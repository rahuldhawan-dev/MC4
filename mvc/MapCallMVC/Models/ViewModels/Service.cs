using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MapCall.Common.Model.Entities;

namespace MapCallMVC.Models.ViewModels
{
    public class SearchServicesRenewed : SearchSet<Service>
    {
        #region Properties

        [DropDown]
        public int? OperatingCenter { get; set; }
        
        public RequiredDateRange DateInstalled { get; set; }
        
        //All, Yes, No options for ddl (switch this to a proper boolean?)
        [DisplayName("Developer Services Driven")]
        public bool? DeveloperServicesDriven { get; set; }

        [DisplayName("WBS #")]
        public virtual string TaskNumber1 { get; set; }

        /*
         *- Operating Center 
         *- DateInstallation
         *- Developer Services Driven
         *- Town
         *- Category of Service
         *- Size of Service
         *- Total Footage
         *- number of services
         *- Original Installation Year
         *- Previous Service Material
         *- Service #
         *- Task Number
        */

        #endregion
    }

    public class SearchServicesInstalled : SearchServicesRenewed
    {
        
    }
}