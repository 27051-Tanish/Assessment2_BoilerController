namespace BoilerMachineController.View
{
    /// <summary>
    /// Provides various methods for writing, reading input in the UI.
    /// </summary>
    public class ConsoleView
    {
        /// <summary>
        /// Writes the message to the UI.
        /// </summary>
        /// <param name="message">The message to be written.</param>
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        /// <summary>
        /// Reads user input from the UI.
        /// </summary>
        /// <returns>The read input value from the user.</returns>
        public string? ReadInput()
        {
            return Console.ReadLine();
        }

        /// <summary>
        /// Prompts user to enter a number, and if number is not valid display the error message else return the value.
        /// </summary>
        /// <param name="prompt">The prompt message.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <returns></returns>
        public int GetChoice(string prompt, string errorMessage)
        {
            this.ShowMessage(prompt);
            while (true)
            {
                if (int.TryParse(ReadInput(), out int result))
                {
                    return result;
                }

                this.ShowMessage(errorMessage);
            }
        }
    }
}
