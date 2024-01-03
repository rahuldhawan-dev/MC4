using System;
using System.Web.Mvc;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using MapCall.Common.Model.Entities;
using MapCall.Common.Utility.Notifications;
using MapCallMVC.Models.ViewModels;
using MMSINC.Controllers;
using MMSINC.Data.NHibernate;
using ControllerBase = MMSINC.Controllers.ControllerBase;

namespace MapCallMVC.Controllers
{
    public class NotificationController : ControllerBase
    {
        #region Constants

        public const string FRAGMENT_IDENTIFIER = "#EmployeesTab";
        public const string SUCCESS_MESSAGE = "Notification Sent";

        #endregion

        #region Private Methods

        private byte[] CreateTrainingRecordICalendar(TrainingRecord trainWreck)
        {
            // This should be easy enough to refactor into a reusable wrapper if this
            // ever needs to be used again.
            var cal = new Ical.Net.Calendar();
            foreach (var sess in trainWreck.TrainingSessions)
            {
                var e = new CalendarEvent();
                e.DtStart = new CalDateTime(sess.StartDateTime);
                e.DtEnd = new CalDateTime(sess.EndDateTime);
                // There are a very small handful of training records without a class location set.
                e.Location = trainWreck.ClassLocation?.ToString();
                e.Summary = $"Training: {trainWreck.TrainingModule}";
                cal.Events.Add(e);
            }

            var serializer = new Ical.Net.Serialization.CalendarSerializer(cal);
            var serializedEvent = serializer.SerializeToString();
            var serializedBytes = System.Text.Encoding.UTF8.GetBytes(serializedEvent);
            cal.Dispose(); // This dispose method isn't IDisposable so it can't be in a using statement. Good programming!
            return serializedBytes;
        }

        private void SendCreationsMostBodaciousNotification(CreateNotification notification)
        {
            var notifier = _container.GetInstance<INotificationService>();
            var entityType = Type.GetType(notification.EntityType);
            var repositoryType = typeof(IRepository<>).MakeGenericType(entityType);
            dynamic repository = _container.GetInstance(repositoryType);
            var model = repository.Find(notification.Id);

            // if the model has a recordUrl property, attempt to set it
            // GetUrlForModel(model, "Show", "TrainingRecord");
            if (model.GetType().GetProperty("RecordUrl") != null)
            {
                if (model.RecordUrl == null)
                    model.RecordUrl = GetUrlForModel(model, "Show", entityType.Name);
            }
            // if the entity type is training record send to the employees instead of the notification group
            if (entityType.Name == "TrainingRecord")
            {
                var trainWreck = (TrainingRecord)model;
                var calendar = CreateTrainingRecordICalendar(trainWreck);
               
                var employees = trainWreck.EmployeesScheduled;
                foreach (var employee in employees)
                {
                    if (employee.Employee?.EmailAddress != null)
                    {
                        var args = new NotifierArgs
                        {
                            OperatingCenterId = 0,
                            Module = notification.RoleModule,
                            Purpose = notification.NotificationPurpose,
                            Data = model,
                            Address = employee.Employee?.EmailAddress,
                            Subject = "MapCall - Training Notification"
                        };

                        args.AddAttachment("training.ics", calendar);

                        notifier.Notify(args);
                    }
                }
            }
            else // send the usual notification
            {
                var args = new NotifierArgs
                {
                    OperatingCenterId = notification.OperatingCenterId,
                    Module = notification.RoleModule,
                    Purpose = notification.NotificationPurpose,
                    Data = model
                };

                notifier.Notify(args);
            }
        }

        #endregion

        [HttpPost]
        public ActionResult Create(CreateNotification model)
        {
            SendCreationsMostBodaciousNotification(model);
            DisplaySuccessMessage(SUCCESS_MESSAGE);
            return RedirectToReferrerOr("Index", "Home", FRAGMENT_IDENTIFIER);
        }

        public NotificationController(ControllerBaseArguments args) : base(args) { }
    }
}
