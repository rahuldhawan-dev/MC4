using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MapCall.Common.Model.Entities;
using MMSINC.ClassExtensions.NameValueCollectionExtensions;
using MMSINC.Metadata;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class AsBuiltImage
    {
        #region Properties

        // AsBuiltImageID
        public int Id { get; set; }
        public string PdfLink { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public County County { get; set; }
        public string Comments { get; set; }
        public DateTime? DateInstalled { get; set; }
        public DateTime? InServiceDate { get; set; }
        public string FullStreet { get; set; }
        public string CrossStreet { get; set; }
        public string WorkOrderNumber { get; set; }
        public Town Town { get; set; }
        public OperatingCenter OperatingCenter { get; set; }

        #endregion

        #region Exposed Methods

        public static AsBuiltImage FromDbRecord(MapCall.Common.Model.Entities.AsBuiltImage asBuiltImage)
        {
            return new AsBuiltImage
            {
                Id = asBuiltImage.Id,
                PdfLink =
                    $"{ConfigurationManager.AppSettings.EnsureValue("BaseUrl")}FieldOperations/AsBuiltImage/Show/{asBuiltImage.Id}.pdf",
                Latitude = asBuiltImage.Coordinate?.Latitude ?? 0,
                Longitude = asBuiltImage.Coordinate?.Longitude ?? 0,
                OperatingCenter = OperatingCenter.FromDbRecord(asBuiltImage.OperatingCenter),
                County = County.FromDbRecord(asBuiltImage.Town?.County),
                Town = Town.FromDbRecord(asBuiltImage.Town),
                FullStreet = asBuiltImage.Street,
                CrossStreet = asBuiltImage.CrossStreet,
                WorkOrderNumber = asBuiltImage.TaskNumber,
                DateInstalled = asBuiltImage.DateInstalled.FromDbRecord(),
                InServiceDate = asBuiltImage.PhysicalInService.FromDbRecord(),
                Comments = asBuiltImage.Comments
            };
        }

        #endregion
    }
}
