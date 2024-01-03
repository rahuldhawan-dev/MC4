using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Events.Models.ViewModels
{
    public class CreateEventDocument : EventDocumentsViewModel
    {
        #region Constructor

        public CreateEventDocument(IContainer container) : base(container) { }

        #endregion
    }
}