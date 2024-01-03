using System;

namespace MapCall.SAP.Model.Entities
{
    [Serializable]
    public class SAPProductionWorkOrderMeasuringPoints
    {
        public virtual string MeasuringPoint1 { get; set; }
        public virtual string MeasuringReading1 { get; set; }
        public virtual string Unit1 { get; set; }
        public virtual string NoReadingTakenFlag { get; set; }
        public virtual string CompleteNotification { get; set; }
        public virtual string MeasuringDocument { get; set; }
        public virtual bool CancellationFlag { get; set; }
        public virtual string MeasuringPointStatus { get; set; }
    }
}
