using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Historian.Data.Client.Entities;
using MapCall.Common.Model.Entities;
using MMSINC.Metadata;
using MMSINC.Results;
using MMSINC.Validation;

namespace MapCallMVC.Areas.Facilities.Models
{
    public class SearchScadaReading
    {
        [Required, DropDown, EntityMustExist(typeof(ScadaTagName))]
        public int? TagName { get; set; }

        [Display(Name = "Use Raw?")]
        public bool UseRaw { get; set; }

        [DateTimePicker]
        public DateTime? StartDate { get; set; }
        [DateTimePicker]
        public DateTime? EndDate { get; set; }

        // to be set by the controler action from the repos:
        public IEnumerable<RawData> Results { get; set; }
        public ScadaTagName TagNameObj { get; set; }

        public object ToRouteValuesForExcel()
        {
            return
                new {
                    ext = ResponseFormatter.KnownExtensions.EXCEL_2003,
                    area = "Facilities",
                    TagName,
                    UseRaw,
                    StartDate,
                    EndDate
                };
        }
    }
}