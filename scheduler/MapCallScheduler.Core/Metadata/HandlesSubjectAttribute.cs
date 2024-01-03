using System;
using System.Text.RegularExpressions;
using MMSINC.Interface;

namespace MapCallScheduler.Metadata
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class HandlesSubjectAttribute : HandlesMessageAttribute
    {
        #region Properties

        public string Regex { get; }

        #endregion

        #region Constructors

        public HandlesSubjectAttribute(Regex regex) : base(GetPredicate(regex))
        {
            Regex = regex.ToString();
        }

        public HandlesSubjectAttribute(string regex) : this(new Regex(regex)){}

        #endregion

        private static Func<IMailMessage, bool> GetPredicate(Regex regex)
        {
            return m =>
                regex.IsMatch(m.Subject);
        }
    }
}