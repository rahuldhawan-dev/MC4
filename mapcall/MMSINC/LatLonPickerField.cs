using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MMSINC
{
    /// <summary>
    /// Represents a custom BoundField object that's capable of working with Coordinate
    /// data for our objects.
    /// </summary>
    public sealed class LatLonPickerField : System.Web.UI.WebControls.BoundField
    {
        #region Constants

        // if changing this, you should also change FormatStrings.GET_COORDS_QUERY
        private static int DEFAULT_ICON_ID = 5;

        private struct FormatStrings
        {
            /// <summary>
            /// Requires 1 value, the value of m_iInstances for the current instance.
            /// </summary>
            public const string TXT_LAT_CLIENT_ID = "txtLat{0}";
            /// <summary>
            /// Requires 1 value, the value of m_iInstances for the current instance.
            /// </summary>
            public const string TXT_LON_CLIENT_ID = "txtLon{0}";
            /// <summary>
            /// Requires 2 values, the ClientID of the textbox to put the latitude into,
            /// and the ClientID of textbox to put the longitude into.
            /// </summary>
            public const string SHOW_COORD_PICKER_SCRIPT = "new MMSINC.Mapping.LatLonPicker($('#{0}'),$('#{1}')).show();return false;";
            /// <summary>
            /// Requires 2 values, the ClientIDs of both the imagebutton whose src attribute may get replaced,
            /// and a hidden input field into which to place the IconID of the user-selected icon.
            /// </summary>
            public const string SHOW_ICON_PICKER_SCRIPT = "new MMSINC.Mapping.IconPicker($('#{0}'),$('#{1}')).show();return false;";
            /// <summary>
            /// Requires 1 value, the CoordinateID that's been passed into this control.
            /// </summary>
            public const string GET_COORDS_QUERY = "SELECT [Latitude], [Longitude], IsNull([IconID], {0}) AS [IconID] FROM [Coordinates] WHERE [CoordinateID] = {1};";
            /// <summary>
            /// Requires 3 values, the latitude, longitude, and iconid of the coordinate we're searching for.
            /// </summary>
            public const string LOOKUP_COORD_QUERY = "SELECT [CoordinateID] FROM [Coordinates] WHERE [Latitude] = {0} AND [Longitude] = {1} AND [IconID] = {2};";
            /// <summary>
            /// Requires 3 values, latitude, longitude, and iconid.
            /// </summary>
            public const string CREATE_COORD_QUERY = "INSERT INTO [Coordinates] ([Latitude], [Longitude], [IconID]) VALUES ({0}, {1}, {2});SELECT @@IDENTITY;";
            /// <summary>
            /// Requires 1 value, the IconID of the coordinate object that's been passed
            /// into this control.
            /// </summary>
            public const string GET_ICON_QUERY = "SELECT [IconURL], [Width], [Height] FROM [MapIcon] WHERE [IconID] = {0}";
            /// <summary>
            /// Requires 2 values, the current Latitude and Longitude of this control.
            /// </summary>
            public const string CELL_DISPLAY_FORMAT = "Latitude: {0}, Longitude: {1}, Icon: ";
            /// <summary>
            /// Requires 1 value, a string representing the image's filename.
            /// </summary>
            public const string ICON_URL = "~/images/{0}";
        }

        private struct LabelText
        {
            public const string DEFAULT_HEADER_TEXT = "Coordinates";
            public const string LATITUDE_LABEL = "Latitude: ";
            public const string LONGITUDE_LABEL = "Longitude: ";
        }

        private struct ImageURLS
        {
            public const string MAP = "applications-internet.png";
            public const string DEFAULT_ICON = "m_ValB.png";
        }

        private struct ScriptURLS
        {
            public const string LIGHT_VIEW = "~/includes/lightview-3.2.7/js/lightview.js";
            public const string MAPPING = "~/includes/Mapping.js";
        }

        private struct ToolTipStrings
        {
            public const string MAP = "Choose a point from the map.";
            public const string ICON = "Choose an icon to display for this object on the map.";
        }

        #endregion

        #region Control Declarations

        private Literal litLat, litLon;
        private TextBox txtLat, txtLon;
        private RequiredFieldValidator rfvLat, rfvLon;
        private ImageButton ibMap, ibIcon;
        private HiddenField hidIcon;

        #endregion

        #region Private Members

        private string m_strHeaderText = LabelText.DEFAULT_HEADER_TEXT;
        private float? m_fLatitude, m_fLongitude;
        private int? m_iCoordinateID, m_iIconID;
        private string coordinateDatabase;

        #region Private Static Members

        // class variable to track instances of this class.
        // useful for creating unique client ids
        private static Unit m_uDefaultImageSize = new Unit(20);
        private int _defaultIconID;

        #endregion

        #endregion

        #region Properties

        public override string DataField
        {
            get
            {
                string strDataField = base.DataField;
                return (String.IsNullOrEmpty(strDataField)) ? "CoordinateID" : strDataField;
            }
            set { base.DataField = value; }
        }

        public float? Latitude
        {
            get
            {
                if (m_fLatitude == null && txtLat != null && !String.IsNullOrEmpty(txtLat.Text))
                    m_fLatitude = float.Parse(txtLat.Text);
                return m_fLatitude;
            }
            set
            {
                m_fLatitude = value;
                if (txtLat != null)
                    txtLat.Text = m_fLatitude.ToString();
            }
        }

        public float? Longitude
        {
            get
            {
                if (m_fLongitude == null && txtLon != null && !String.IsNullOrEmpty(txtLon.Text))
                    m_fLongitude = float.Parse(txtLon.Text);
                return m_fLongitude;
            }
            set
            {
                m_fLongitude = value;
                if (txtLon != null)
                    txtLon.Text = m_fLongitude.ToString();
            }
        }

        public int? IconID
        {
            get
            {
                if (m_iIconID == null && hidIcon != null && !String.IsNullOrEmpty(hidIcon.Value))
                    m_iIconID = int.Parse(hidIcon.Value);
                // If its still null, lets default it
                return m_iIconID ?? DefaultIconID;
            }
            set
            {
                m_iIconID = value;
                if (hidIcon != null)
                    hidIcon.Value = m_iIconID.ToString();
            }
        }

        public override string HeaderText
        {
            get { return m_strHeaderText; }
            set { base.HeaderText = m_strHeaderText = value; }
        }

        // TODO: Need a coordinate object.

        public int? CoordinateID
        {
            get
            {
                if (m_iCoordinateID == null)
                    m_iCoordinateID = GetCoordinateID();
                return m_iCoordinateID;
            }
            set { m_iCoordinateID = value; }
        }

        public string CoordinateDatabase
        {
            get
            {
                if (String.IsNullOrEmpty(coordinateDatabase))
                    coordinateDatabase = "MCProd";
                return coordinateDatabase;
            }
            set
            {
                coordinateDatabase = value;
            }
        }

        public bool Required { get; set; }

        public int DefaultIconID
        {
            get
            {
                if (_defaultIconID == 0)
                    _defaultIconID = DEFAULT_ICON_ID;
                return _defaultIconID;
            } 
            set { _defaultIconID = value; }
        }

        #region Private Properties
        
        private string IconURL
        {
            get;
            set;
        }

        private int IconWidth
        {
            get;
            set;
        }

        private int IconHeight
        {
            get;
            set;
        }

        #endregion

        #endregion

        #region Constructors

        public LatLonPickerField()
            : base()
        {
        }

        public LatLonPickerField(string database)
            : base()
        {
            coordinateDatabase = database;
        }

        #endregion

        #region Private Methods

        private int? LookupCoordinate()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CoordinateDatabase].ConnectionString))
            {
                using (var cmd = new SqlCommand(String.Format(FormatStrings.LOOKUP_COORD_QUERY, Latitude, Longitude, (IconID == null) ? "NULL" : IconID.ToString()), conn))
                {
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                    {
                        int? ret = null;
                        if (rdr.Read())
                            ret = rdr.GetInt32(0);
                        rdr.Close();
                        return ret;
                    }
                }
            }
        }

        private void GetCoordinateData()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CoordinateDatabase].ConnectionString))
            {
                using (var cmd = new SqlCommand(String.Format(FormatStrings.GET_COORDS_QUERY, DefaultIconID, CoordinateID), conn))
                {
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                    {
                        if (rdr.Read())
                        {
                            Latitude = float.Parse(rdr.GetValue(0).ToString());
                            Longitude = float.Parse(rdr.GetValue(1).ToString());
                            IconID = rdr.GetInt32(2);
                        }
                    }
                }
            }
        }

        private void GetIconData()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings["mcprod"].ConnectionString))
            {
                using (var cmd = new SqlCommand(String.Format(FormatStrings.GET_ICON_QUERY, IconID), conn))
                {
                    conn.Open();
                    using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow | CommandBehavior.CloseConnection))
                    {
                        if (rdr.Read())
                        {
                            IconURL = rdr.GetString(0);
                            IconWidth = rdr.GetInt32(1);
                            IconHeight = rdr.GetInt32(2);
                        }
                    }
                }
            }
        }

        private int? GetCoordinateID()
        {
            // not even worth looking if either of these are null
            if (Latitude == null || Longitude == null)
                return null;

            // this uses Latitude, Longitude, and IconID to search
            // for an existing CoordinateID
            int? id = LookupCoordinate();

            // if id is still null here then a new coordinate must be created,
            // and its id returned
            return (id == null) ? CreateCoordinate() : id;
        }

        private int CreateCoordinate()
        {
            using (var conn = new SqlConnection(ConfigurationManager.ConnectionStrings[CoordinateDatabase].ConnectionString))
            {
                using (var cmd = new SqlCommand(string.Format(FormatStrings.CREATE_COORD_QUERY, Latitude, Longitude, IconID ?? DefaultIconID), conn))
                {
                    conn.Open();
                    int iRet = int.Parse(cmd.ExecuteScalar().ToString());
                    return iRet;
                }
            }
        }

        private ImageButton CreateImageButton(string strURL, string strToolTip)
        {
            return new ImageButton()
            {
                ImageUrl = VirtualPathUtility.ToAbsolute(string.Format(FormatStrings.ICON_URL, strURL)),
                Height = m_uDefaultImageSize,
                ToolTip = strToolTip
            };
        }

        #endregion

        #region BoundField Methods

        public override void ExtractValuesFromCell(IOrderedDictionary dictionary, DataControlFieldCell cell, DataControlRowState rowState, bool includeReadOnly)
        {
            base.ExtractValuesFromCell(dictionary, cell, rowState, includeReadOnly);
            if (cell.Controls.Count > 6)
            {
                txtLat = (TextBox)cell.Controls[5];
                txtLon = (TextBox)cell.Controls[11];
                ibIcon = (ImageButton)cell.Controls[15];
                hidIcon = (HiddenField)cell.Controls[16];
            }
            int? icoord = CoordinateID;
            if (dictionary.Contains(DataField))
                dictionary[DataField] = CoordinateID;
            else
                dictionary.Add(DataField, CoordinateID);
        }

        protected override void InitializeDataCell(DataControlFieldCell cell, DataControlRowState rowState)
        {
            base.InitializeDataCell(cell, rowState);
            // This method only gets called when being displayed as a static cell,
            // so this is the best place for this to happen.  However, the cell
            // won't have text yet, so we have to hook into PreRender to grab
            // the value, figure out lat/lon, and re-populate it with something
            // a little more useful.
            cell.PreRender += (object sender, EventArgs e) =>
            {
                int iTmp;
                if (!String.IsNullOrEmpty(cell.Text) && Int32.TryParse(cell.Text, out iTmp))
                {
                    CoordinateID = iTmp;
                    GetCoordinateData();
                    GetIconData();
                    cell.Controls.Clear();
                    cell.Controls.Add(new Label()
                    {
                        Text = string.Format(FormatStrings.CELL_DISPLAY_FORMAT, Latitude, Longitude)
                    });
                    cell.Controls.Add(new Image()
                    {
                        ImageUrl = VirtualPathUtility.ToAbsolute(string.Format(FormatStrings.ICON_URL, IconURL)),
                        Width = IconWidth,
                        Height = IconHeight
                    });
                }
            };
        }

        public override void InitializeCell(DataControlFieldCell cell, DataControlCellType cellType, DataControlRowState rowState, int rowIndex)
        {
            if (Control.Page != null)
            {
                ScriptManager mgr = ScriptManager.GetCurrent(Control.Page);
           //     mgr.Scripts.Add(new ScriptReference(ScriptURLS.PROTOTYPE));
          //      mgr.Scripts.Add(new ScriptReference(ScriptURLS.SCRIPTACULOUS));
                mgr.Scripts.Add(new ScriptReference(ScriptURLS.LIGHT_VIEW));
                mgr.Scripts.Add(new ScriptReference(ScriptURLS.MAPPING));
            }

            // defer to the base method so we get the value for [CoorinateID] from the textbox
            // that a BoundField will create by default
            base.InitializeCell(cell, cellType, rowState, rowIndex);

            // no need to bother if this is the header cell
            if (cellType != DataControlCellType.DataCell)
                return;

            if (cell != null && cell.Controls.Count == 1 && cell.Controls[0] is TextBox)
            {
                // snatch up the text box that was provided by default
                txtLat = (TextBox)cell.Controls[0];
                txtLat.ID = String.Format("txtLat{0}_{1}", DataField, rowIndex);
                txtLon = new TextBox() { ID = String.Format("txtLon{0}_{1}", DataField, rowIndex) };

                //required field validators based on Required property
                rfvLat = new RequiredFieldValidator { ControlToValidate = txtLat.ID, Text = "Required", Enabled = Required };
                rfvLon = new RequiredFieldValidator { ControlToValidate = txtLon.ID, Text = "Required", Enabled = Required };

                // labels, with a spacer
                litLat = new Literal() { Text = LabelText.LATITUDE_LABEL };
                litLon = new Literal() { Text = " " + LabelText.LONGITUDE_LABEL };

                // image buttons for user interaction
                ibMap = CreateImageButton(ImageURLS.MAP, ToolTipStrings.MAP);
                ibIcon = CreateImageButton(ImageURLS.DEFAULT_ICON, ToolTipStrings.ICON);

                hidIcon = new HiddenField() { Value = IconID.ToString() };

                // big, ugly control building block
                cell.Controls.Clear();
                cell.Controls.Add(new Literal {Text = "<table><tr>"}); // 0
                cell.Controls.Add(new Literal {Text = "<td>"});        // 1
                cell.Controls.Add(litLat);                             // 2
                cell.Controls.Add(new Literal { Text = "</td>" });     // 3
                cell.Controls.Add(new Literal { Text = "<td>" });      // 4
                cell.Controls.Add(txtLat);                             // 5
                cell.Controls.Add(new Literal { Text = "</td>" });     // 6
                cell.Controls.Add(new Literal { Text = "<td>" });      // 7
                cell.Controls.Add(litLon);                             // 8
                cell.Controls.Add(new Literal { Text = "</td>" });     // 9
                cell.Controls.Add(new Literal { Text = "<td>" });      // 10
                cell.Controls.Add(txtLon);                             // 11
                cell.Controls.Add(new Literal { Text = "</td>" });     // 12
                cell.Controls.Add(new Literal { Text = "<td>" });      // 13
                cell.Controls.Add(ibMap);                              // 14
                cell.Controls.Add(ibIcon);                             // 15
                cell.Controls.Add(hidIcon);                            // 16
                cell.Controls.Add(new Literal { Text = "</td>" });     // 17
                cell.Controls.Add(new Literal { Text = "</tr><tr>" }); // 18
                cell.Controls.Add(new Literal { Text = "<td></td>" }); // 19 - cell underneath litLat
                cell.Controls.Add(new Literal { Text = "<td>" });      // 20
                cell.Controls.Add(rfvLat);                             // 21
                cell.Controls.Add(new Literal { Text = "</td>" });     // 22
                cell.Controls.Add(new Literal { Text = "<td></td>" }); // 23 - cell underneath litLon
                cell.Controls.Add(new Literal { Text = "<td>" });      // 24
                cell.Controls.Add(rfvLon);                             // 25
                cell.Controls.Add(new Literal { Text = "</td></tr></table>" }); // 26
                

                cell.Load += (object sender, EventArgs e) =>
                {
                    // needed to hook into cell.Load for this because the ClientIDs
                    // for the textboxen don't exist on cell.Load
                    ibMap.OnClientClick = String.Format(FormatStrings.SHOW_COORD_PICKER_SCRIPT, txtLat.ClientID, txtLon.ClientID);
                    ibIcon.OnClientClick = String.Format(FormatStrings.SHOW_ICON_PICKER_SCRIPT, ibIcon.ClientID, hidIcon.ClientID);
                };

                cell.PreRender += (object sender, EventArgs e) =>
                {
                    // There's something really messed up somewhere that is setting txtLat.Text
                    // to the CoordinateID *sometimes* and the original code here was trying to parse
                    // it out. Other times, it has the Latitude in it like you'd expect something like
                    // txtLat to have. The orignial code would fail every time trying to parse out an
                    // integer from a latitude value. 
                    //
                    // NOTE: THIS IS NOT A FIX. IT IS A "FIX". IT IS BAD AND I FEEL BAD. -Ross 1/25/2013
                    if (!CoordinateID.HasValue)
                    {
                        var strCoordinateID = txtLat.Text;
                        if (!String.IsNullOrEmpty(strCoordinateID))
                        {
                            // gather the coordinate data from CoordinateID,
                            // which has been provided by the default textbox
                            // that BoundField created for us
                            int todoPLEASEFIXMECoordinateId;
                            if (int.TryParse(strCoordinateID, out todoPLEASEFIXMECoordinateId))
                            {
                                CoordinateID = todoPLEASEFIXMECoordinateId;
                            }
                        }
                    }

                    if (CoordinateID.HasValue)
                    {
                        // gather the coordinate data from CoordinateID,
                        // which has been provided by the default textbox
                        // that BoundField created for us
                        GetCoordinateData();
                        GetIconData();
                        ibIcon.ImageUrl = VirtualPathUtility.ToAbsolute(string.Format(FormatStrings.ICON_URL, IconURL));
                        hidIcon.Value = IconID.ToString();
                    }

                    // This is terriwrong and I'm leaving it commented out for the time being.
                    //string strCoordinateID = txtLat.Text;
                    //if (!String.IsNullOrEmpty(strCoordinateID))
                    //{
                    //    // gather the coordinate data from CoordinateID,
                    //    // which has been provided by the default textbox
                    //    // that BoundField created for us
                    //    CoordinateID = Int32.Parse(strCoordinateID);
                    //    GetCoordinateData();
                    //    GetIconData();
                    //    ibIcon.ImageUrl = VirtualPathUtility.ToAbsolute(string.Format(FormatStrings.ICON_URL, IconURL));
                    //    hidIcon.Value = IconID.ToString();
                    //}
                };
            }
        }

        #endregion
    }
}