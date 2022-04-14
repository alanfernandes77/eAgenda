using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using eAgenda.ConsoleApp.Entities;
using eAgenda.ConsoleApp.Enums;
using eAgenda.ConsoleApp.Repositories;
using eAgenda.ConsoleApp.Utils;

namespace eAgenda.ConsoleApp.Views
{
    internal class TaskView : GenericView
    {
        private readonly TaskRepository _taskRepository;

        public TaskView(TaskRepository taskRepository) : base("Gerenciador de Tarefas")
        {
            _taskRepository = taskRepository;
        }

        public override void ShowOptions()
        {
            base.ShowOptions();

            Console.WriteLine("5 -> Visualizar Tarefas Concluídas");
            Console.WriteLine("6 -> Visualizar Tarefas Pendentes");
            Console.WriteLine();
            Console.WriteLine("7 -> Adicionar Item");
            Console.WriteLine("8 -> Remover Item");
            Console.WriteLine("9 -> Atualizar Item");
            Console.WriteLine();
            Console.WriteLine("0 -> Voltar");
            Console.WriteLine();

            Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

            SelectOption();
        }

        private Task GetTask(int option)
        {
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――");
            Console.WriteLine();
            Console.WriteLine("Tarefas disponíveis:");
            Console.WriteLine();

            switch (option)
            {
                case 1:
                    _taskRepository.GetPendingTasks().ForEach(tasks => Console.WriteLine($"  ({tasks.id}) - {tasks.Title}"));
                    break;

                case 2:
                    _taskRepository.GetCompletedTasks().ForEach(tasks => Console.WriteLine($"  ({tasks.id}) - {tasks.Title}"));
                    break;

                case 3:
                    _taskRepository.GetAll().ForEach(tasks => Console.WriteLine($"  ({tasks.id}) - {tasks.Title}"));
                    break;
            }
            Console.WriteLine();
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――");

            Console.WriteLine();

            Console.Write("Insira o identificador da tarefa: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Task task = _taskRepository.GetById(x => x.id == id);

            return task;
        }

        private void RegisterTask()
        {
            ShowTitle("Criando nova tarefa");

            Console.Write("Insira o título da tarefa: ");
            string title = Console.ReadLine();

            Console.WriteLine();

            Console.WriteLine("Selecione a prioridade da tarefa:");
            Console.WriteLine();
            Console.WriteLine("1 -> Baixa");
            Console.WriteLine("2 -> Normal");
            Console.WriteLine("3 -> Alta");
            Console.WriteLine();

            Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    _taskRepository.Insert(new Task(title, TaskPriority.Baixa));
                    break;

                case 2:
                    _taskRepository.Insert(new Task(title, TaskPriority.Normal));
                    break;

                case 3:
                    _taskRepository.Insert(new Task(title, TaskPriority.Alta));
                    break;
            }

            Messenger.Send("Tarefa registrada com sucesso!", MessageLevel.Sucesso, false);
            Messenger.Send("Para adicionar itens nesta tarefa, volte ao menú anterior e selecione a opção 'Adicionar Itens'.", MessageLevel.Sucesso, false);

            Console.ReadLine();
        }

        private void EditTask()
        {
            ShowTitle("Editando Tarefa");

            if (_taskRepository.GetPendingTasks().Count == 0)
            {
                Messenger.Send("Nenhuma tarefa disponível para ser editada.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Task task = GetTask(1);

            if (task == null)
            {
                Messenger.Send("Tarefa não encontrada.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }
            else
            {
                Messenger.Send("Edições possíveis: (Titulo, Prioridade)", MessageLevel.Informacao, true);

                Console.WriteLine("Deseja prosseguir com a edição?");
                Console.WriteLine();
                Console.WriteLine("1 -> Sim");
                Console.WriteLine("2 -> Não (Voltar ao menú principal)");
                Console.WriteLine();

                Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.WriteLine();

                        Console.Write("Insira o novo título da tarefa: ");
                        string newTitle = Console.ReadLine();
                        task.Title = newTitle;

                        Console.WriteLine();
                        Console.WriteLine("Selecione a nova prioridade da tarefa:");
                        Console.WriteLine();
                        Console.WriteLine("1 -> Baixa");
                        Console.WriteLine("2 -> Normal");
                        Console.WriteLine("3 -> Alta");
                        Console.WriteLine();

                        Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

                        int option2 = Convert.ToInt32(Console.ReadLine());

                        switch (option2)
                        {
                            case 1:
                                if (task.Priority == TaskPriority.Baixa)
                                {
                                    Messenger.Send("A prioridade atual desta tarefa já é BAIXA.", MessageLevel.Erro, false);
                                    Console.ReadLine();
                                    return;
                                }
                                else
                                {
                                    task.Priority = TaskPriority.Baixa;
                                }
                                break;

                            case 2:
                                if (task.Priority == TaskPriority.Normal)
                                {
                                    Messenger.Send("A prioridade atual desta tarefa já é Normal.", MessageLevel.Erro, false);
                                    Console.ReadLine();
                                    return;
                                }
                                else
                                {
                                    task.Priority = TaskPriority.Normal;
                                }
                                break;

                            case 3:
                                if (task.Priority == TaskPriority.Alta)
                                {
                                    Messenger.Send("A prioridade atual desta tarefa já é ALTA.", MessageLevel.Erro, false);
                                    Console.ReadLine();
                                    return;
                                }
                                else
                                {
                                    task.Priority = TaskPriority.Alta;
                                }
                                break;
                        }

                        Messenger.Send($"Tarefa editada com sucesso!", MessageLevel.Sucesso, true);

                        Console.ReadLine();

                        break;

                    case 2:
                        break;

                    default:
                        Messenger.Send("Opção não encontrada.", MessageLevel.Erro, true);
                        break;
                }
            }
        }

        private void DeleteTask()
        {
            ShowTitle("Deletando Tarefa");

            if (_taskRepository.GetAll().Count == 0)
            {
                Messenger.Send("Nenhuma tarefa disponível para ser deletada.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Task task = GetTask(3);

            if (task == null)
            {
                Messenger.Send("Tarefa não encontrada.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }
            else
            {
                _taskRepository.Delete(task);

                Messenger.Send($"Tarefa: '({task.id}) - {task.Title}', deletada com sucesso!", MessageLevel.Sucesso, true);

                Console.ReadLine();
            }
        }

        private void ListAllTasks()
        {
            ShowTitle("Visualizando Tarefas");

            if (_taskRepository.GetAll().Count == 0)
            {
                Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            _taskRepository.GetAll().Sort();
            _taskRepository.GetAll().ForEach(tasks => Console.WriteLine(tasks));

            Console.ReadLine();
        }

        private void ListPendingTasks()
        {
            ShowTitle("Visualizando Tarefas Pendentes");

            if (_taskRepository.GetPendingTasks().Count == 0)
            {
                Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            _taskRepository.GetPendingTasks().Sort();
            _taskRepository.GetPendingTasks().ForEach(tasks => Console.WriteLine(tasks));

            Console.ReadLine();
        }

        private void ListCompletedTasks()
        {
            ShowTitle("Visualizando Tarefas Concluídas");

            if (_taskRepository.GetCompletedTasks().Count == 0)
            {
                Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            _taskRepository.GetCompletedTasks().Sort();
            _taskRepository.GetCompletedTasks().ForEach(tasks => Console.WriteLine(tasks));

            Console.ReadLine();
        }

        private void AddItem()
        {
            ShowTitle("Adicionando Item a uma Tarefa");

            if (_taskRepository.GetPendingTasks().Count == 0)
            {
                Messenger.Send("Nenhum registro de tarefa encontrado para adicionar item.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Task task = GetTask(1);

            if (task == null)
            {
                Messenger.Send("Tarefa não encontrada.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }
            else
            {
                Messenger.Send($"-> Tarefa selecionada: '({task.id}) - {task.Title}'", MessageLevel.Informacao, true);

                Console.Write("Quantos itens deseja adicionar? ");
                int itens = Convert.ToInt32(Console.ReadLine());

                for (int i = 1; i <= itens; i++)
                {
                    Console.WriteLine();

                    Messenger.SendCustom($"Item ({i})", ConsoleColor.DarkCyan, true);

                    Console.WriteLine();

                    Console.Write("Insira a descrição do item: ");
                    string description = Console.ReadLine();

                    task.GetItems().Add(new Item(description));

                    Messenger.Send("Item adicionado com sucesso!", MessageLevel.Sucesso, false);
                }

                Messenger.Send($"* Itens adicionado(s) com sucesso na tarefa '({task.id}) - {task.Title}'", MessageLevel.Sucesso, true);

                Console.ReadLine();
            }
        }

        private void RemoveItem()
        {
            ShowTitle("Removendo Item de uma Tarefa");

            if (_taskRepository.GetPendingTasks().Count == 0)
            {
                Messenger.Send("Nenhum registro de tarefa encontrado para remover item.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Task task = GetTask(1);

            if (task == null)
            {
                Messenger.Send("Tarefa não encontrada.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }
            else
            {
                if (task.GetItems().Count == 0)
                {
                    Messenger.Send("Nenhum registro de item foi encontrado para essa tarefa.", MessageLevel.Erro, true);
                    Console.ReadLine();
                    return;
                }

                Messenger.Send($"-> Tarefa selecionada: '({task.id}) - {task.Title}'", MessageLevel.Informacao, true);

                Console.WriteLine("Itens disponíveis:");
                Console.WriteLine();

                task.GetItems().ForEach(items => Console.WriteLine($"{task.GetItems().IndexOf(items)} -> {items.Description}"));

                Console.WriteLine();
                Console.Write("Qual item deseja remover? ");
                int itens = Convert.ToInt32(Console.ReadLine());

                Item item = task.GetItems().ElementAtOrDefault(itens);

                if (item == null)
                {
                    Messenger.Send("Item não encontrado.", MessageLevel.Erro, true);
                    Console.ReadLine();
                    return;
                }
                else
                {
                    task.GetItems().Remove(item);

                    Messenger.Send($"Item removido com sucesso da '({task.id}) - {task.Title}'!", MessageLevel.Sucesso, true);

                    Console.ReadLine();
                }
            }
        }

        private void UpdateItem()
        {
            ShowTitle("Atualizando Item de uma Tarefa");

            if (_taskRepository.GetPendingTasks().Count == 0)
            {
                Messenger.Send("Nenhum registro de tarefa encontrado para atualizar.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Task task = GetTask(1);

            if (task == null)
            {
                Messenger.Send("Tarefa não encontrada.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }
            else
            {
                if (task.GetItems().Count == 0)
                {
                    Messenger.Send("Nenhum registro de item foi encontrado para essa tarefa.", MessageLevel.Erro, true);
                    Console.ReadLine();
                    return;
                }

                Console.WriteLine();

                Messenger.Send($"-> Tarefa selecionada: '({task.id}) - {task.Title}'", MessageLevel.Informacao, true);

                Console.WriteLine("Itens disponíveis:");
                Console.WriteLine();

                List<Item> pendingItems = task.GetItems().Where(x => x.Status == ItemStatus.Pendente).ToList();

                if (pendingItems.Count == 0)
                {
                    Messenger.Send("Nenhum item pendente encontrado para ser atualizado.", MessageLevel.Erro, true);
                    Console.ReadLine();
                    return;
                }

                pendingItems.ForEach(items => Console.WriteLine($"  {pendingItems.IndexOf(items)} -> {items.Description}"));

                Console.WriteLine();
                Console.Write("Qual item deseja atualizar? ");
                int itens = Convert.ToInt32(Console.ReadLine());

                Item item = pendingItems.ElementAtOrDefault(itens);

                if (item == null)
                {
                    Messenger.Send("Item não encontrado.", MessageLevel.Erro, true);
                    Console.ReadLine();
                    return;
                }
                else
                {
                    Messenger.Send("Deseja realmente marcar este item como concluído?", MessageLevel.Informacao, true);

                    Console.WriteLine("1 -> Sim");
                    Console.WriteLine("2 -> Não (Voltar ao menú principal)");
                    Console.WriteLine();

                    Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

                    int option = Convert.ToInt32(Console.ReadLine());

                    switch (option)
                    {
                        case 1:
                            item.Status = ItemStatus.Completado;

                            Messenger.Send($"Item marcado como completado com sucesso!", MessageLevel.Sucesso, true);

                            Console.ReadLine();
                            break;

                        case 2:
                            break;

                        default:
                            Messenger.Send("Opção não encontrada.", MessageLevel.Erro, true);
                            break;
                    }
                }
            }
        }

        private void SelectOption()
        {
            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    RegisterTask();
                    break;

                case 2:
                    EditTask();
                    break;

                case 3:
                    DeleteTask();
                    break;

                case 4:
                    ListAllTasks();
                    break;

                case 5:
                    ListCompletedTasks();
                    break;

                case 6:
                    ListPendingTasks();
                    break;

                case 7:
                    AddItem();
                    break;

                case 8:
                    RemoveItem();
                    break;

                case 9:
                    UpdateItem();
                    break;

                case 0:
                    break;

                default:
                    Messenger.Send("Opção não encontrada.", MessageLevel.Erro, true);
                    Console.ReadLine();
                    break;
            }
        }
    }
}