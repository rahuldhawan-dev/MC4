using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;

namespace MMSINC.Validation
{
    /// <summary>
    /// Validates that an entity exists for a given key when the key is passed
    /// to a repository.
    /// </summary>
    /// <remarks>
    /// 
    /// NOTE: This works specifically with using ObjectFactory.
    /// 
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EntityMustExistAttribute : ValidationAttribute, ICustomModelMetadataAttribute
    {
        #region Constants

        public const string ADDITIONAL_VALUES_KEY = "EntityMustExistAttribute",
                            DEFAULT_ERROR_MESSAGE_FORMAT = "{0}'s value does not match an existing object.";

        #endregion

        #region Fields

        private static readonly Type _entityMustExistType = typeof(EntityMustExistAttribute);

        #endregion

        #region Properties

        /// <summary>
        /// Gets the repository type that's registered with ObjectFactory.
        /// </summary>
        public Type RepositoryType { get; private set; }

        public Type EntityType { get; private set; }

        #endregion

        #region Constructors

        #region Constructor

        public EntityMustExistAttribute(Type entityType)
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType.ThrowIfNull(nameof(entityType))))
            {
                throw new ArgumentException(
                    $"{nameof(entityType)} must implement {nameof(IEntity)}.  {entityType.Name} does not.",
                    nameof(entityType));
            }

            EntityType = entityType;
            RepositoryType = _emptyIRepositoryType.MakeGenericType(EntityType);
            ErrorMessage = DEFAULT_ERROR_MESSAGE_FORMAT;
        }

        #endregion

        #endregion

        #region Fields

        private static readonly Type _IBaseRepositoryType = typeof(IBaseRepository),
                                     _emptyIRepositoryType = typeof(IRepository<>);

        #endregion

        #region Public Methods

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var result = ValidationResult.Success;

            // Return Success when the value is null. Use RequiredAttribute if the
            // entity must exist and can't be null.
            if (value == null)
            {
                return result;
            }

            int[] keys;

            if (value is string)
            {
                keys = new[] {Convert.ToInt32(value)};
            }
            else if (value is int[])
            {
                keys = (int[])value;
            }
            else
            {
                keys = new[] {(int)value};
            }

            var repo = (IBaseRepository)validationContext.GetService(RepositoryType);

            if (repo == null)
            {
                throw new ArgumentException($"Unable to resolve repository type '{RepositoryType.FullName}'");
            }

            if (!keys.All(id => repo.Exists(id)))
            {
                var memberNames = validationContext.MemberName != null
                    ? new[] {validationContext.MemberName}
                    : null;
                result = new ValidationResult(FormatErrorMessage(validationContext.DisplayName), memberNames);
            }

            return result;
        }

        public override bool IsValid(object value)
        {
            throw new InvalidOperationException("Use GetValidationResult instead.");
        }

        public void Process(ModelMetadata modelMetaData)
        {
            modelMetaData.AdditionalValues.Add(ADDITIONAL_VALUES_KEY, this);
        }

        /// <summary>
        /// Returns the EntityMustExistAttribute for the given ModelMetadata. Returns null if an attribute
        /// does not exist.
        ///  </summary>
        public static EntityMustExistAttribute GetAttributeForModel(ModelMetadata meta)
        {
            meta.ThrowIfNull("metadata");

            if (meta.AdditionalValues.TryGetValue(ADDITIONAL_VALUES_KEY, out var attr))
            {
                return (EntityMustExistAttribute)attr;
            }

            return null;
        }

        #endregion
    }
}
