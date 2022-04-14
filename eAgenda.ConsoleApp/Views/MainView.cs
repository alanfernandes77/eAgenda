using System;
using eAgenda.ConsoleApp.Enums;
using eAgenda.ConsoleApp.Repositories;
using eAgenda.ConsoleApp.Utils;

namespace eAgenda.ConsoleApp.Views
{
    internal class MainView : GenericView
    {
        private readonly TaskView _taskView;
        private readonly AgendaView _contactView;

        public MainView() : base("eAgenda - v1.0")
        {
            _taskView = new(new TaskRepository());
            _contactView = new(new ContactRepository(), new AppointmentRepository());
        }

        public override void ShowOptions()
        {
            while (true)
            {
                ShowTitle(Title);

                Console.WriteLine("1 -> Módulo Tarefas");
                Console.WriteLine("2 -> Módulo Agenda/Compromissos");
                Console.WriteLine();
                Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

                SelectOption();
            }
        }

        private void SelectOption()
        {

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    _taskView.ShowOptions();
                    break;

                case 2:
                    _contactView.ShowOptions();
                    break;

                default:
                    Messenger.Send("Opção não encontrada.", MessageLevel.Erro, true);
                    Console.ReadLine();
                    break;
            }
        }
    }
}