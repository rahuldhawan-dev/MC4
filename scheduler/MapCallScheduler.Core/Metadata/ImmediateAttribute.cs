using System;

namespace MapCallScheduler.Metadata
{
    /// <summary>
    /// Specifies that a job should be started immediately, rather than waiting
    /// for the configured start time.
    /// </summary>
    public class ImmediateAttribute : Attribute {}
}