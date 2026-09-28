using System.Diagnostics;
using BoilerMachineController.Model;
using BoilerMachineController.Model.Core.Enums;
using BoilerMachineController.Repository;

namespace BoilerMachineController.Service
{
    /// <summary>
    /// Provides various methods for performing different actions and log their result in the file and notify using an event.
    /// </summary>
    public class BoilerService
    {
        private readonly ILogRepository _logger;
        public event Action<string> _EventHandler;

        /// <summary>
        /// Initializes the repository instance.
        /// </summary>
        /// <param name="logger">The instance of the logger repository.</param>
        public BoilerService(ILogRepository logger)
        {
            this._logger = logger;
        }

        /// <summary>
        /// Starts the boiler sequence asynchronously.
        /// </summary>
        /// <param name="boiler">The boiler which has to be started.</param>
        /// <returns>The task in asynchronous manner.</returns>
        public async Task StartBoilingAsync(BoilerMachineInfo boiler, CancellationToken cts)
        {    
            if (boiler.IsRunning)
            {
                _logger.WriteToFile("[Warn]: Already boiler is running.");
                _EventHandler.Invoke("[Warn]: Already boiler is running.");
                return;
            }
            
            if (boiler.SystemStatus == SystemStatus.Lockout)
            {
                _logger.WriteToFile("[Error]: System is in lockout state.\nPlease reset to perform start.");
                _EventHandler.Invoke("[Error]: System is in lockout state.\nPlease reset to perform start.");
                return;
            }

            if (boiler.IsError)
            {
                _logger.WriteToFile("[Error]: System is facing some unknown error.\nPlease reset to perform start.");
                _EventHandler.Invoke("[Error]: System is facing some unknown error.\nPlease reset to perform start.");
                return;
            }

            if (boiler.SwitchStatus == InterlockSwitchStatus.Open)
            {
                _logger.WriteToFile("[Warn]: The interlock switch is open. Please toggle it to 'close' to continue operation.");
                _EventHandler.Invoke("[Warn]: The interlock switch is open. Please toggle it to 'close' to continue operation.");
                return;
            }
            if (cts.IsCancellationRequested)
            {
                boiler.IsRunning = false;
                boiler.MachineStatus = MachineStatus.Idle;
                boiler.SystemStatus = SystemStatus.Lockout;
                _logger.WriteToFile("[Info]: Stopped the operation before completion.");
                _EventHandler.Invoke("[Info]: Stopped the operation before completion.");
                throw new TaskCanceledException();
            }

            Stopwatch watch = Stopwatch.StartNew();
            boiler.IsRunning = true;
            boiler.MachineStatus = MachineStatus.PrePurge;
            await Task.Delay(10000);
            watch.Stop();

            _logger.WriteToFile($"Pre-Purge is completed in {watch.ElapsedMilliseconds} ms.");
            _EventHandler.Invoke($"[{DateTime.Now}] Pre-Purge is completed in {watch.ElapsedMilliseconds} ms.");

            watch.Restart();
            boiler.MachineStatus = MachineStatus.Ignition;
            await Task.Delay(10000);
            watch.Stop();

            _logger.WriteToFile($"Ignition is completed in {watch.ElapsedMilliseconds} ms.");
            _EventHandler.Invoke($"[{DateTime.Now}] Ignition is completed in {watch.ElapsedMilliseconds} ms.");

            boiler.MachineStatus = MachineStatus.Operational;
            _logger.WriteToFile($"The machine is currently operational.");
            _EventHandler.Invoke($"[{DateTime.Now}] The machine is currently operational.");
        }

        /// <summary>
        /// Stops the boiling sequence between any phases.
        /// </summary>
        /// <param name="boiler">The boiler which has to be stopped</param>
        public void StopBoiling(BoilerMachineInfo boiler, CancellationToken cts)
        {
            if (boiler.IsRunning && !(boiler.MachineStatus == MachineStatus.Operational))
            {
                boiler.IsRunning = false;
                boiler.MachineStatus = MachineStatus.Idle;
                _logger.WriteToFile($"[Warn]: The boiling operation is stopped before completion.");
                _EventHandler.Invoke($"[Warn]: The boiling operation is stopped before completion.");
                cts.ThrowIfCancellationRequested(); // Throws if cancellation requested, on clicking option 2 from the menu when boiler sequence is not in operational state.
            }
            else if(boiler.IsRunning && boiler.MachineStatus == MachineStatus.Operational)
            {
                boiler.IsRunning = false;
                boiler.MachineStatus = MachineStatus.Idle;
                _logger.WriteToFile($"[Info]: The boiling operation is stopped and completed.");
                _EventHandler.Invoke($"[Info]: The boiling operation is stopped and completed.");
            }
        }

        /// <summary>
        /// Simulate an error to stop the boiling sequence.
        /// </summary>
        /// <param name="boiler">The boiler in which the error needs to be simulated.</param>
        public void SimulateBoilerError(BoilerMachineInfo boiler)
        {
            if (!boiler.IsRunning)
            {
                _logger.WriteToFile("[Error]: The boiler is not running cannot simulate error.");
                _EventHandler.Invoke("[Error]: The boiler is not running cannot simulate error.");
            }
            else if (!boiler.IsError)
            {
                boiler.IsError = true;
                boiler.IsRunning = false;
                boiler.SystemStatus = SystemStatus.Lockout;
                _logger.WriteToFile("[Error]: Unknown error occurred.\nPlease reset the lockout state and close the interlock switch to continue.");
                _EventHandler.Invoke("[Error]: Unknown error occurred.\nPlease reset the lockout state and close the interlock switch to continue.");          
            }
        }

        /// <summary>
        /// Toggle the interlock switch between open and close.
        /// </summary>
        /// <param name="boiler">The boiler in which their switch needs to be toggled.</param>
        public void ToggleSwitch(BoilerMachineInfo boiler)
        {
            if (boiler.SwitchStatus == InterlockSwitchStatus.Open)
            {
                boiler.SwitchStatus = InterlockSwitchStatus.Close;
                _logger.WriteToFile("[Info]: The switch toggled from 'open' to 'close'");
                _EventHandler.Invoke("[Info]: The switch toggled from 'open' to 'close'");
            }
            else
            {
                boiler.SwitchStatus = InterlockSwitchStatus.Open;
                _logger.WriteToFile("[Info]: The switch toggled from 'close' to 'open'");
                _EventHandler.Invoke("[Info]: The switch toggled from 'close' to 'open'");
            }
        }

        /// <summary>
        /// Resets the systems state.
        /// </summary>
        /// <param name="boiler">The boiler which state needs to be reset.</param>
        public void ResetLockoutState(BoilerMachineInfo boiler)
        {
            if (boiler.IsError || boiler.SystemStatus == SystemStatus.Lockout)
            {
                boiler.IsError = false;
                boiler.SystemStatus = SystemStatus.Ready;
                _logger.WriteToFile("[Info]: The lockout state is changed.");
                _EventHandler.Invoke("[Info]: The lockout state is changed.");
            }

            if (boiler.SwitchStatus == InterlockSwitchStatus.Open)
            {
                _logger.WriteToFile("[Info]: Please change the interlock switch to 'close' state to start the boiler.");
                _EventHandler.Invoke("[Info]: Please change the interlock switch to 'close' state to start the boiler.");
            }
        }

        /// <summary>
        /// Reads the logged details.
        /// </summary>
        /// <returns>The list of logged information.</returns>
        public IReadOnlyList<string> ReadLogDetails()
        {
            return _logger.ReadFromFile();
        }
    }
}
