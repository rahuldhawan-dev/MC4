using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using System;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.GeneralWorkOrder
{
    public class EditTrafficControl : ViewModel<WorkOrder>
    {
        #region Constructor

        public EditTrafficControl(IContainer container) : base(container) { }

        #endregion

        #region Fields

        private WorkOrder _original;

        #endregion

        #region Properties

        [DoesNotAutoMap]
        public WorkOrder WorkOrder
        {
            get
            {
                if (_original == null)
                {
                    _original = Original ?? _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
                }
                return _original;
            }
        }

        public int? NumberOfOfficersRequired { get; set; }

        [Multiline, DoesNotAutoMap]
        public string AppendNotes { get; set; }

        #endregion

        #region Exposed Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);

            if (!string.IsNullOrWhiteSpace(AppendNotes))
            {
                if (!string.IsNullOrWhiteSpace(entity.Notes))
                {
                    entity.Notes += Environment.NewLine;
                }

                entity.Notes += $"{_container.GetInstance<IAuthenticationService<User>>().CurrentUser.FullName} " +
                                _container.GetInstance<IDateTimeProvider>().GetCurrentDate().ToString(CommonStringFormats.DATETIME_WITH_SECONDS_WITH_EST_TIMEZONE_FOR_WEBFORMS) +
                                $" {AppendNotes}";
            }

            return entity;
        }

        #endregion
    }
}
