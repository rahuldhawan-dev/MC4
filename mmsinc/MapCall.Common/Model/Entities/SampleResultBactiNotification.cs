using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    // This class doesn't seem to be used anywhere? -Ross 10/26/2017

    public class SampleResultBactiNotification : IEntity
    {
        public int Id { get; set; }
        public string Sample { get; set; }
        public string SampleSite { get; set; }
        public string SampleDate { get; set; }
        public string BactiSampleType { get; set; }
        public string CollectedBy { get; set; }
        public string AnalysisPerformedBy { get; set; }
        public string Location { get; set; }
        public string Cl2Free { get; set; }
        public string Cl2Total { get; set; }
        public string Nitrite { get; set; }
        public string Nitrate { get; set; }
        public string HPC { get; set; }
        public string Monochloramine { get; set; }
        public string FreeAmmonia { get; set; }
        public string pH { get; set; }
        public string TempCelsius { get; set; }
        public string ValueFe { get; set; }
        public string ValueMn { get; set; }
        public string ValueTurb { get; set; }
        public string ValueOrtho { get; set; }
        public string ValueConductivity { get; set; }
        public string NonSheenColonyCount { get; set; }
        public string NonSheenColonyCountOperator { get; set; }
        public string SheenColonyCount { get; set; }
        public string SheenColonyCountOperator { get; set; }
        public string ColiformConfirm { get; set; }
        public string EColiConfirm { get; set; }
        public string Notes { get; set; }
        public string DTMIncubatorIn { get; set; }
        public string DTMIncubatorOut { get; set; }
        public string DTMDataEntered { get; set; }
    }
}
