using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using MapCall.Common.ClassExtensions.WorkOrderAssetViewModelExtensions;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Utility;
using MapCallMVC.Areas.FieldOperations.Controllers;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using NHibernate.Engine;
using StructureMap;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditMarkoutPlanning : ViewModel<WorkOrder>
    {
        #region Properties
        [Required, DoesNotAutoMap]
        [View(FormatStyle.Date)]
        public virtual DateTime? DateMarkoutNeeded { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(MarkoutType))]
        public virtual int? MarkoutTypeNeeded { get; set; }

        public virtual string RequiredMarkoutNote { get; set; }

        #endregion

        #region Constructors

        public EditMarkoutPlanning(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            if (DateMarkoutNeeded != null)
            {
                entity.MarkoutToBeCalled = WorkOrdersWorkDayEngine.GetCallDate((DateTime)DateMarkoutNeeded, MarkoutRequirementEnum.Routine);
            }
            return base.MapToEntity(entity);
        }
        
        public override void Map(WorkOrder entity)
        {
            base.Map(entity);

            if (entity.MarkoutToBeCalled != null)
            {
                DateMarkoutNeeded = WorkOrdersWorkDayEngine.GetRoutineReadyDate((DateTime)entity.MarkoutToBeCalled);
            }
        }

        #endregion
    }
}