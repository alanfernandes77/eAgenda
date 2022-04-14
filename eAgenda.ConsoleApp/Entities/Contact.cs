using System;
using System.Text;

namespace eAgenda.ConsoleApp.Entities
{
    internal class Contact : IComparable<Contact>
    {
        public int id;
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string JobPosition { get; set; }

        public Contact(string name, string email, string phone, string company, string jobPosition)
        {
            id = new Random().Next(10000, 100000);
            Name = name;
            Email = email;
            Phone = phone;
            Company = company;
            JobPosition = jobPosition;
        }

        public int CompareTo(Contact other)
        {
            return JobPosition.CompareTo(other.JobPosition);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine($"Contato: '{id}'");
            sb.AppendLine();
            sb.AppendLine($"  Nome: {Name}");
            sb.AppendLine($"  E-mail: {Email}");
            sb.AppendLine($"  Telefone: {Phone}");
            sb.AppendLine($"  Empresa: {Company}");
            sb.AppendLine($"  Cargo: {JobPosition}");

            return sb.ToString();
        }
    }
}