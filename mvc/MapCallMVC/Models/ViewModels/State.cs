using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EditState : ViewModel<State>
    {
        #region Properties

        [Required, StringLength(State.MaxLengths.ABBREVIATION)]
        public string Abbreviation { get; set; }
        [Required, StringLength(State.MaxLengths.NAME)]
        public string Name { get; set; }
        [StringLength(State.MaxLengths.SCADA_TBL)]
        public string ScadaTable { get; set; }

        #endregion
        
        #region Constructors

        public EditState(IContainer container) : base(container) { }

        #endregion
    }
}