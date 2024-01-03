using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using StructureMap;

namespace MapCallImporter.ViewModels
{
    public class MyCreateHydrantInspection : CreateHydrantInspection
    {
        #region Properties

        public string SAPNotificationNumber { get; set; }

        #endregion

        #region Constructors

        public MyCreateHydrantInspection(IContainer container) : base(container) { }

        #endregion
    }
}