using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class EditRecurringProjectPipeDataLookupValue : ViewModel<RecurringProjectPipeDataLookupValue>
    {
        #region Constructors

        public EditRecurringProjectPipeDataLookupValue(IContainer container) : base(container) { }

        #endregion

        #region Properties

        #region Display Only

        private RecurringProject _displayRecurringProject;

        [DoesNotAutoMap("Display only")]
        public RecurringProject DisplayRecurringProject
        {
            get
            {
                if (_displayRecurringProject == null)
                    _displayRecurringProject = _container.GetInstance<IRecurringProjectRepository>().Find(RecurringProject);
                return _displayRecurringProject;
            }
        }

        private PipeDataLookupValue _displayPipeDataLookupValue;

        [DoesNotAutoMap("Display only")]
        public PipeDataLookupValue DisplayPipeDataLookupValue
        {
            get
            {
                if (_displayPipeDataLookupValue == null)
                    _displayPipeDataLookupValue = _container.GetInstance<IRepository<PipeDataLookupValue>>().Find(PipeDataLookupValue.Value);
                return _displayPipeDataLookupValue;
            }
        }

        #endregion
        
        [Required, EntityMustExist(typeof(RecurringProject)), EntityMap]
        public virtual int RecurringProject { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(PipeDataLookupValue))]
        public virtual int? PipeDataLookupValue { get; set; }

        #endregion


    }
}