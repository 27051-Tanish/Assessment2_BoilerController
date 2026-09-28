using BoilerMachineController.Repository;

namespace BoilerMachineController.Service
{
    /// <summary>
    /// Provides various business logics for performing different sequences.
    /// </summary>
    public class BoilerService
    {
        private readonly LogRepository _logger;
        private readonly object _locker = new object();
        private System.Timers.Timer _timer;

        /// <summary>
        /// Initializes the repository instance.
        /// </summary>
        /// <param name="logger">The instance of the logger repository.</param>
        public BoilerService(LogRepository logger)
        {
            this._logger = logger;
        }
    }
}
