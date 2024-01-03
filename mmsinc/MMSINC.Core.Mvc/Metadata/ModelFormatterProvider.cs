using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MMSINC.ClassExtensions.ObjectExtensions;

namespace MMSINC.Metadata
{
    public enum FormatMode
    {
        Display,
        Editor
    }

    public class ModelFormatterProvider
    {
        #region Fields

        private readonly Dictionary<Type, ModelFormatterAttribute> _formattersByType;
        private readonly Dictionary<Type, ModelFormatterAttribute> _editorFormattersByType;

        #endregion

        #region Constructor

        public ModelFormatterProvider()
        {
            _formattersByType = new Dictionary<Type, ModelFormatterAttribute>();
            _editorFormattersByType = new Dictionary<Type, ModelFormatterAttribute>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes all registered formatters.
        /// </summary>
        public void Clear()
        {
            _formattersByType.Clear();
            _editorFormattersByType.Clear();
        }

        /// <summary>
        /// Registers a default formatter for a specific type. This will overwrite
        /// an existing formatter if one has been registered. This formatter is
        /// used by default for both display and editor templates.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="formatter"></param>
        public void RegisterFormatter(Type type, ModelFormatterAttribute formatter)
        {
            type.ThrowIfNull("type");
            formatter.ThrowIfNull("formatter");
            _formattersByType[type] = formatter;
        }

        /// <summary>
        /// Registers a default formatter for a specific type that's only used for
        /// rendering values in an editor template.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="formatter"></param>
        public void RegisterEditorFormatter(Type type, ModelFormatterAttribute formatter)
        {
            type.ThrowIfNull("type");
            formatter.ThrowIfNull("formatter");
            _editorFormattersByType[type] = formatter;
        }

        /// <summary>
        /// Returns the associated ModelFormatter for metadata. Returns null if no custom
        /// or default formatter is found.
        /// </summary>
        public ModelFormatterAttribute TryGetModelFormatter(ModelMetadata metadata, FormatMode formatMode)
        {
            metadata.ThrowIfNull("metadata");

            // We don't care about the FormatMode if there's an attribute on the model itself.
            var custom = ModelFormatterAttribute.TryGetAttributeFromModelMetadata(metadata);
            if (custom != null)
            {
                return custom;
            }

            ModelFormatterAttribute attr;

            if (formatMode == FormatMode.Editor)
            {
                _editorFormattersByType.TryGetValue(metadata.ModelType, out attr);

                if (attr != null)
                {
                    return attr;
                }
            }

            // Return the registered display formatter if an editor one can't be found.
            // That and return a display one if the mode is display.
            _formattersByType.TryGetValue(metadata.ModelType, out attr);
            return attr;
        }

        #endregion
    }
}
