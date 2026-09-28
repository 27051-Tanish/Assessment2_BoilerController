namespace BoilerMachineController.Repository
{
    public class LogRepository
    {
        private readonly string _logFilePath = Path.Combine(AppContext.BaseDirectory, "Logs.txt");
        public LogRepository()
        {
            CreateFile();
        }

        public void CreateFile()
        {
            if (File.Exists(_logFilePath))
            {
                return;
            }

            File.WriteAllText(_logFilePath, "TimeStamp,Event,Event Data" + Environment.NewLine);
        }

        public void WriteToFile(string message)
        {
            string timeStampedMessage = $"{DateTime.Now:dd:MM:yyyy HH:mm:ss}, {message}";
            File.AppendAllText(_logFilePath, timeStampedMessage + Environment.NewLine);
        }

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
