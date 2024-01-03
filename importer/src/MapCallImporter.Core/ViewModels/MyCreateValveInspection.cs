using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateValveInspection : CreateValveInspection
    {
        #region Properties

        public string SAPNotificationNumber { get; set; }

        #endregion

        #region Constructors

        public MyCreateValveInspection(IContainer container) : base(container) {  }

        #endregion
    }
}