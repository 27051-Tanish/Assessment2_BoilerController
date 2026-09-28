namespace BoilerMachineController.Model.Core.Enums
{
    /// <summary>
    /// Represents the status of the switch.
    /// </summary>
    public enum InterlockSwitchStatus
    {
        /// <summary>
        /// Represents the open state, where the system cannot be in the ready state.
        /// </summary>
        Open,

        /// <summary>
        /// Represents the close state, where the system can be in the ready state and can start the boiling.
        /// </summary>
        Close,
    }
}
