using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class ReverseMeasurementPointProductionWorkOrder : ViewModel<ProductionWorkOrder>
    {
        #region Properties

        [DoesNotAutoMap]
        public string MeasurementDocId { get; set; }

        #endregion

        #region Constructors

        public ReverseMeasurementPointProductionWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        

        #endregion
    }
}