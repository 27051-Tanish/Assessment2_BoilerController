namespace BoilerMachineController.Repository
{
    public class LogRepository
    {
        private readonly string _logFilePath;
        private readonly object _lock = new object();
        public LogRepository()
        {
            _logFilePath = "Logs.txt";
        }

        public void CreateFile()
        {
            if (File.Exists(_logFilePath))
            {
                return;
            }

            File.WriteAllText(_logFilePath, "TimeStamp,Event,Event Data");
        }

        public void WriteToFile(string message)
        {
            string timeStampedMessage = $"{DateTime.Now:dd:MM:yyyy HH:mm:ss}, {message}";

            lock(_lock)
            {
                File.AppendAllText(_logFilePath, timeStampedMessage + Environment.NewLine);
            }
        }

        public List<string> ReadFromFile()
        {
            List<string> messages = new List<string>();
            string[] lines = File.ReadAllLines(_logFilePath);
            foreach (string line in lines)
            {
                string[] data = line.Split(",");
                if (data.Length < 2)
                {
                    continue;
                }
                messages.Add(data[0] + data[1]);
            }

            return messages;
        }
    }
}
