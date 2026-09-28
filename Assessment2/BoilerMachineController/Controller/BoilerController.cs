using BoilerMachineController.Model;
using BoilerMachineController.Model.Core.Enums;
using BoilerMachineController.Service;
using BoilerMachineController.View;

namespace BoilerMachineController.Controller
{
    /// <summary>
    /// Acts as the orchestrator between view and service.
    /// </summary>
    public class BoilerController
    {
        private readonly BoilerMachineInfo _boiler;
        private readonly BoilerService _boilerService;
        private readonly ConsoleView _view;
        CancellationTokenSource source;
        CancellationToken token;

        public BoilerController(BoilerService boilerService, ConsoleView view)
        {
            this._boilerService = boilerService;
            this._view = view;
            source = new CancellationTokenSource();
            token = source.Token;
            _boiler = new BoilerMachineInfo(false, false, InterlockSwitchStatus.Open, MachineStatus.Idle, SystemStatus.Lockout);
            _boilerService._EventHandler += DisplayNotification;
        }

        /// <summary>
        /// Displays the message which is passed by the event.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        private void DisplayNotification(string message)
        {
            this._view.ShowMessage(message);
        }

        /// <summary>
        /// Starts the main menu.
        /// </summary>
        public void RunApplication()
        {           
            int choice;
            MainMenu menu;
            this._view.ShowMessage("==== Boiler Controller Initialized ====");

            do
            {
                this._view.ShowTitle("MAIN MENU");
                this._view.ShowMessage("\n[1]. Start boiling\n[2]. Stop boiling\n[3]. Simulate Error\n[4]. Toggle switch\n" +
                    "[5]. Reset lockout\n[6]. View log\n[7]. Exit\n");
                choice = this._view.GetChoice("Enter your choice: ", "Please select from the menu [1 to 7].");
                menu = (MainMenu)choice;

                switch (menu)
                {
                    case MainMenu.StartBoiler:
                        this.StartBoilerSequence();
                        break;
                    case MainMenu.StopBoiler:
                        this.StopBoilerSequence();
                        break;
                    case MainMenu.SimulateError:
                        this.SimulateError();
                        break;
                    case MainMenu.ToggleSwitch:
                        this.ToggleInterLockSwitch();
                        break;
                    case MainMenu.ResetLockout:
                        this.ResetLockout();
                        break;
                    case MainMenu.ViewLog:
                        this.ViewLog();
                        break;
                    case MainMenu.Exit:
                        break;
                    default:
                        this._view.ShowMessage("Please select from the menu [1 to 7].");
                        break;
                }
            }
            while (menu != MainMenu.Exit);
        }

        /// <summary>
        /// Starts the boiler sequence asynchronously.
        /// </summary>
        private void StartBoilerSequence()
        {
            try
            {
                _ = _boilerService.StartBoilingAsync(_boiler, token);
            }
            catch (TaskCanceledException ex)
            {
                this._view.ShowMessage($"{ex.Message}");
            }
        }

        /// <summary>
        /// Stops the boiler sequence.
        /// </summary>
        private void StopBoilerSequence()
        {
            if (_boiler.MachineStatus == MachineStatus.Idle)
            {
                this._view.ShowMessage("The boiler is currently not started.");
                return;
            }
            _boilerService.StopBoiling(_boiler, token);
        }

        /// <summary>
        /// Simulate an error and stop the boiler sequence.
        /// </summary>
        private void SimulateError()
        {
            _boilerService.SimulateBoilerError(_boiler);
        }

        /// <summary>
        /// Toggle the interlock switch between 'open' and 'close'.
        /// </summary>
        private void ToggleInterLockSwitch()
        {
            _boilerService.ToggleSwitch(_boiler);
        }

        /// <summary>
        /// Resets the error state and provide proper prompt to start the boiler.
        /// </summary>
        private void ResetLockout()
        {
            _boilerService.ResetLockoutState(_boiler);
        }

        /// <summary>
        /// Displays the events and errors logged in a separate file.
        /// </summary>
        private void ViewLog()
        {
            IReadOnlyList<string> logs = _boilerService.ReadLogDetails();
            foreach (var log in logs)
            {
                this._view.ShowMessage($"{log}");
            }
        }
    }
}
