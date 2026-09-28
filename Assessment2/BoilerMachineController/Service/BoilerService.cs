using System.Diagnostics;
using BoilerMachineController.Model;
using BoilerMachineController.Model.Core.Enums;
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
        private event Action<string> _EventHandler;

        /// <summary>
        /// Initializes the repository instance.
        /// </summary>
        /// <param name="logger">The instance of the logger repository.</param>
        public BoilerService(LogRepository logger)
        {
            this._logger = logger;
        }

        public async Task StartBoilingAsync(BoilerMachineInfo boiler)
        {    
            if (boiler.IsRunning)
            {
                _EventHandler.Invoke("Already boiler is running.");
                return;
            }
            
            if (boiler.SystemStatus == SystemStatus.Lockout)
            {
                _EventHandler.Invoke("[Error]: System is in lockout state.\nPlease reset to perform start.");
                return;
            }

            if (boiler.IsError)
            {
                _EventHandler.Invoke("[Error]: System is facing some unknown error.\nPlease reset to perform start.");
                return;
            }

            if (boiler.SwitchStatus == InterlockSwitchStatus.Open)
            {
                _EventHandler.Invoke("[Warn]: The interlock switch is open. Please toggle it to 'close' to continue operation.");
                return;
            }

            Stopwatch watch = Stopwatch.StartNew();
            boiler.MachineStatus = MachineStatus.PrePurge;
            await Task.Delay(10000);
            watch.Stop();

            _EventHandler.Invoke($"Pre-Purge is completed in {watch.ElapsedMilliseconds} ms.");

            watch.Restart();
            boiler.MachineStatus = MachineStatus.Ignition;
            await Task.Delay(10000);
            watch.Stop();

            _EventHandler.Invoke($"Ignition is completed in {watch.ElapsedMilliseconds} ms.");

            boiler.MachineStatus = MachineStatus.Operational;
            _EventHandler.Invoke($"[{DateTime.Now}] The machine is currently operational.");
        }

        public void StopBoiling(BoilerMachineInfo boiler)
        {
            if (boiler.IsRunning && !(boiler.MachineStatus == MachineStatus.Operational))
            {
                boiler.IsRunning = false;
                _EventHandler.Invoke($"[Warn]: The boiling operation is stopped before completion.");
            }
            else if(boiler.IsRunning && boiler.MachineStatus == MachineStatus.Operational)
            {
                boiler.IsRunning = false;
                _EventHandler.Invoke($"[Info]: The boiling operation is stopped and completed.");
            }
        }

        public void SimulateBoilerError(BoilerMachineInfo boiler)
        {
            if (!boiler.IsError)
            {
                boiler.IsError = true;
                boiler.IsRunning = false;
                boiler.SystemStatus = SystemStatus.Lockout;
                _EventHandler.Invoke("[Error]: Unknown error occurred.\nPlease reset the lockout state and close the interlock switch to continue.");          
            }
        }

        public void ToggleSwitch(BoilerMachineInfo boiler)
        {
            if (boiler.SwitchStatus == InterlockSwitchStatus.Open)
            {
                boiler.SwitchStatus = InterlockSwitchStatus.Close;
                _EventHandler.Invoke("[Info]: The switch toggled from 'open' to 'close'");
            }
            else
            {
                boiler.SwitchStatus = InterlockSwitchStatus.Open;
                _EventHandler.Invoke("[Info]: The switch toggled from 'close' to 'open'");
            }
        }

        public void ResetLockoutState(BoilerMachineInfo boiler)
        {
            if (boiler.IsError && boiler.SystemStatus == SystemStatus.Lockout)
            {
                boiler.IsError = false;
                _EventHandler.Invoke("[Info]: The unknown error is resolved.");
            }

            if (boiler.SwitchStatus == InterlockSwitchStatus.Open)
            {
                _EventHandler.Invoke("Please change the interlock switch to 'close' state.");
            }
        }

        public IReadOnlyList<string> ReadLogDetails()
        {
            return _logger.ReadFromFile();
        }
    }
}
