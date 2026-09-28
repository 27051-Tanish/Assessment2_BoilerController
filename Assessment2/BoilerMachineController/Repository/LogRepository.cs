namespace BoilerMachineController.Repository
{
    /// <summary>
    /// Provides methods for reading and writing into a file.
    /// </summary>
    public class LogRepository
    {
        private readonly string _logFilePath = Path.Combine(AppContext.BaseDirectory, "Logs.txt");
        public LogRepository()
        {
            CreateFile();
        }

        /// <summary>
        /// Creates the logger file if not exists and writes the header.
        /// </summary>
        public void CreateFile()
        {
            if (File.Exists(_logFilePath))
            {
                return;
            }

            File.WriteAllText(_logFilePath, "TimeStamp,Event,Event Data" + Environment.NewLine);
        }

        /// <summary>
        /// Writes the errors and events to the file with timestamp.
        /// </summary>
        /// <param name="message"></param>
        public void WriteToFile(string message)
        {
            string timeStampedMessage = $"{DateTime.Now:dd:MM:yyyy HH:mm:ss}, {message}";
            File.AppendAllText(_logFilePath, timeStampedMessage + Environment.NewLine);
        }

        /// <summary>
        /// Reads from the file.
        /// </summary>
        /// <returns></returns>
        public List<string> ReadFromFile()
        {
            List<string> messages = new List<string>();
            string[] lines = File.ReadAllLines(_logFilePath);
            //Console.WriteLine(_logFilePath); for testing purpose.
            foreach (string line in lines)
            {
                string[] data = line.Split(",");
                if (data.Length <= 1)
                {
                    continue;
                }
                string updatedMessage = $"[{data[0]}]: {data[1]}";
                messages.Add(updatedMessage);
            }

            return messages;
        }
    }
}
