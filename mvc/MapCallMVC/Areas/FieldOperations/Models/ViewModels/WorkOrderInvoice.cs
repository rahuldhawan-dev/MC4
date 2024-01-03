using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using Permits.Data.Client.Entities;
using Permits.Data.Client.Repositories;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class WorkOrderInvoiceViewModel : ViewModel<WorkOrderInvoice>
    {
        #region Properties

        [Required]
        public DateTime? InvoiceDate { get; set; }
        public bool IncludeMaterials { get; set; }
        [EntityMap, EntityMustExist(typeof(WorkOrder),ErrorMessage = "The WorkOrderID does not exist. Please remove it or enter a valid WorkOrderID"), DisplayName("Work Order ID")]
        public int? WorkOrder { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public DateTime? CanceledDate { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(ScheduleOfValueType))]
        public int? ScheduleOfValueType { get; set; }
        [Multiline]
        public string InvoiceNotes { get; set; }
        
        #endregion

        #region Constructors

        public WorkOrderInvoiceViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class WorkOrderInvoicePdf : ViewModel<WorkOrderInvoice>
    {
        private IPermitsRepositoryFactory _permitsRepositoryFactory;
        private MMSINC.Data.WebApi.IRepository<Permit> _permitRepository;
        private WorkOrderInvoice _workOrderInvoice;

        internal MMSINC.Data.WebApi.IRepository<Permit> PermitRepository
        {
            get
            {
                if (DisplayWorkOrderInvoice?.WorkOrder == null)
                {
                    return null;
                }

                return
                    _permitRepository ?? (_permitRepository =
                        _permitsRepositoryFactory.GetRepository<Permit>(DisplayWorkOrderInvoice.WorkOrder
                           .OperatingCenter.PermitsUserName));
            }
            set { _permitRepository = value; }
        }

        [DoesNotAutoMap]
        public virtual WorkOrderInvoice DisplayWorkOrderInvoice
        {
            get
            {
               return _workOrderInvoice ?? (_workOrderInvoice =
                     _container.GetInstance<IRepository<WorkOrderInvoice>>().Find(Id));
            }
            set { _workOrderInvoice = value; }
        }
        
        [DoesNotAutoMap]
        public virtual IList<Permit> Permits
        {
            get
            {
                if (DisplayWorkOrderInvoice?.WorkOrder == null)
                    return null;
                var permits = PermitRepository.Search(new NameValueCollection { { "ArbitraryIdentifier", DisplayWorkOrderInvoice.WorkOrder.ToString() } });
                return permits?.ToList();
            }
        }
        
        #region Constructors

        public WorkOrderInvoicePdf(IContainer container, IPermitsRepositoryFactory permitsRepositoryFactory) : base(container)
        {
            _permitsRepositoryFactory = permitsRepositoryFactory;
        }

        #endregion
    }

    public class CreateWorkOrderInvoice : WorkOrderInvoiceViewModel
    {
        #region Constructors

		public CreateWorkOrderInvoice(IContainer container) : base(container) {}

        #endregion

        public override WorkOrderInvoice MapToEntity(WorkOrderInvoice entity)
        {
            base.MapToEntity(entity);

            if (WorkOrder != null)
            {
                var wo = _container.GetInstance<IWorkOrderRepository>().Find(WorkOrder.Value);
                if (wo.WorkOrdersScheduleOfValues != null && wo.WorkOrdersScheduleOfValues.Any())
                {
                    foreach(var x in wo.WorkOrdersScheduleOfValues.Where(y => y.ScheduleOfValue.ScheduleOfValueCategory.ScheduleOfValueType.Id == ScheduleOfValueType))
                    {
                        entity.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue {
                            ScheduleOfValue = x.ScheduleOfValue,
                            MaterialCost = x.ScheduleOfValue.MaterialCost,
                            MiscCost = x.ScheduleOfValue.MiscCost,
                            Total = x.Total,
                            LaborUnitCost = (x.IsOvertime) ? x.ScheduleOfValue.LaborUnitOvertimeCost : x.LaborUnitCost,
                            IsOvertime = x.IsOvertime,
                            WorkOrderInvoice = entity,
                            OtherDescription = x.OtherDescription,
                            IncludeWithInvoice = true,
                            IncludeMarkup = true
                        });
                    }
                }
            }

            return entity;
        }
    }

    public class EditWorkOrderInvoice : WorkOrderInvoiceViewModel
    {
        #region Properties

        [DoesNotAutoMap]
        public bool SendSubmittedNotificationOnSave { get; set; }

        [DoesNotAutoMap]
        public bool SendCanceledNotificationOnSave { get; set; }

        #endregion
        
        #region Constructors

		public EditWorkOrderInvoice(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override WorkOrderInvoice MapToEntity(WorkOrderInvoice entity)
        {
            SendSubmittedNotificationOnSave = (!entity.SubmittedDate.HasValue && SubmittedDate.HasValue) || (entity.SubmittedDate != SubmittedDate);
            SendCanceledNotificationOnSave = (!entity.CanceledDate.HasValue && CanceledDate.HasValue) || (entity.CanceledDate != CanceledDate);
            return base.MapToEntity(entity);
        }

        #endregion
    }

    public class AddWorkOrderInvoiceScheduleOfValue : ViewModel<WorkOrderInvoice>
    {
        #region Properties

        [DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(ScheduleOfValueCategory))]
        public virtual int? ScheduleOfValueCategory { get; set; }

        [DropDown("FieldOperations", "ScheduleOfValue", "ByScheduleOfValueCategoryId", DependsOn = "ScheduleOfValueCategory", PromptText = "Please select a schedule of value category above")]
        [Required,EntityMap(MapDirections.None),EntityMustExist(typeof(ScheduleOfValue))]
        public virtual int? ScheduleOfValue { get; set; }

        [DoesNotAutoMap]
        public virtual decimal? LaborUnitCost { get; set; }
        [DoesNotAutoMap]
        public virtual decimal? MaterialCost { get; set; }
        [DoesNotAutoMap]
        public virtual decimal? MiscCost { get; set; }
        [DisplayName("Total/Hours"), DoesNotAutoMap]
        public virtual int? Total { get; set; }
        [DoesNotAutoMap]
        public virtual bool? IsOvertime { get; set; }
        [StringLength(50), DoesNotAutoMap]
        [RequiredWhen("ScheduleOfValueCategory", ComparisonType.EqualTo, 27)] // What is 27? -Ross 1/26/2018
        public string OtherDescription { get; set; }
        [Required, DoesNotAutoMap]
        public bool? IncludeWithInvoice { get; set; }
        [DoesNotAutoMap]
        public bool IncludeMarkup { get; set; }

        #endregion

        #region Constructors

        public AddWorkOrderInvoiceScheduleOfValue(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override WorkOrderInvoice MapToEntity(WorkOrderInvoice entity)
        {
            entity = base.MapToEntity(entity);
            var scheduleOfValue = _container.GetInstance<IRepository<ScheduleOfValue>>().Find(ScheduleOfValue.Value);
            entity.WorkOrderInvoicesScheduleOfValues.Add(new WorkOrderInvoiceScheduleOfValue {
                WorkOrderInvoice = entity,
                ScheduleOfValue = scheduleOfValue,
                LaborUnitCost = LaborUnitCost ?? ((IsOvertime.HasValue && IsOvertime.Value) ? scheduleOfValue.LaborUnitOvertimeCost : scheduleOfValue.LaborUnitCost),
                MaterialCost = scheduleOfValue.MaterialCost,
                MiscCost = scheduleOfValue.MiscCost,
                Total = Total,
                IsOvertime = IsOvertime.HasValue && IsOvertime.Value,
                OtherDescription = OtherDescription,
                IncludeWithInvoice = IncludeWithInvoice.Value,
                IncludeMarkup = IncludeMarkup
            });
            return entity;
        }

        #endregion
    }

    public class RemoveWorkOrderInvoiceScheduleOfValue : ViewModel<WorkOrderInvoice>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public virtual int ScheduleOfValueId { get; set; }

        #endregion

        #region Constructors

        public RemoveWorkOrderInvoiceScheduleOfValue(IContainer container) : base(container) { }

        #endregion

        public override WorkOrderInvoice MapToEntity(WorkOrderInvoice entity)
        {
            // Do not call base.MapToEntity, there's nothing for it to do.
            entity.WorkOrderInvoicesScheduleOfValues.Remove(
                _container.GetInstance<IRepository<WorkOrderInvoiceScheduleOfValue>>()
                    .Find(ScheduleOfValueId));
            return entity;
        }
    }

    public class EditWorkOrderInvoiceScheduleOfValue : ViewModel<WorkOrderInvoiceScheduleOfValue>
    {
        #region Constructors

        public EditWorkOrderInvoiceScheduleOfValue(IContainer container) : base(container) { }

        #endregion

        #region Properties
        
        [EntityMap, EntityMustExist(typeof(WorkOrderInvoice))]
        public int? WorkOrderInvoice { get; set; }
        [EntityMap, EntityMustExist(typeof(ScheduleOfValue))]
        public int? ScheduleOfValue { get; set; }
        public decimal? LaborUnitCost { get; set; }
        public decimal? MaterialCost { get; set; }
        public decimal? MiscCost { get; set; }
        public decimal? Total { get; set; }
        public bool IsOvertime { get; set; }
        [StringLength(50)]
        public string OtherDescription { get; set; }
        [Required]
        public bool? IncludeWithInvoice { get; set; }
        public bool IncludeMarkup { get; set; }

        #endregion
    }

    public class SearchWorkOrderInvoice : SearchSet<WorkOrderInvoice>
    {
        #region Properties

        public int? Id { get; set; }
        public DateRange InvoiceDate { get; set; }
        [SearchAlias("WorkOrder", "Id")]
        public int? WorkOrderID { get; set; }
                
        [DropDown, EntityMap, EntityMustExist(typeof(WorkOrderInvoiceStatus))]
        public int? WorkOrderInvoiceStatus { get; set; }

        #endregion
    }
}