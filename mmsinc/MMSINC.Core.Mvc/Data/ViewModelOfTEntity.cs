using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Script.Serialization;
using MMSINC.ClassExtensions.ObjectExtensions;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MMSINC.Data
{
    // This exists solely for some reflection junk
    public abstract class ViewModel { }

    /// <remarks>
    /// Hey, you, are you inheriting from this class? If so, you need to include one parameterless
    /// constructor so MVC can create an object. You can't make one constructor with an optional
    /// parameter. It doesn't work.
    /// -Ross 11/11/11 (Palindrome day!)
    /// </remarks>
    public abstract class ViewModel<TEntity> : ViewModel, IValidatableObject where TEntity : class
    {
        #region Fields

        // Use the property accessor.
        private IObjectMapper _mapper;
        protected readonly IContainer _container;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the original entity used to create this ViewModel, if one was given. 
        /// </summary>
        [DoesNotAutoMap]
        protected virtual TEntity Original { get; set; }

        [ScriptIgnore] // I dunno if this ScriptIgnore is even needed anymore?
        internal IObjectMapper Mapper
        {
            get => _mapper ?? (_mapper = CreateObjectMapper());
            set => _mapper = value;
        }

        [AutoMap(MapDirections.ToViewModel)]
        public virtual int Id { get; set; }

        #endregion

        #region Constructor

        [DefaultConstructor]
        public ViewModel(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Private Methods

        private IObjectMapper CreateObjectMapper()
        {
            return new AutoObjectMapper(_container, GetType(), typeof(TEntity));
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Maps the given entity to this view model instance.
        /// </summary>
        /// <notes>This is typically called prior to rendering "Edit" views.</notes>
        /// <param name="entity">The entity serving as the source of the map process.</param>
        public virtual void Map(TEntity entity)
        {
            entity.ThrowIfNull("entity");
            Original = entity;
            Mapper.MapToPrimary(this, entity);
        }

        /// <summary>
        /// Maps this view model instance to the given entity. 
        /// </summary>
        /// <notes>
        /// Ultimately where the view model (instance) loaded with data from the user gets mapped
        /// onto an instance of the entity which has been previously loaded fresh from the database
        /// OR to a new TEntity(). Typically you want to call base when you're overriding, unless
        /// you know there's default mappings you don't want to be applied.</notes>
        /// <param name="entity">The entity serving as the target of the map process.</param>
        /// <returns>An entity which has been mapped to from this view model instance.</returns>
        public virtual TEntity MapToEntity(TEntity entity)
        {
            entity.ThrowIfNull("entity");
            Mapper.MapToSecondary(this, entity);
            return entity;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            if (Original != null)
            {
                var orig = Original.ToString();
                if (orig != Original.GetType().FullName)
                {
                    return orig;
                }
            }

            return base.ToString();
        }

        /// <summary>
        /// Use this method to set your default values such as dates. It will automatically 
        /// be called by ActionHelper DoNew. Note that SetDefaults is meant to be called
        /// prior to the call to Map! If a default value needs to be set after Map then
        /// you should be setting it from Map.
        /// </summary>
        public virtual void SetDefaults()
        {
            // noop goes the dynamite!
        }

        #endregion
    }
}
