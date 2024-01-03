using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using MMSINC.ClassExtensions.IEnumerableExtensions;

namespace MMSINC.Utilities
{
    public class LapStopwatch
    {
        #region Constants

        public struct Lap
        {
            #region Properties

            public string Name { get; }
            public long ElapsedMiliseconds { get; }

            #endregion

            #region Constructors

            public Lap(string name, long elapsedMiliseconds)
            {
                Name = name;
                ElapsedMiliseconds = elapsedMiliseconds;
            }

            #endregion
        }

        #endregion

        #region Private Members

        private readonly Stopwatch _stopwatch;
        private readonly IList<Lap> _laps;
        private string _currentLap;

        #endregion

        #region Properties

        public Lap[] Laps => _laps.ToArray();

        /// <summary>
        /// All laps plus a summary called "total" with all times summed.
        /// </summary>
        public Lap[] Summary =>
            _laps.MergeWith(new[] {new Lap("total", _laps.Sum(l => l.ElapsedMiliseconds))}).ToArray();

        #endregion

        #region Constructors

        public LapStopwatch()
        {
            _stopwatch = new Stopwatch();
            _laps = new List<Lap>();
        }

        #endregion

        #region Exposed Methods

        public void Start(string lapName = null)
        {
            _stopwatch.Start();
            _currentLap = lapName ?? "Lap 1";
        }

        public void NextLap(string lapName = null)
        {
            Stop();
            _currentLap = lapName ?? $"Lap {_laps.Count + 1}";
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        public void Stop()
        {
            if (!_stopwatch.IsRunning)
            {
                throw new InvalidOperationException(
                    "Stopwatch must be started before any other actions can be performed.");
            }

            _laps.Add(new Lap(_currentLap, _stopwatch.ElapsedMilliseconds));
            _stopwatch.Stop();
        }

        #endregion
    }
}
