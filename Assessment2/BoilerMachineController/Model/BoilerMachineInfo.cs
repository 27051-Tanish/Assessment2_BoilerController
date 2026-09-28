using BoilerMachineController.Model.Core.Enums;

namespace BoilerMachineController.Model
{
    /// <summary>
    /// Provides the required properties of the machine.
    /// </summary>
    public class BoilerMachineInfo
    {
        public BoilerMachineInfo(bool isRunning, bool isError, InterlockSwitchStatus switchStatus, MachineStatus machineStatus, SystemStatus systemStatus)
        {
            this.IsRunning = isRunning;
            this.IsError = isError;
            this.SwitchStatus = switchStatus;
            this.MachineStatus = machineStatus;
            this.SystemStatus = systemStatus;
        }

        /// <summary>
        /// Gets or sets whether the machine is running or not.
        /// </summary>
        bool IsRunning { get; set; }

        /// <summary>
        /// Gets or sets whether the machine encounters an error or not.
        /// </summary>
        bool IsError { get; set; }

        /// <summary>
        /// Gets or sets the status of the interlock switch.
        /// </summary>
        InterlockSwitchStatus SwitchStatus { get; set; }

        /// <summary>
        /// Gets or sets the status of the machine.
        /// </summary>
        MachineStatus MachineStatus { get; set; }

        /// <summary>
        /// Gets or sets the status of the system.
        /// </summary>
        SystemStatus SystemStatus { get; set; }

    }
}
