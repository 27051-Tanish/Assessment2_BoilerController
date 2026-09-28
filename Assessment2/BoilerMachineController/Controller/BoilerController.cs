using BoilerMachineController.Model.Core.Enums;
using BoilerMachineController.Repository;
using BoilerMachineController.Service;
using BoilerMachineController.View;

namespace BoilerMachineController.Controller
{
    public class BoilerController
    {
        private readonly LogRepository _logRepository;
        private readonly BoilerService _boilerService;
        private readonly ConsoleView _view;
        public BoilerController(LogRepository logRepository, BoilerService boilerService, ConsoleView view)
        {
            this._logRepository = logRepository;
            this._boilerService = boilerService;
            this._view = view;
        }

        public void RunApplication()
        {
            int choice;
            MainMenu menu;

            do
            {
                this._view.ShowMessage("[1]. Start boiling\n[2]. Stop boiling\n[3]. Simulate Error\n[4]. Toggle switch\n" +
                    "[5]. Reset lockout\n[6]. View log\n[7]. Exit");
                choice = this._view.GetChoice("Enter your choice: ", "Please select from the menu [1 to 7].");
                menu = (MainMenu)choice;

                switch (menu)
                {
                    case MainMenu.StartBoiler:
                        break;
                    case MainMenu.StopBoiler:
                        break;
                    case MainMenu.SimulateError:
                        break;
                    case MainMenu.ToggleSwitch:
                        break;
                    case MainMenu.ResetLockout:
                        break;
                    case MainMenu.ViewLog:
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
    }
}
