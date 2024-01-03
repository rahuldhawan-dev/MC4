using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace MMSINC.Testing.SeleniumMvc
{
    public enum JQueryEventType
    {
        // These are all named as the actual jQuery events, hence
        // "DblClick" and not "DoubleClick".
        Blur,
        Change,
        Click,
        DblClick,
        Focus,
        Keydown,
        Keypress,
        Keyup,
        Mousedown,
        Mousemove,
        Mouseover,
        Mouseup
    }

    public enum JQueryMouseEventButton
    {
        // Actual values are important here.
        Left = 1
    }

    public enum JQueryKeyCodes
    {
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Delete = 46
    }

    public class JQueryEvent
    {
        #region Fields

        private IBetterWebDriver _driver;

        #endregion

        #region Properties

        // Event
        public bool Bubbles { get; set; }
        public IBetterWebElement Target { get; set; } // This may need to be an IWebElement? I dunno
        public JQueryEventType Type { get; set; }

        // Mouse
        public JQueryMouseEventButton MouseButton { get; set; }
        public int ClientX { get; set; }
        public int ClientY { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        public int PageX { get; set; }
        public int PageY { get; set; }

        // Keyboard
        public JQueryKeyCodes KeyCode { get; set; }
        public bool IsCtrlKeyPressed { get; set; }
        public bool IsShiftKeyPressed { get; set; }

        #endregion

        #region Constructors

        public JQueryEvent(IBetterWebDriver driver)
        {
            _driver = driver;
            // Setting this by default cause various jQuery plugins
            // check for this, and we're almost always going to be left-clicking.
            MouseButton = JQueryMouseEventButton.Left;
        }

        #endregion

        #region Private methods

        protected virtual Dictionary<string, object> CreateEventArgsDictionary()
        {
            // PageX and PageY are the jQuery standardized versions
            // of ClientX/ClientY. Might be able to just ditch the ClientX/ClientY 
            // stuff, but who knows.
            var args = new Dictionary<string, object> {
                {"bubbles", Bubbles},
                {"clientX", ClientX},
                {"clientY", ClientY},
                {"offsetX", OffsetX},
                {"offsetY", OffsetY},
                {"pageX", PageX},
                {"pageY", PageY},
                {"type", Type.ToString().ToLower()},

                // "button" is needed for jQuery UI's stupid use of event.button
                // instead of event.which when IE8 or lower is used. 
                {"button", (int)MouseButton}, // Need to serialize to int, not the enum name
                {"which", (int)MouseButton}, // Need to serialize to int, not the enum name
                {"keyCode", (int)KeyCode}, // Need to serialize to int, not the enum name
                {"ctrlKey", IsCtrlKeyPressed},
                {"shiftKey", IsShiftKeyPressed}
            };

            return args;
        }

        protected virtual string Serialize()
        {
            return JsonConvert.SerializeObject(CreateEventArgsDictionary());
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Fires an event on a given element. A unique identifier is given to the element so 
        /// jQuery can find it(doesn't overwrite the id attribute).
        /// </summary>
        public virtual void Fire(IBetterWebElement el)
        {
            // This performs faster than converting to a css selector and 
            // calling the other Fire overload.
            var sb = new StringBuilder();
            sb.AppendFormat("var args = {0};", Serialize());
            var hasTarget = Target != null;
            if (hasTarget)
            {
                sb.Append("args.target = arguments[1];");
            }

            sb.Append("jQuery(arguments[0]).trigger(args);");
            if (hasTarget)
            {
                _driver.ExecuteScript(sb.ToString(), el.InternalElement, Target.InternalElement);
            }
            else
            {
                _driver.ExecuteScript(sb.ToString(), el.InternalElement);
            }
        }

        /// <summary>
        /// Use this to fire an event on $(document).
        /// </summary>
        public virtual void FireOnDocument()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("var args = {0};", Serialize());
            var hasTarget = Target != null;
            if (hasTarget)
            {
                sb.Append("args.target = arguments[1];");
            }

            sb.AppendFormat("jQuery(document).trigger(args);");

            if (hasTarget)
            {
                _driver.ExecuteScript(sb.ToString(), Target.InternalElement);
            }
            else
            {
                _driver.ExecuteScript(sb.ToString());
            }
        }

        #endregion
    }
}
