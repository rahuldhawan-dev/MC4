using System;
using System.Windows.Forms;
using System.Security.Principal;
using MMSINC.Interface;
using MMSINC.Utilities.Permissions;
using Subtext.TestLibrary;

namespace WorkOrderImportHelper
{
    public partial class frmMain : Form
    {
        #region Constants

        private struct MarkoutStatusStrings
        {
            public const string STARTING = "Loading WorkOrders with Markouts...",
                                FOUND_WORK_ORDERS =
                                    "Found {0} WorkOrders with Markouts.",
                                EXITING = "Finished processing Markout dates.";
        }

        private struct WorkOrderNotesStatusStrings
        {
            public const string STARTING =
                                    "Concatenating notes from EmployeeWorkOrders to WorkOrders...",
                                FOUND_WORK_ORDERS =
                                    "Found {0} WorkOrders with EmployeeWorkOrders",
                                EXITING = "Finished processing WorkOrder notes";
        }

        private struct WorkOrderImporterStatusStrings
        {
            public const string STARTING = "Processing Work Orders",
                                FOUND_WORK_ORDERS =
                                    "Found {0} WorkOrders",
                                EXITING = "Finished importing Work Orders";
        }

        #endregion

        #region Constructors

        public frmMain()
        {
            InitializeComponent();
        }

        #endregion

        #region Private Methods

        private void ClearOutput()
        {
            txtOutput.Text = string.Empty;
        }

        private void DisplayCurrentWorkOrderID(int workOrderID)
        {
            txtCurrentWorkOrderID.Text = workOrderID.ToString();
            Application.DoEvents();
        }

        private void Output(string format, params object[] args)
        {
            Output(string.Format(format, args));
        }

        private void Output(string output)
        {
            txtOutput.Text = output;
            Application.DoEvents();
        }

        private void OutputLines(string output)
        {
            txtOutput.AppendText(string.Format("{0}\r\n", output));
            Application.DoEvents();
        }

        private void FixMarkoutDates()
        {
            using (var simulator = new HttpSimulator().SimulateRequest())
            {
                WorkOrders.Library.Permissions.SecurityService.Instance.Init(new FullyPermittedUser());

                Output(MarkoutStatusStrings.STARTING);

                var fixer = new MarkoutWorkDayFixer(DisplayCurrentWorkOrderID,
                    Output);

                Output(MarkoutStatusStrings.FOUND_WORK_ORDERS,
                    fixer.LoadWorkOrders());

                fixer.ProcessMarkouts();

//                Output(MarkoutStatusStrings.EXITING);
            }
        }

        private void ConcatenateNotes()
        {
            Output(WorkOrderNotesStatusStrings.STARTING);

            var concatenator = new WorkOrderNotesConcatenator(DisplayCurrentWorkOrderID, Output);

            Output(WorkOrderNotesStatusStrings.FOUND_WORK_ORDERS, concatenator.LoadWorkOrders());

            concatenator.ProcessNotes();

            Output(WorkOrderNotesStatusStrings.EXITING);
        }

        private void ImportOrders()
        {
            using (var simulator = new HttpSimulator().SimulateRequest())
            {
                WorkOrders.Library.Permissions.SecurityService.Instance.Init(
                    new FullyPermittedUser());

                Output(WorkOrderImporterStatusStrings.STARTING);

                var importer = new WorkOrderImporter(DisplayCurrentWorkOrderID,
                    OutputLines);

                Output(WorkOrderImporterStatusStrings.FOUND_WORK_ORDERS,
                    importer.LoadWorkOrders());

                importer.Import();

                OutputLines(WorkOrderImporterStatusStrings.EXITING);
            }
        }

        private void BarrierCloseOuts()
        {
            using (var simulator = new HttpSimulator().SimulateRequest())
            {
                WorkOrders.Library.Permissions.SecurityService.Instance.Init(
                    new FullyPermittedUser());

                Output(WorkOrderImporterStatusStrings.STARTING);

                var importer = new WorkOrderImporter(DisplayCurrentWorkOrderID,
                    OutputLines);

                //Output(WorkOrderImporterStatusStrings.FOUND_WORK_ORDERS,
                //    importer.LoadWorkOrders());

                importer.CloseOutOrders();

                OutputLines(WorkOrderImporterStatusStrings.EXITING);
            }
        }
        
        #endregion

        #region Event Handlers
        
        private void btnImport_Click(object sender, EventArgs e)
        {
            //FixMarkoutDates();
            //ConcatenateNotes();
            //ImportOrders();
            BarrierCloseOuts();
        }

        #endregion
    }
    public class FullyPermittedUser : IUser
    {
        public bool IsInRole(string role)
        {
            return true;
        }

        public IIdentity Identity
        {
            get { throw new NotImplementedException(); }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public IPermissionsObject CanRead(IModulePermissions perm)
        {
            return new FullyPermissivePermissionsObject(ModuleAction.Read);
        }

        public IPermissionsObject CanAdd(IModulePermissions perm)
        {
            return new FullyPermissivePermissionsObject(ModuleAction.Add);
        }

        public IPermissionsObject CanEdit(IModulePermissions perm)
        {
            return new FullyPermissivePermissionsObject(ModuleAction.Edit);
        }

        public IPermissionsObject CanDelete(IModulePermissions perm)
        {
            return new FullyPermissivePermissionsObject(ModuleAction.Delete);
        }

        public IPermissionsObject CanAdministrate(IModulePermissions perm)
        {
            return new FullyPermissivePermissionsObject(ModuleAction.Administrate);
        }
    }

    public class FullyPermissivePermissionsObject : PermissionsObject
    {
        protected FullyPermissivePermissionsObject(IRoleManager roleManager, IUser user, IModulePermissions specificPermissions, ModuleAction action) : base(roleManager, user, specificPermissions, action) { }
        public FullyPermissivePermissionsObject(ModuleAction action) : this(null, null, null, action) { }

        public override bool In(string opCntr)
        {
            return true;
        }

        public override bool InAny()
        {
            return true;
        }
    }
}
