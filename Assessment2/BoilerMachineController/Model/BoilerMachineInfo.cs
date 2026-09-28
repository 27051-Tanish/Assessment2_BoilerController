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
        public bool IsRunning { get; set; }

        /// <summary>
        /// Gets or sets whether the machine encounters an error or not.
        /// </summary>
        public bool IsError { get; set; }

        /// <summary>
        /// Gets or sets the starting time of the operation.
        /// </summary>
        public DateTime StartTime { get; set; }

        /// <summary>
        /// Gets or sets the status of the interlock switch.
        /// </summary>
        public InterlockSwitchStatus SwitchStatus { get; set; }

        /// <summary>
        /// Gets or sets the status of the machine.
        /// </summary>
        public MachineStatus MachineStatus { get; set; }

        /// <summary>
        /// Gets or sets the status of the system.
        /// </summary>
        public SystemStatus SystemStatus { get; set; }

    }
}
