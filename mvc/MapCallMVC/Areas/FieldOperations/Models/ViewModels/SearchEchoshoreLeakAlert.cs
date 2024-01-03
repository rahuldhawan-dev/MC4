using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class SearchEchoshoreLeakAlert : SearchSet<EchoshoreLeakAlert>
    {
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        [SearchAlias("EchoshoreSite", "OperatingCenter.Id")]
        public int? OperatingCenter { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(EchoshoreSite))]
        public int? EchoshoreSite { get; set; }
        public int? PersistedCorrelatedNoiseId { get; set; }
        public DateRange DatePCNCreated { get; set; }
        public DateRange FieldInvestigationRecommendedOn { get; set; }
        public string Hydrant1Text { get; set; }
        public string Hydrant2Text { get; set; }

        [View("Has Work Orders"), Search(ChecksExistenceOfChildCollection = true)]

        public bool? WorkOrders { get; set; }
    }
}