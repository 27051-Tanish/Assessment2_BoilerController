namespace BoilerMachineController.Repository
{
    public interface ILogRepository
    {
        /// <summary>
        /// Creates the logger file if not exists and writes the header.
        /// </summary>
        public void CreateFile();

        /// <summary>
        /// Writes the errors and events to the file with timestamp.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public void WriteToFile(string message);

        /// <summary>
        /// Reads from the file.
        /// </summary>
        /// <returns>The log details as list.</returns>
        public List<string> ReadFromFile();
    }
}
