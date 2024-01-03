using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HumanResources.Models.ViewModels
{
    public class EditEmployeeAccountabilityAction : EmployeeAccountabilityActionViewModel
    {
        public EditEmployeeAccountabilityAction(IContainer container) : base(container) { }
    }
}