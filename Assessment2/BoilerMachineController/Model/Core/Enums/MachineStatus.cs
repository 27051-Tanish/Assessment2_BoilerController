namespace BoilerMachineController.Model.Core.Enums
{
    /// <summary>
    /// Represents the status of the boiler machine.
    /// </summary>
    public enum MachineStatus
    {
        /// <summary>
        /// Represents the first phase of the boiling process.
        /// </summary>
        PrePurge,

        /// <summary>
        /// Represents the second phase of the boiling process.
        /// </summary>
        Ignition,

        /// <summary>
        /// Represents the final phase of the boiling process.
        /// </summary>
        Operational,
    }
}
