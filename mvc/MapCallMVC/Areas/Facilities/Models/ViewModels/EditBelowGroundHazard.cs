using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using DataAnnotationsExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class EditBelowGroundHazard : BelowGroundHazardViewModel
    {
        public EditBelowGroundHazard(IContainer container) : base(container) { }
    }
}