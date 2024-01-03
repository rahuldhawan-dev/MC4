using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class TrafficControlTicketCheckViewModel : ViewModel<TrafficControlTicketCheck>
    {
        #region Properties

        [Required]
        public virtual decimal? Amount { get; set; }
        [Required]
        public virtual int? CheckNumber { get; set; }
        public virtual string Memo { get; set; }

        #endregion

        #region Constructors

        public TrafficControlTicketCheckViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateTrafficControlTicketCheck : TrafficControlTicketCheckViewModel
    {
        #region Properties

        [Required]
        [EntityMap, EntityMustExist(typeof(TrafficControlTicket))]
        public virtual int? TrafficControlTicket { get; set; }

        [AutoMap(MapDirections.None)]
        public TrafficControlTicket TrafficControlTicketDisplay { get; set; }

        #endregion

        #region Constructors

		public CreateTrafficControlTicketCheck(IContainer container) : base(container) {}

        #endregion
	}

    public class EditTrafficControlTicketCheck : TrafficControlTicketCheckViewModel
    {
        #region Properties

        public virtual bool? Reconciled { get; set; }
        
        #endregion

        #region Constructors

		public EditTrafficControlTicketCheck(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchTrafficControlTicketCheck : SearchSet<TrafficControlTicketCheck>
    {
        #region Properties

        public int? CheckNumber { get; set; }
        public string Memo { get; set; }

        #endregion
	}
}