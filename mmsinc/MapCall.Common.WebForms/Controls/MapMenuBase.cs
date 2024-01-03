using System;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using MapCall.Common.Model.Repositories;
using MMSINC.Common;
using MMSINC.Utilities;
using StructureMap;

namespace MapCall.Common.Controls
{
    public abstract class MapMenuBase : MvpUserControl
    {
        #region Fields

        protected DateTime? _gisDataDate;

        #endregion

        #region Control Declarations

        protected PlaceHolder gisLayersOuterPlaceHolder,
                              layersOuterPlaceHolder,
                              layersInnerPlaceHolder,
                              legendOuterPlaceHolder,
                              legendInnerPlaceHolder,
                              optionsOuterPlaceHolder,
                              optionsInnerPlaceHolder,
                              notesPlaceHolder,
                              bottomPlaceHolder;

        #endregion

        #region Properties

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate LayersTemplate { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate LegendTemplate { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate OptionsTemplate { get; set; }

        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate BottomTemplate { get; set; }

        [TemplateContainer(typeof(NotesContainer))]
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ITemplate NotesTemplate { get; set; }

        public bool ToggleGISLayers { get; set; }

        public DateTime GISDataDate
        {
            get
            {
                return _gisDataDate ??
                       (_gisDataDate = DependencyResolver.Current.GetService<IGISLayerUpdateRepository>().GetCurrent()
                                                         .Updated).Value;
            }
        }

        #endregion

        #region Constructors

        public MapMenuBase()
        {
            ToggleGISLayers = true;
        }

        #endregion

        #region Private Methods

        protected virtual void Page_Init(object sender, EventArgs e)
        {
            gisLayersOuterPlaceHolder.Visible = ToggleGISLayers;

            RenderOrHide(LayersTemplate, layersOuterPlaceHolder, layersInnerPlaceHolder);
            RenderOrHide(LegendTemplate, legendOuterPlaceHolder, legendInnerPlaceHolder);
            RenderOrHide(OptionsTemplate, optionsOuterPlaceHolder, optionsInnerPlaceHolder);
            RenderOrHide(BottomTemplate, bottomPlaceHolder, bottomPlaceHolder);

            if (NotesTemplate != null)
            {
                var container = new NotesContainer(GISDataDate);
                NotesTemplate.InstantiateIn(container);
                notesPlaceHolder.Controls.Add(container);
            }
        }

        private void RenderOrHide(ITemplate template, PlaceHolder outerPlaceHolder, PlaceHolder innerPlaceHolder)
        {
            if (template != null)
            {
                template.InstantiateIn(innerPlaceHolder);
            }
            else
            {
                outerPlaceHolder.Visible = false;
            }
        }

        #endregion
    }

    public class NotesContainer : Control, INamingContainer
    {
        private readonly DateTime _gisDataDate, _mapDataDate;

        public string GISDataDate
        {
            get { return String.Format(CommonStringFormats.DATE, _gisDataDate); }
        }

        public string MapDataDate
        {
            get { return String.Format(CommonStringFormats.DATETIME_WITHOUT_SECONDS, _mapDataDate); }
        }

        internal NotesContainer(DateTime gisDataDate)
        {
            _gisDataDate = gisDataDate;
            _mapDataDate = DependencyResolver.Current.GetService<IDateTimeProvider>().GetCurrentDate();
        }
    }
}
