using System;
using System.Data;
using System.Data.Common;
using System.Web.UI.WebControls;
using MMSINC.ClassExtensions.IOrderedDictionaryExtensions;
using MMSINC.Controls;

namespace MMSINC.DataPages
{
    // NOTE: Don't call DataBind on the DetailsView while it's in insert mode.
    //       All it does is blow up the page validators(making Page.IsValid return true
    //       after a validator has failed) and all the field values disappear.
    public abstract class DetailsViewDataPageBase : DataPageBase
    {
        #region Properties

        public virtual bool AutoAddDataKeyToDetailsView
        {
            get { return true; }
        }

        protected DetailsViewCrudField DetailsViewCrudField { get; private set; }

        protected abstract IDetailsView DetailsView { get; }

        protected SqlDataSource DetailsViewDataSource
        {
            get { return (SqlDataSource)DetailsView.DataSourceObject; }
        }

        /// <summary>
        /// Gets the DbTransaction being used if there's currently an insert/update occurring from the DetailsView. 
        /// </summary>
        protected DbTransaction Transaction { get; set; }

        /// <summary>
        /// Stores the DataRecordSavingArgs created during OnRecordSaving so it can be used durinng OnRecordSaved.
        /// </summary>
        internal DataRecordSavingEventArgs PreviousDataRecordSavingArgs { get; set; }

        #endregion

        #region Private Methods

        #region Page Lifecycle

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // TODO: Can probably kill this whole initializer when the PageMode isn't set to view Details.

            if (AutoAddDataKeyToDetailsView)
            {
                SetDataKeyNamesOnDetailsView();
            }

            // Add the required event handlers here, so there's no need to re-add these in the
            // markup of every single page.

            DetailsView.ModeChanged += OnDetailsViewModeChanged;
            DetailsView.ItemInserting += OnDetailsViewItemInserting;
            DetailsView.ItemUpdating += OnDetailsViewItemUpdating;
            DetailsView.ItemDeleting += OnDetailsViewItemDeleting;
            DetailsView.ItemCommand += OnDetailsViewItemCommand;
            DetailsViewDataSource.Inserted += OnDetailsViewDataSourceInserted;
            DetailsViewDataSource.Updated += OnDetailsViewDataSourceUpdated;
            DetailsViewDataSource.Inserting += OnDetailsViewDataSourceInserting;
            DetailsViewDataSource.Updating += OnDetailsViewDataSourceUpdating;
            DetailsViewDataSource.Deleting += OnDetailsViewDataSourceDeleting;
            DetailsViewDataSource.Deleted += OnDetailsViewDataSourceDeleted;

            // At this point all the Fields should have been added. This can't be
            // added any time after this(besides maybe InitComplete) or else none
            // of the postback events will work.
            DetailsViewCrudField = new DetailsViewCrudField(Permissions);
            DetailsView.Fields.Add(DetailsViewCrudField);
        }

        #endregion

        /// <summary>
        /// Does exactly what the name says it does so you don't have to.
        /// </summary>
        private void SetDataKeyNamesOnDetailsView()
        {
            DetailsView.DataKeyNames = AddPrimaryKeyToDataKeyNames(DetailsView.DataKeyNames);
        }

        /// <summary>
        /// Method called after inserting/updating when UseTransactionsWithDetailsView is set to true. This method call
        /// is wrapped in a transaction and will automatically rollback if an error occurs.
        /// </summary>
        protected virtual void PerformExtendedDataSaving(SqlDataSourceStatusEventArgs e,
            DataRecordSavingEventArgs savingArgs)
        {
            // Do nothing here. For inheritors only. 
        }

        protected override void LoadDataRecord(int recordId)
        {
            base.LoadDataRecord(recordId);

            var primaryKeyParam = DetailsViewDataSource.SelectParameters[DataElementPrimaryFieldName];
            if (primaryKeyParam == null)
            {
                throw new NullReferenceException(
                    "DetailsViewDataSource.SelectParameters does not contain a parameter for DataElementPrimaryFieldName '" +
                    DataElementPrimaryFieldName + "'.");
            }

            primaryKeyParam.DefaultValue = recordId.ToString();

            // This may cause issues if, at some point, there are more than one SelectParameters that need to have a 
            // value set prior to DataBind being called. 
            DetailsView.DataBind();
        }

        #region Transactions

        /* 
         These two methods deal with creating and properly disposing
         DbTransaction objects if the UseTransaction property is true.
         */

        protected virtual void SetupTransaction(SqlDataSourceCommandEventArgs e)
        {
            var conn = e.Command.Connection;

            // Can't make a transaction if the connection isn't open.
            // Since this is manually opened, it needs to be manually
            // closed too.
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            // Transaction is stored because the command's transaction
            // property is being nulled by SqlDataSource or DetailsView
            // somewhere. 
            Transaction = conn.BeginTransaction();
            e.Command.Transaction = Transaction;
        }

        protected virtual DbCommand CreateTransactionCommand(string commandText = null)
        {
            if (Transaction == null)
            {
                throw new NullReferenceException("Transaction");
            }

            var comm = Transaction.Connection.CreateCommand();
            comm.Transaction = Transaction;
            comm.CommandType = CommandType.Text; // .Net defaults to this, but putting it in anyway.
            comm.CommandText = commandText;
            return comm;
        }

        protected virtual void CompleteTransaction(SqlDataSourceStatusEventArgs e, DataRecordSaveTypes saveType)
        {
            using (Transaction)
            {
                // Transactions null their Connection property after the Transaction
                // is no longer usable(ie after rollback/commit). So this needs
                // to be stored locally. 
                var conn = Transaction.Connection;

                try
                {
                    PerformExtendedDataSaving(e, PreviousDataRecordSavingArgs);

                    // Rollback any changes if the client's disconnected. This can be from them
                    // closing the browser, hitting the back button, or any other thing that could
                    // cause them to leave mid-transaction. That information shouldn't be stored. 
                    if (IResponse.IsClientConnected)
                    {
                        Transaction.Commit();
                    }
                    else
                    {
                        Transaction.Rollback();
                    }
                }
                catch (Exception)
                {
                    Transaction.Rollback();
                    throw;
                }
                finally
                {
                    // Connection needs to be manually closed because it was manually opened.
                    if (conn.State != ConnectionState.Closed)
                    {
                        conn.Close();
                    }

                    Transaction = null;
                }
            }
        }

        #endregion

        #endregion

        #region Event Handlers

        protected virtual void OnDetailsViewItemCommand(object sender, DetailsViewCommandEventArgs e)
        {
            // Use this instead of having btnCancels all over the place and having to find them.
            if (e.CommandName.Equals("Cancel", StringComparison.OrdinalIgnoreCase))
            {
                if (PageMode == PageModes.RecordInsert)
                {
                    RedirectPageToSearch();
                }
                else
                {
                    RedirectPageToRecord(CurrentDataRecordId);
                }
            }
        }

        protected virtual void OnDetailsViewModeChanged(object sender, EventArgs e)
        {
            // This is for making sure that the PageMode is properly set if
            // the DetailsView mode change is coming from the DetailsView itself
            // and not from a call in this class.
            if (DetailsView.CurrentMode == DetailsViewMode.Edit)
            {
                PageMode = PageModes.RecordUpdate;
            }
        }

        /// <remarks>
        /// 
        /// If you need to cancel an insert to the database, set the e.Cancel property to true here.
        /// Setting the Cancel property on the SqlDataSource's OnInserting event will cancel the command,
        /// but the DetailView will display nothing on the screen after posting back. 
        /// 
        /// </remarks>
        protected virtual void OnDetailsViewItemInserting(object sender, DetailsViewInsertEventArgs e)
        {
            e.Values.CleanValues();
            var createdBy = DetailsViewDataSource.InsertParameters["CreatedBy"];
            if (createdBy != null)
            {
                createdBy.DefaultValue = IUser.Name;
            }

            PreviousDataRecordSavingArgs =
                new DataRecordSavingEventArgs(DataRecordSaveTypes.Insert, CurrentDataRecordId, e.Values, null);
            OnRecordSaving(PreviousDataRecordSavingArgs);
            e.Cancel = PreviousDataRecordSavingArgs.Cancel;
        }

        protected virtual void OnDetailsViewItemUpdating(object sender, DetailsViewUpdateEventArgs e)
        {
            e.NewValues.CleanValues();
            PreviousDataRecordSavingArgs = new DataRecordSavingEventArgs(DataRecordSaveTypes.Update,
                CurrentDataRecordId, e.NewValues, e.OldValues);
            OnRecordSaving(PreviousDataRecordSavingArgs);
            e.Cancel = PreviousDataRecordSavingArgs.Cancel;
        }

        protected virtual void OnDetailsViewItemDeleting(object sender, DetailsViewDeleteEventArgs e)
        {
            PreviousDataRecordSavingArgs =
                new DataRecordSavingEventArgs(DataRecordSaveTypes.Delete, CurrentDataRecordId, null);
            OnRecordDeleting(e);
        }

        protected void OnDetailsViewDataSourceInserting(object sender, SqlDataSourceCommandEventArgs e)
        {
            SetupTransaction(e);
        }

        protected void OnDetailsViewDataSourceUpdating(object sender, SqlDataSourceCommandEventArgs e)
        {
            SetupTransaction(e);
        }

        protected void OnDetailsViewDataSourceDeleting(object sender, SqlDataSourceCommandEventArgs e)
        {
            SetupTransaction(e);
        }

        private static void ValidateDetailsViewDataSourceCommand(IDbTransaction trans, SqlDataSourceStatusEventArgs e)
        {
            if (e.Exception != null)
            {
                trans.Rollback();

                // Throw this so that sql exceptions that are eaten by DetailsView/DataSource don't
                // continue to go on being eaten.
                throw e.Exception;
            }
        }

        protected virtual void OnDetailsViewDataSourceInserted(object sender, SqlDataSourceStatusEventArgs e)
        {
            ValidateDetailsViewDataSourceCommand(Transaction, e);

            // @ needs to be added to the primary field name parameter. The SqlDataSource has
            // its parameter names without "@", but when it creates the DbCommand it uses, it
            // then adds the "@" sign to it.
            var primaryParameter = e.Command.Parameters["@" + DataElementPrimaryFieldName];

            var id = int.Parse(primaryParameter.Value.ToString());
            base.CurrentDataRecordId = id;

            // CurrentDataRecordId is set prior to additional transactions so inheritors can
            // have that value easily accessible. 

            CompleteTransaction(e, DataRecordSaveTypes.Insert);

            OnRecordSaved(new DataRecordSavedEventArgs(DataRecordSaveTypes.Insert, id,
                PreviousDataRecordSavingArgs.Values));
        }

        protected virtual void OnDetailsViewDataSourceUpdated(object sender, SqlDataSourceStatusEventArgs e)
        {
            ValidateDetailsViewDataSourceCommand(Transaction, e);
            CompleteTransaction(e, DataRecordSaveTypes.Update);
            OnRecordSaved(new DataRecordSavedEventArgs(DataRecordSaveTypes.Update, CurrentDataRecordId,
                PreviousDataRecordSavingArgs.Values, PreviousDataRecordSavingArgs.OldValues));
        }

        protected virtual void OnDetailsViewDataSourceDeleted(object sender, SqlDataSourceStatusEventArgs e)
        {
            ValidateDetailsViewDataSourceCommand(Transaction, e);
            CompleteTransaction(e, DataRecordSaveTypes.Delete);
            OnRecordSaved(new DataRecordSavedEventArgs(DataRecordSaveTypes.Delete, CurrentDataRecordId, null));
        }

        protected override void OnPageModeChanged(PageModes newMode)
        {
            base.OnPageModeChanged(newMode);

            switch (newMode)
            {
                case PageModes.RecordReadOnly:
                    DetailsView.ChangeMode(DetailsViewMode.ReadOnly);

                    break;

                case PageModes.RecordInsert:

                    if (DetailsView.AutoGenerateRows)
                    {
                        // Throwing this because AutoGenerateRows does not generate anything when DetailsView is in InsertMode. It requires
                        // that a row be able to be selected so it can figure out what parameters it needs. Why? I have no idea, considering
                        // that data source has InsertParameters and all. 
                        throw new Exception(
                            "DataPageWithDetailsElement does not support DetailsView with the AutoGenerateRows set to true.");
                    }

                    DetailsView.ChangeMode(DetailsViewMode.Insert);

                    break;

                case PageModes.RecordUpdate:
                    DetailsView.ChangeMode(DetailsViewMode.Edit);

                    break;
            }
        }

        #endregion
    }
}
