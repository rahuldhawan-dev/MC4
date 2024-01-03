using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data.NHibernate;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MMSINC.Authentication
{
    /// <summary>
    /// Base NHibernate repository capable of authenticating users and resetting their passwords.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class AuthenticationRepositoryBase<TEntity> : RepositoryBase<TEntity>,
        IAuthenticationRepository<TEntity>, IForgotPasswordRepository<TEntity>
        where TEntity : class, IAdministratedUser, IRetrievablePasswordUser
    {
        #region Fields

        // This can be static readonly because DefaultCredentialPolicy is a threadsafe object.
        private static readonly DefaultCredentialPolicy _defaultCredentialPolicy = new DefaultCredentialPolicy();

        #endregion

        #region Properties

        public virtual ICredentialPolicy CredentialPolicy
        {
            get { return _defaultCredentialPolicy; }
        }

        #endregion

        #region Constructors

        public AuthenticationRepositoryBase(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Private Methods

        protected virtual bool ValidateEmailAddress(string email)
        {
            return email.IsValidEmail();
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Empty method meant to be overridden in inheriting classes.  The
        /// override should run any necessary extra validation logic, returning
        /// a boolean value to indicate wether or not the given email address,
        /// password, or typed user object are valid.  The reference status
        /// parameter should be set to any needed status code, but will be
        /// ignored if the override returns true.
        /// This method will be called after the email address is validated,
        /// but prior to validation of the password.
        /// </summary>
        public virtual bool FurtherValidate(string email, string password, TEntity user,
            ref UserLoginAttemptStatus status)
        {
            return true;
        }

        /// <summary>
        /// Returns a UserLoginAttemptStatus value indicating whether the given
        /// email address and password are valid, and if not, why.
        /// </summary>
        public UserLoginAttemptStatus ValidateUser(string email, string password)
        {
            if (!ValidateEmailAddress(email))
            {
                return UserLoginAttemptStatus.InvalidEmail;
            }

            var user = GetUser(email);
            if (user == null)
            {
                return UserLoginAttemptStatus.UnknownUser;
            }

            if (!user.HasAccess)
            {
                return UserLoginAttemptStatus.AccessDisabled;
            }

            // I'm not sure that FurtherValidate is used anywhere. Contractors was using it, but it was doing
            // a redundant HasAccess check. -Ross 9/20/2017
            UserLoginAttemptStatus status = UserLoginAttemptStatus.Success;
            if (!FurtherValidate(email, password, user, ref status) && status != UserLoginAttemptStatus.Success)
            {
                return status;
            }

            if (string.IsNullOrWhiteSpace(password) || user.Password != password.Salt(user.PasswordSalt))
            {
                OnInvalidPassword(user);
                return UserLoginAttemptStatus.BadPassword;
            }

            OnValidPassword(user);

            if (!CredentialPolicy.PasswordMeetsRequirement(password))
            {
                return UserLoginAttemptStatus.SuccessRequiresPasswordChange;
            }

            return UserLoginAttemptStatus.Success;
        }

        public virtual void OnInvalidPassword(TEntity user)
        {
            // Increase the failed login count 
        }

        public virtual void OnValidPassword(TEntity user)
        {
            // Reset failed login count to 0 
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not a user with the
        /// given email address exists.
        /// </summary>
        public bool UserExists(string email)
        {
            return GetUser(email) != null;
        }

        /// <summary>
        /// Returns a typed user object representing the record with the given
        /// email address if one exists, else null. Note that this evades
        /// security restrictions, and so should only be used in places where
        /// a user isn't logged in.
        /// </summary>
        public virtual TEntity GetUser(string email)
        {
            // TODO: This needs to be fixed. It's too specific in using
            //       email(instead of a generic username property that could
            //       potentially be an email address but might not be)

            AuthenticationServiceBase.ThrowIfInvalidEmail(email);
            email = email.SanitizeAndDowncase();
            var result = base.Criteria.Add(Restrictions.Eq("Email", email));
            return result.UniqueResult<TEntity>();
        }

        /// <summary>
        /// Returns a boolean value indicating if the given password question
        /// answer is valid for the user with the given email address. Returns
        /// false if no user record is found for the given email address.
        /// </summary>
        public bool ValidatePasswordQuestionAnswer(string email, string answer)
        {
            var user = GetUser(email);

            return user != null &&
                   (user.PasswordAnswer == answer
                                          .SanitizeAndDowncase()
                                          .Salt(user.PasswordSalt));
        }

        #endregion
    }

    /// <summary>
    /// NHibernate repository capable of authenticating users, but nothing else.
    /// </summary>
    public interface IAuthenticationRepository<TEntity> : IRepository<TEntity>
        where TEntity : IAdministratedUser
    {
        #region Methods

        TEntity Find(int id);

        /// <summary>
        /// Returns a UserLoginAttemptStatus value indicating whether the given
        /// email address and password are valid, and if not, why.
        /// </summary>
        UserLoginAttemptStatus ValidateUser(string email, string password);

        /// <summary>
        /// Returns a boolean value indicating whether or not a user with the
        /// given email address exists.
        /// </summary>
        bool UserExists(string email);

        /// <summary>
        /// Returns a typed user object representing the record with the given
        /// email address if one exists, else null. Note that this evades
        /// security restrictions, and so should only be used in places where
        /// a user isn't logged in.
        /// </summary>
        TEntity GetUser(string email);

        #endregion
    }

    /// <summary>
    /// NHibernate repository capable of resetting user passwords.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IForgotPasswordRepository<TEntity> : IRepository<TEntity>
        where TEntity : IRetrievablePasswordUser
    {
        #region Methods

        /// <summary>
        /// Returns a typed user object representing the record with the given
        /// email address if one exists, else null. Note that this evades
        /// security restrictions, and so should only be used in places where
        /// a user isn't logged in.
        /// </summary>
        TEntity GetUser(string email);

        /// <summary>
        /// Returns a boolean value indicating if the given password question
        /// answer is valid for the user with the given email address. Returns
        /// false if no user record is found for the given email address.
        /// </summary>
        bool ValidatePasswordQuestionAnswer(string email, string answer);

        #endregion
    }
}
