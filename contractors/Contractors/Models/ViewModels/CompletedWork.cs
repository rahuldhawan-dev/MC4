using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Contractors.Data.Models.Repositories;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    [StringLengthNotRequired]
    public class SearchCompletedWork : SearchSet<MeterChangeOut>
    {
        private int? _status;

        [DropDown, EntityMap, EntityMustExist(typeof(ContractorMeterCrew)), Required]
        public int? CalledInByContractorMeterCrew { get; set; }
        [Required]
        public DateRange DateStatusChanged { get; set; }

        [EntityMap, EntityMustExist(typeof(MeterChangeOutStatus))]
        public int? MeterChangeOutStatus =>
            _status ?? (_status = MapCall
                                 .Common.Model.Entities
                                 .MeterChangeOutStatus.Indices
                                 .CHANGED);
    }
}