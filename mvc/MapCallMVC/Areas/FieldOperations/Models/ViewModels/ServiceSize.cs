using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    [Serializable]
    public class ServiceSizeViewModel : ViewModel<ServiceSize>
    {
        #region Properties

        [Required]
        public bool Hydrant { get; set; }
        [Required]
        public bool Lateral { get; set; }
        [Required]
        public bool Main { get; set; }
        [Required]
        public bool Meter { get; set; }
        public int? SortOrder { get; set; }
        [Required]
        public bool Service { get; set; }
        public string ServiceSizeDescription { get; set; }
        [Required]
        public decimal Size { get; set; }

        #endregion

        #region Constructors

        public ServiceSizeViewModel(IContainer container) : base(container) { }

        #endregion
    }
}