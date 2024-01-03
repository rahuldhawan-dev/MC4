using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class SearchPipeDataLookupValue : SearchSet<PipeDataLookupValue>
    {
         [DropDown, EntityMap, EntityMustExist(typeof(PipeDataLookupType))]
         public int? PipeDataLookupType { get; set; }
    }
}