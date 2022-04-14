using eAgenda.ConsoleApp.Enums;

namespace eAgenda.ConsoleApp.Entities
{
    internal class Item
    {
        public string Description { get; set; }
        public ItemStatus Status { get; set; }

        public Item(string description)
        {
            Description = description;
            Status = ItemStatus.Pendente;
        }

        public override string ToString() => $"{Description} -> Status: ({Status})";
    }
}