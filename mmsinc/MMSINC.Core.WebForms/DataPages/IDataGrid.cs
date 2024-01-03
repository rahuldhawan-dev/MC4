namespace MMSINC.DataPages
{
    // This interface/class isn't being used yet. It's just a placeholder for eventually
    // making an exportable GridView class that will work with the excel export project.
    //
    // Right now this is just a copy of the H2OSurveyReportResult class in MapCall. 
    //
    //
    public interface IDataGrid
    {
        #region Properties

        string SelectCommand { get; set; }

        #endregion

        #region Methods

        void ApplyFilter(IFilterBuilder builder);

        // Should there be an ExcelOptions type class that says "Hey, export me asa worksheet, or a workbook, I dunno"?
        //
        // Also, this will be required to return an Excel object of some sort. 
        // Let whoever's requesting the export deal with how to serialize it and return it. 
        object ExportToExcel();

        #endregion
    }

    // Commenting this out due to ambiguous name errors I don't feel like fixing yet.

    //[ParseChildren(true)]
    //public class DataGrid : MvpPlaceHolder, IDataGrid
    //{
    //    #region Fields

    //    // Do not call these directly! They are lazy loaded by their property getters!
    //    private MvpGridView _gridView;
    //    private SqlDataSource _dataSource;

    //    #endregion

    //    #region Properties

    //    [PersistenceMode(PersistenceMode.InnerProperty)]
    //    public DataControlFieldCollection Columns { get { return GridView.Columns; } }

    //    public string SelectCommand
    //    {
    //        get { return DataSource.SelectCommand; }
    //        set { DataSource.SelectCommand = value; }
    //    }

    //    public MvpGridView GridView
    //    {
    //        get
    //        {
    //            if (_gridView == null)
    //            {
    //                _gridView = new MvpGridView();
    //            }
    //            return _gridView;
    //        }
    //    }

    //    public SqlDataSource DataSource
    //    {
    //        get
    //        {
    //            if (_dataSource == null)
    //            {
    //                _dataSource = new SqlDataSource();
    //            }
    //            return _dataSource;
    //        }
    //    }

    //    public bool AutoGenerateColumns
    //    {
    //        get { return GridView.AutoGenerateColumns; }
    //        set { GridView.AutoGenerateColumns = value; }
    //    }

    //    #endregion

    //    #region Private Methods

    //    private void InitControls()
    //    {
    //        this.GridView.DataSource = _dataSource;

    //        this.Controls.Add(this.GridView);
    //        this.Controls.Add(this.DataSource);
    //    }

    //    protected override void OnInit(EventArgs e)
    //    {
    //        base.OnInit(e);
    //        InitControls();
    //        this.DataSource.SelectCommand = this.SelectCommand;
    //    }

    //    #endregion

    //    #region Public Methods

    //    public void ApplyFilter(IFilterBuilder fb)
    //    {
    //        if (fb == null)
    //        {
    //            throw new ArgumentNullException("fb");
    //        }
    //        // filt will always have the original SelectCommand at the beginning, so it
    //        // should never ever be null. 
    //        var filt = fb.BuildCompleteCommand();

    //        this.DataSource.SelectCommand = filt;

    //        // Watch out here if ApplyFilter ever needs to be called
    //        // more than once. A duplicate parameter error will be
    //        // thrown. 

    //        var sp = this.DataSource.SelectParameters;
    //        foreach (var p in fb.BuildParameters())
    //        {
    //            sp.Add(p);
    //        }

    //        this.GridView.DataBind();

    //    }

    //    public object ExportToExcel()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    #endregion
    //}
}
