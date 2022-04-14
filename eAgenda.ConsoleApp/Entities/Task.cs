using eAgenda.ConsoleApp.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace eAgenda.ConsoleApp.Entities
{
    internal class Task : IComparable<Task>
    {
        public int id;
        public string Title { get; set; }
        public DateTime CreationDate { get; set; }
        public List<Item> Items { get; set; }
        public TaskPriority Priority { get; set; }

        public Task(string title, TaskPriority priority)
        {
            id = new Random().Next(10000, 100000);
            Title = title;
            CreationDate = DateTime.Now;
            Items = new List<Item>();
            Priority = priority;
            CompletedPercentage = 0.0;
            ConclusionDate = DateTime.MinValue;
        }

        public double CompletedPercentage 
        {
            get
            {
                List<Item> completedItens = Items.Where(x => x.Status == ItemStatus.Completado).ToList();

                return (double) completedItens.Count / Items.Count * 100.0;
            }

            set { }
        
        }

        public DateTime ConclusionDate
        {
            get
            {
                if (CompletedPercentage != 100.0)
                    return DateTime.MinValue;

                return DateTime.Now;
            }

            set { }
        }

        public List<Item> GetItems() => Items;

        public bool IsCompleted()
        {
            if (CompletedPercentage == 100.0)
                return true;

            return false;
        }

        public int CompareTo(Task other)
        {
            return Priority.CompareTo(other.Priority);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine($"Tarefa: '{id}'");
            sb.AppendLine();
            sb.AppendLine($" * Titulo: {Title}");
            sb.AppendLine($" * Data de Criação: {CreationDate:dd/MM/yyyy HH:mm}");
            sb.AppendLine(" * Itens:");

            if (Items.Count == 0)
                sb.AppendLine("    - Nenhum item registrado.");
            else
                foreach (Item items in Items)
                    sb.AppendLine($"    - {items}");

            sb.AppendLine($" * Prioridade: {Priority}");
            sb.AppendLine($" * Percentual Concluído: {CompletedPercentage.ToString("F2", CultureInfo.InvariantCulture)}%");

            if (CompletedPercentage != 100.0)
                sb.AppendLine($" * Data de Conclusão: ");
            else
                sb.AppendLine($" * Data de Conclusão: {ConclusionDate:dd/MM/yyyy HH:mm}");

            return sb.ToString();
        }
    }
}