using BoilerMachineController.Controller;
using BoilerMachineController.Repository;
using BoilerMachineController.Service;
using BoilerMachineController.View;

namespace BoilerMachineController
{
    public class Program
    {
        static void Main(string[] args)
        {
            LogRepository logRepository = new LogRepository();
            BoilerService boilerService = new BoilerService(logRepository);
            ConsoleView consoleView = new ConsoleView();

            BoilerController controller = new BoilerController(boilerService, consoleView);
            controller.RunApplication();
        }
    }
}
