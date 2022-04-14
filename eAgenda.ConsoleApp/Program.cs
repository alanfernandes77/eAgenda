using eAgenda.ConsoleApp.Views;

namespace eAgenda.ConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            MainView mainView = new();

            mainView.ShowOptions();
        }
    }
}