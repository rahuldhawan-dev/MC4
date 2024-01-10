using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.WorkOrderStockToIssue
{
    public class ApproveWorkOrderStockToIssue : ViewModel<WorkOrder>
    {
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

        [Required]
        public DateTime? MaterialPostingDate { get; set; }

        [MaxLength(WorkOrder.StringLengths.MATERIALS_DOC_ID)]
        public string MaterialsDocID { get; set; }

        #endregion

        #region Constructor

        public ApproveWorkOrderStockToIssue(IContainer container) : base(container) { }

        #endregion 

        #region Public Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            base.MapToEntity(entity);

            entity.MaterialsApprovedOn = _container.GetInstance<IDateTimeProvider>().GetCurrentDate();
            entity.MaterialsApprovedBy = _container.GetInstance<IAuthenticationService<User>>().CurrentUser;

            return entity;
        }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var entity = _container.GetInstance<IRepository<WorkOrder>>().Find(Id);
            if (entity.MaterialsApprovedOn.HasValue)
            {
                yield return new ValidationResult("Materials have already been approved for this work order.");
            }
        }

        public override void Map(WorkOrder entity)
        {
            base.Map(entity);
            MaterialPostingDate = DateTime.Now;
        }

        #endregion
    }
}