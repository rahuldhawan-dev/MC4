using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchNonRevenueWaterEntry : SearchSet<NonRevenueWaterEntry>
    {
        [MultiSelect, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int[] OperatingCenter { get; set; }

        [DropDown]
        public int? Month { get; set; }

        public int? Year { get; set; }
    }
}