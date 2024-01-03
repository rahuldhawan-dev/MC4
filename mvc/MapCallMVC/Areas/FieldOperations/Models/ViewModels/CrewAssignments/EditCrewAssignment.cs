using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.CrewAssignments
{
    public class EditCrewAssignment : ViewModel<CrewAssignment>
    {
        #region Fields

        private CrewAssignment _original;

        #endregion

        #region Constructor

        public EditCrewAssignment(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public CrewAssignment Display
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<CrewAssignment>>().Find(Id);
                }
                return _original;
            }
        }

        [DateTimePicker]
        public DateTime? DateStarted { get; set; }

        [DateTimePicker]
        public DateTime? DateEnded { get; set; }

        public float? EmployeesOnJob { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int? WorkOrder { get; set; }

        #endregion
    }
}