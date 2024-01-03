using System;
using System.Collections.Generic;

namespace Permits.Data.Client.Entities
{
    public class Permit : IPermit<FieldPermitValue>
    {
        public virtual int Id { get; set; }
        public virtual int FormId { get; set; }
        public virtual IList<FieldPermitValue> FieldPermitValues { get; set; }
        public virtual decimal ProcessingFee { get; set; }
        public virtual decimal PermitFee { get; set; }
        public virtual decimal BondFee { get; set; }
        public virtual decimal TotalCharged { get; set; }

        public virtual string ArbitraryIdentifier { get; set; }
        public virtual bool ShowArbitraryIdentifier { get; set; }
        public virtual string ArbitraryIdentifierLabelText { get; set; }

        public virtual string VerbalAgreementWith { get; set; }
        public virtual string VerbalAgreementPhone { get; set; }
        public virtual string VerbalPermitNumber { get; set; }
        public virtual string SpecialNotes { get; set; }
        public bool HasMetDrawingRequirement { get; set; }
        public bool IsPaidFor { get; set; }
        public bool IsCanceled { get; set; }
        public int? TrafficControlDiagramId { get; set; }
        public int? RestorationDiagramId { get; set; }

        public virtual string PermitFor { get; set; }
        public string AccountingCode { get; set; }
        public bool FeeWaived { get; set; }
        public string FeeWaivedNotes { get; set; }
        public virtual DateTime CreatedAt { get; set; }
    }

    public interface IPermit<TFieldPermitValueType> where TFieldPermitValueType : IFieldPermitValue
    {
        int Id { get; set; }
        int FormId { get; set; }
        IList<TFieldPermitValueType> FieldPermitValues { get; set; }

        decimal ProcessingFee { get; }
        decimal PermitFee { get; }
        decimal BondFee { get; }
        decimal TotalCharged { get; }
        string ArbitraryIdentifier { get; set; }
        bool ShowArbitraryIdentifier { get; set; }
        string ArbitraryIdentifierLabelText { get; set; }

        string VerbalAgreementWith { get; set; }
        string VerbalAgreementPhone { get; set; }
        string VerbalPermitNumber { get; set; }
        string SpecialNotes { get; set; }
        bool HasMetDrawingRequirement { get; set; }
        bool IsPaidFor { get; set; }
        bool IsCanceled { get; set; }
        int? TrafficControlDiagramId { get; set; }
        int? RestorationDiagramId { get; set; }

        string PermitFor { get; set; }

        string AccountingCode { get; set; }

        bool FeeWaived { get; set; }
        string FeeWaivedNotes { get; set; }
    }
}
