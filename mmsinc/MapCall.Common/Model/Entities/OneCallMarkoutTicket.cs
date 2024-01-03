using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using StructureMap;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class OneCallMarkoutTicket : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Fields

        [NonSerialized] private IRepository<OneCallMarkoutTicket> _ticketRepository;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual OneCallMarkoutMessageType MessageType { get; set; }
        public virtual Town Town { get; set; }
        public virtual Street Street { get; set; }
        public virtual Street NearestCrossStreet { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }

        [Required]
        public virtual int RequestNumber { get; set; }

        public virtual int? RelatedRequestNumber { get; set; }
        public virtual OneCallMarkoutTicket RelatedRequest { get; set; }

        [Required]
        public virtual DateTime DateTransmitted { get; set; }

        [Required]
        public virtual DateTime DateReceived { get; set; }

        [StringLength(50), DisplayName("County")]
        public virtual string CountyText { get; set; }

        [StringLength(50), DisplayName("Town")]
        public virtual string TownText { get; set; }

        [StringLength(50), DisplayName("Street")]
        public virtual string StreetText { get; set; }

        [StringLength(50), DisplayName("Nearest Cross Street")]
        public virtual string NearestCrossStreetText { get; set; }

        public virtual string TypeOfWork { get; set; }
        public virtual string WorkingFor { get; set; }
        public virtual string Excavator { get; set; }

        [DisplayName("CDC Code")]
        public virtual string CDCCode { get; set; }

        [Required]
        public virtual string FullText { get; set; }

        public virtual bool HasResponse { get; set; }

        public virtual IEnumerable<OneCallMarkoutTicket> FollowUpTickets
        {
            get
            {
                return
                    _ticketRepository
                       .Where(t => t.CDCCode == CDCCode && t.RelatedRequestNumber == RequestNumber);
            }
        }

        public virtual IList<OneCallMarkoutResponse> Responses { get; set; }

        #region Logical Properties

        #region Documents

        public virtual IList<OneCallMarkoutTicketDocument> OneCallMarkoutTicketDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return OneCallMarkoutTicketDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return OneCallMarkoutTicketDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<OneCallMarkoutTicketNote> OneCallMarkoutTicketNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return OneCallMarkoutTicketNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return OneCallMarkoutTicketNotes.Map(n => n.Note); }
        }

        #endregion

        public virtual string TableName => nameof(OneCallMarkoutTicket) + "s";

        public virtual bool ExcavatorIsAmericanWater { get; set; }

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IRepository<OneCallMarkoutTicket> TicketRepository
        {
            set => _ticketRepository = value;
        }

        #endregion

        #endregion

        public OneCallMarkoutTicket()
        {
            Responses = new List<OneCallMarkoutResponse>();
        }

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return RequestNumber.ToString();
        }

        #endregion
    }
}
