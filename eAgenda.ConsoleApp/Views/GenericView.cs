using System;
using eAgenda.ConsoleApp.Utils;

namespace eAgenda.ConsoleApp.Views
{
    internal class GenericView
    {
        protected string Title { get; set; }

        public GenericView(string title)
        {
            Title = title;
        }

        public virtual void ShowOptions()
        {
            ShowTitle(Title);

            Console.WriteLine("1 -> Cadastrar");
            Console.WriteLine("2 -> Editar");
            Console.WriteLine("3 -> Excluir");
            Console.WriteLine("4 -> Visualizar");
        }

        public void ShowTitle(string title)
        {
            Console.Clear();
            Messenger.SendCustom(title, ConsoleColor.Magenta, true);
            Console.WriteLine();
        }
    }
}