using System;
using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels.Services;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateService : CreateService
    {
        private static readonly ConcurrentDictionary<(string Installation, int? OperatingCenter), bool> LINKED_SAMPLE_SITE_CACHE = new ConcurrentDictionary<(string Installation, int? OperatingCenter), bool>();

        public DateTime? CustomerSideSLWarrantyExpiration { get; set; }
        public DateTime? DateCreditProcessed { get; set; }
        public int? ImageActionID { get; set; }
        public string SAPErrorCode { get; set; }
        public string StreetAddress { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(PermitType))]
        public int? PermitType { get; set; }

        public DateTime? PermitExpirationDate { get; set; }

        [StringLength(Service.StringLengths.PERMIT_NUMBER)]
        public string PermitNumber { get; set; }

        public DateTime? PermitReceivedDate { get; set; }

        [RequiredWhen("PermitReceivedDate", ComparisonType.NotEqualTo, null)]
        public DateTime? PermitSentDate { get; set; }

        public override bool HasLinkedSampleSiteByInstallation
        {
            get
            {
                var key = (Installation, OperatingCenter);
                if (LINKED_SAMPLE_SITE_CACHE.ContainsKey(key))
                {
                    return LINKED_SAMPLE_SITE_CACHE[key];
                }

                return LINKED_SAMPLE_SITE_CACHE[key] = base.HasLinkedSampleSiteByInstallation;
            }
        }

        public MyCreateService(IContainer container) : base(container) { }
    }
}