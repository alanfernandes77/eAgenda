using System;
using System.Text;

namespace eAgenda.ConsoleApp.Entities
{
    internal class Appointment : IComparable<Appointment>
    {
        public int id;

        public bool withContact;
        public string Subject { get; set; }
        public string Local { get; set; }
        public DateTime AppointmentDate { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Contact Contact { get; set; }

        public Appointment(string subject, string local, DateTime appointmentDate, DateTime startTime, DateTime endTime)
        {
            id = new Random().Next(10000, 100000);
            Subject = subject;
            Local = local;
            AppointmentDate = appointmentDate;
            StartTime = startTime;
            EndTime = endTime;
            withContact = false;
        }

        public Appointment(string subject, string local, DateTime appointmentDate, DateTime startTime, DateTime endTime, Contact contact) : this (subject, local, appointmentDate, startTime, endTime)   
        {
            Contact = contact;
            withContact = true;
        }

        public bool IsPassed()
        {
            if (DateTime.Now > AppointmentDate)
                return true;

            return false;
        }

        public int CompareTo(Appointment other)
        {
            return AppointmentDate.CompareTo(other.AppointmentDate);
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine($"Compromisso: '{id}'");
            sb.AppendLine();
            sb.AppendLine($"  Assunto: {Subject}");

            if (withContact)
                sb.AppendLine($"  Com o contato: {Contact.Name}");

            sb.AppendLine($"  Local: {Local}");
            sb.AppendLine($"  Data: {AppointmentDate:dd/MM/yyyy}");
            sb.AppendLine($"  Início: {StartTime:HH:mm}");
            sb.AppendLine($"  Término: {EndTime:HH:mm}");

            return sb.ToString();
        }
    }
}