using System;

namespace MMSINC.Bundling
{
    /// <summary>
    /// Bundle class for bundling the javascripts.
    /// </summary>
    public class BetterScriptBundle : ContentBundle
    {
        #region Constructor

        public BetterScriptBundle(string virtualPath, string cdnPath = null)
            : base(virtualPath, cdnPath)
        {
            // This is exactly the same thing ScriptBundle does in its constructor. 
            // Mimicing it for compatibility.
            ConcatenationToken = ";" + Environment.NewLine;
        }

        #endregion
    }
}
