using System;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using System.ComponentModel.DataAnnotations;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateTrainingSession : ViewModel<TrainingSession>
    {
        #region Properties

        [DateTimePicker]
        [Required]
        public virtual DateTime? StartDateTime { get; set; }
        [DateTimePicker]
        [Required]
        public virtual DateTime? EndDateTime { get; set; }
        [DropDown]
        [Required]
        [EntityMap, EntityMustExist(typeof(TrainingRecord))]
        public virtual int? TrainingRecord { get; set; }

        #endregion
        
        #region Constructors

        public CreateTrainingSession(IContainer container) : base(container) {}

        #endregion
    }

    public class EditTrainingSession : ViewModel<TrainingSession>
    {
        #region Properties

        [AutoMap(MapDirections.None)]
        public virtual TrainingRecord TrainingRecord { get; set; }
        [AutoMap(MapDirections.None)]
        public virtual int TrainingRecordId { get; set; }

        [DateTimePicker]
        [Required]
        public virtual DateTime? StartDateTime { get; set; }
        [DateTimePicker]
        [Required]
        public virtual DateTime? EndDateTime { get; set; }

        #endregion

        #region Constructors

        public EditTrainingSession(IContainer container) : base(container) {}

        #endregion

        #region Public Methods

        public override void Map(TrainingSession entity)
        {
            base.Map(entity);
            TrainingRecord = entity.TrainingRecord;
            TrainingRecordId = entity.TrainingRecord.Id;
        }

        #endregion
    }
}