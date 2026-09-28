namespace BoilerMachineController.Model.Core.Enums
{
    /// <summary>
    /// Represents the different status of the system.
    /// </summary>
    public enum SystemStatus
    {
        /// <summary>
        /// Represents the lockout state where the system cannot start the boiler.
        /// Requires reset of the state to start the boiler.
        /// </summary>
        Lockout,

        /// <summary>
        /// Represents the ready state where system can start the boiler.
        /// </summary>
        Ready,
    }
}
