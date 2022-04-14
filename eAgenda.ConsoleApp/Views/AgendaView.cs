using eAgenda.ConsoleApp.Entities;
using eAgenda.ConsoleApp.Enums;
using eAgenda.ConsoleApp.Repositories;
using eAgenda.ConsoleApp.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace eAgenda.ConsoleApp.Views
{
    internal class AgendaView : GenericView
    {
        private readonly ContactRepository _contactRepository;
        private readonly AppointmentRepository _appointmentRepository;

        public AgendaView(ContactRepository contactRepository, AppointmentRepository appointmentRepository) : base("Gerenciador de Contatos")
        {
            _contactRepository = contactRepository;
            _appointmentRepository = appointmentRepository;
        }

        public override void ShowOptions()
        {
            ShowTitle("Gerenciador de Contatos/Compromissos");

            Console.WriteLine("1 -> Adicionar Contato");
            Console.WriteLine("2 -> Editar Contato");
            Console.WriteLine("3 -> Deletar Contato");
            Console.WriteLine("4 -> Visualizar Contatos");
            Console.WriteLine();
            Console.WriteLine("5 -> Registrar Compromisso");
            Console.WriteLine("6 -> Editar Compromisso");
            Console.WriteLine("7 -> Deletar Compromisso");
            Console.WriteLine("8 -> Visualizar Compromissos Passados");
            Console.WriteLine("9 -> Visualizar Compromissos Futuros");
            Console.WriteLine();
            Console.WriteLine("0 -> Voltar");
            Console.WriteLine();

            Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

            SelectOption();
        }

        private Contact GetContact()
        {
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――");
            Console.WriteLine();
            Console.WriteLine("Contatos disponíveis:");
            Console.WriteLine();

            _contactRepository.GetAll().ForEach(contacts => Console.WriteLine($"  ({contacts.id}) - {contacts.Name}"));

            Console.WriteLine();
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――");

            Console.WriteLine();

            Console.Write("Insira o identificador do contato: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Contact contact = _contactRepository.GetById(x => x.id == id);

            return contact;
        }

        private void RegisterContact()
        {
            ShowTitle("Criando novo contato");

            Console.Write("Insira o nome do contato: ");
            string name = Console.ReadLine();

            Console.Write("Insira o e-mail do contato: ");
            string email = Console.ReadLine();

            if (string.IsNullOrEmpty(email))
            {
                Messenger.Send("O campo 'E-mail' não pode ser nulo ou vazio.", MessageLevel.Erro, false);
                Console.ReadLine();
                return;
            }

            Console.Write("Insira o telefone do contato: ");
            string phone = Console.ReadLine();

            if (string.IsNullOrEmpty(phone) && phone.Length != 11)
            {
                Messenger.Send("O campo 'Telefone' não pode ser nulo ou vazio e deve conter 11 carácteres.", MessageLevel.Erro, false);
                Console.ReadLine();
                return;
            }

            Console.Write("Insira a empresa do contato: ");
            string company = Console.ReadLine();

            Console.Write("Insira o cargo do contato: ");
            string jobPosition = Console.ReadLine();

            _contactRepository.Insert(new Contact(name, email, phone, company, jobPosition));

            Messenger.Send("Contato registrado com sucesso!", MessageLevel.Sucesso, false);

            Console.ReadLine();
        }

        private void EditContact()
        {
            ShowTitle("Editando Contato");

            if (_contactRepository.GetAll().Count == 0)
            {
                Messenger.Send("Nenhum contato disponível para ser editado.", MessageLevel.Erro, false);
                Console.ReadLine();
                return;
            }

            Contact contact = GetContact();

            if (contact == null)
            {
                Messenger.Send("Contato não encontrado.", MessageLevel.Erro, false);
                Console.ReadLine();
                return;
            }
            else
            {
                Messenger.Send("Edições possíveis: (Nome, E-mail, Telefone, Empresa e Cargo)", MessageLevel.Informacao, true);

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

                        Console.Write("Insira o novo nome do contato: ");
                        string name = Console.ReadLine();

                        Console.Write("Insira o novo e-mail do contato: ");
                        string email = Console.ReadLine();

                        if (string.IsNullOrEmpty(email))
                        {
                            Messenger.Send("O campo 'E-mail' não pode ser nulo ou vazio.", MessageLevel.Erro, false);
                            Console.ReadLine();
                            return;
                        }

                        Console.Write("Insira o novo telefone do contato: ");
                        string phone = Console.ReadLine();

                        if (string.IsNullOrEmpty(phone) && phone.Length != 11)
                        {
                            Messenger.Send("O campo 'Telefone' não pode ser nulo ou vazio e deve conter 11 carácteres.", MessageLevel.Erro, false);
                            Console.ReadLine();
                            return;
                        }

                        Console.Write("Insira a novo empresa do contato: ");
                        string company = Console.ReadLine();

                        Console.Write("Insira o novo cargo do contato: ");
                        string jobPosition = Console.ReadLine();

                        contact.Name = name;
                        contact.Email = email;
                        contact.Phone = phone;
                        contact.Company = company;
                        contact.JobPosition = jobPosition;

                        Messenger.Send($"Contato editado com sucesso!", MessageLevel.Sucesso, true);

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

        private void DeleteContact()
        {
            ShowTitle("Deletando Contato");

            if (_contactRepository.GetAll().Count == 0)
            {
                Messenger.Send("Nenhum contato disponível para ser deletado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Contact contact = GetContact();

            if (contact == null)
            {
                Messenger.Send("Contato não encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }
            else
            {
                _contactRepository.Delete(contact);

                Messenger.Send($"Contato: '({contact.id}) - {contact.Name}', deletado com sucesso!", MessageLevel.Sucesso, true);

                Console.ReadLine();
            }
        }

        private void ListAllContacts()
        {
            ShowTitle("Visualizando Contatos");

            if (_contactRepository.GetAll().Count == 0)
            {
                Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            _contactRepository.GetAll().Sort();
            _contactRepository.GetAll().ForEach(tasks => Console.WriteLine(tasks));

            Console.ReadLine();
        }

        private Appointment GetAppointment()
        {
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――");
            Console.WriteLine();
            Console.WriteLine("Compromissos disponíveis:");
            Console.WriteLine();

            _appointmentRepository.GetAll().ForEach(appointment => Console.WriteLine($"  ({appointment.id}) - {appointment.Subject}"));

            Console.WriteLine();
            Console.WriteLine("―――――――――――――――――――――――――――――――――――――――――――――――――");

            Console.WriteLine();

            Console.Write("Insira o identificador do compromisso: ");
            int id = Convert.ToInt32(Console.ReadLine());

            Appointment appointment = _appointmentRepository.GetById(x => x.id == id);

            return appointment;
        }

        private void RegisterAppointment()
        {
            ShowTitle("Criando novo compromisso");

            Console.Write("Insira o assunto do compromisso: ");
            string subject = Console.ReadLine();

            Console.Write("Insira o local do compromisso: ");
            string local = Console.ReadLine();

            Console.Write("Insira a data do compromisso (dd/MM/yyyy): ");
            DateTime appointmentDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (appointmentDate < DateTime.Now)
            {
                Messenger.Send("A data do compromisso deve ser uma data futura.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Console.Write("Insira o horário de início do compromisso (HH:mm): ");
            DateTime startTime = DateTime.ParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture);

            Console.Write("Insira o horário de término do compromisso (HH:mm): ");
            DateTime endTime = DateTime.ParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture);

            if (endTime < startTime)
            {
                Messenger.Send("O horário de término deve ser maior que o horário de início.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Console.WriteLine();
            Console.WriteLine("Este compromisso possui alguma relação com algum contato de sua agenda?");
            Console.WriteLine();
            Console.WriteLine("1 -> Sim");
            Console.WriteLine("2 -> Não");
            Console.WriteLine();
            Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    if (_contactRepository.GetAll().Count == 0)
                    {
                        Messenger.Send("Nenhum contato encontrado para completar o registro.", MessageLevel.Erro, true);
                        Console.ReadLine();
                        return;
                    }

                    Contact contact = GetContact();

                    if (contact == null)
                    {
                        Messenger.Send("Contato não encontrado.", MessageLevel.Erro, false);
                        Console.ReadLine();
                        return;
                    }
                    else
                    {
                        _appointmentRepository.Insert(new(subject, local, appointmentDate, startTime, endTime, contact));
                    }
                    break;

                case 2:
                    _appointmentRepository.Insert(new(subject, local, appointmentDate, startTime, endTime));
                    break;

                default:
                    Messenger.Send("Opção não encontrada.", MessageLevel.Erro, true);
                    break;
            }

            Messenger.Send("Compromisso registrado com sucesso!", MessageLevel.Sucesso, false);

            Console.ReadLine();
        }

        private void EditAppointment()
        {
            ShowTitle("Editando Compromisso");

            if (_appointmentRepository.GetAll().Count == 0)
            {
                Messenger.Send("Nenhum compromisso disponível para ser editado.", MessageLevel.Erro, false);
                Console.ReadLine();
                return;
            }

            Appointment appointment = GetAppointment();

            if (appointment == null)
            {
                Messenger.Send("Compromisso não encontrado.", MessageLevel.Erro, false);
                Console.ReadLine();
                return;
            }
            else
            {
                Messenger.Send("Edições possíveis: (Assunto, Local, Data do compromisso, Início, Término e Contato (se disponivel).)", MessageLevel.Informacao, true);

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

                        Console.Write("Insira o assunto do compromisso: ");
                        string newSubject = Console.ReadLine();

                        Console.Write("Insira o local do compromisso: ");
                        string newLocal = Console.ReadLine();

                        Console.Write("Insira a data do compromisso (dd/MM/yyyy): ");
                        DateTime newAppointmentDate = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (newAppointmentDate < DateTime.Now)
                        {
                            Messenger.Send("A nova data do compromisso deve ser uma data futura.", MessageLevel.Erro, true);
                            Console.ReadLine();
                            return;
                        }

                        Console.Write("Insira o horário de início do compromisso (HH:mm): ");
                        DateTime newStartTime = DateTime.ParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture);

                        Console.Write("Insira o horário de término do compromisso (HH:mm): ");
                        DateTime newEndTime = DateTime.ParseExact(Console.ReadLine(), "HH:mm", CultureInfo.InvariantCulture);

                        if (newEndTime < newStartTime)
                        {
                            Messenger.Send("O novo horário de término deve ser maior que o novo horário de início.", MessageLevel.Erro, true);
                            Console.ReadLine();
                            return;
                        }

                        appointment.Subject = newSubject;
                        appointment.Local = newLocal;
                        appointment.AppointmentDate = newAppointmentDate;
                        appointment.StartTime = newStartTime;
                        appointment.EndTime = newEndTime;

                        if (appointment.withContact == true)
                        {
                            Contact contact = GetContact();

                            if (contact == null)
                            {
                                Messenger.Send("Contato não encontrado.", MessageLevel.Erro, false);
                                Console.ReadLine();
                                return;
                            }
                            else
                            {
                                appointment.Contact = contact;
                            }
                        }

                        Messenger.Send($"Compromisso editado com sucesso!", MessageLevel.Sucesso, true);

                        Console.ReadLine();
                        break;

                    case 2:
                        break;

                    default:
                        Messenger.Send("Opção não encontrada.", MessageLevel.Erro, true);
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void DeleteAppointment()
        {
            ShowTitle("Deletando Compromisso");

            if (_appointmentRepository.GetAll().Count == 0)
            {
                Messenger.Send("Nenhum compromisso disponível para ser deletado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            Appointment appointment = GetAppointment();

            if (appointment == null)
            {
                Messenger.Send("Compromisso não encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }
            else
            {
                _appointmentRepository.Delete(appointment);

                Messenger.Send($"Compromisso: '({appointment.id}) - {appointment.Subject}', deletado com sucesso!", MessageLevel.Sucesso, true);

                Console.ReadLine();
            }
        }

        private void ListPassedAppointments()
        {
            ShowTitle("Visualizando Tarefas Pendentes");

            if (_appointmentRepository.GetPassedAppointments().Count == 0)
            {
                Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            _appointmentRepository.GetPassedAppointments().Sort();
            _appointmentRepository.GetPassedAppointments().ForEach(appointments => Console.WriteLine(appointments));

            Console.ReadLine();
        }

        private void ListFutureAppointments()
        {
            ShowTitle("Visualizando Compromissos Futuros");

            if (_appointmentRepository.GetFutureApppointments().Count == 0)
            {
                Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                Console.ReadLine();
                return;
            }

            _appointmentRepository.GetFutureApppointments().Sort();
            _appointmentRepository.GetFutureApppointments().ForEach(appointments => Console.WriteLine(appointments));

            bool run = true;

            while (run)
            {
                Console.WriteLine();
                Console.WriteLine("Deseja filtrar os compromissos?");
                Console.WriteLine();
                Console.WriteLine("1 -> Sim");
                Console.WriteLine("2 -> Não");
                Console.WriteLine();

                Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

                int option = Convert.ToInt32(Console.ReadLine());

                switch (option)
                {
                    case 1:
                        Console.WriteLine();

                        Console.WriteLine();
                        Console.WriteLine("Qual filtro deseja aplicar?");
                        Console.WriteLine();
                        Console.WriteLine("1 -> Por mês");
                        Console.WriteLine("2 -> Por período");
                        Console.WriteLine();

                        Messenger.SendCustom("Opção: ", ConsoleColor.DarkCyan, false);

                        option = Convert.ToInt32(Console.ReadLine());

                        switch (option)
                        {
                            case 1:
                                ShowTitle("Filtrando Compromisso (Mês)");

                                Console.Write("Insira o mês: ");
                                DateTime filter = DateTime.ParseExact(Console.ReadLine(), "MM", CultureInfo.InvariantCulture);

                                List<Appointment> filteredByMonthAppointments = _appointmentRepository.GetFutureApppointments().Where(x => x.AppointmentDate.Month == filter.Month).ToList();

                                if (filteredByMonthAppointments.Count == 0)
                                {
                                    Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                                    Console.ReadLine();
                                    return;
                                }

                                ShowTitle($"Visualizando Compromissos do Mês ({filter:MM})");

                                filteredByMonthAppointments.Sort();
                                filteredByMonthAppointments.ForEach(appointments => Console.WriteLine(appointments));

                                run = true;
                                break;

                            case 2:
                                ShowTitle("Filtrando Compromisso (Período)");

                                Console.WriteLine();
                                Console.Write("Insira a primeira data (dd/MM/yyyy): ");
                                DateTime date1 = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                Console.Write("Insira a segunda data (dd/MM/yyyy): ");
                                DateTime date2 = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                                List<Appointment> filteredByPeriodAppointments = _appointmentRepository.GetFutureApppointments().Where(x => (date1 <= x.AppointmentDate && x.AppointmentDate <= date2)).ToList();

                                if (filteredByPeriodAppointments.Count == 0)
                                {
                                    Messenger.Send("Nenhum registro encontrado.", MessageLevel.Erro, true);
                                    Console.ReadLine();
                                    return;
                                }

                                ShowTitle($"Visualizando Compromissos de ({date1:dd/MM/yyyy} até {date2:dd/MM/yyyy})");

                                filteredByPeriodAppointments.Sort();
                                filteredByPeriodAppointments.ForEach(appointments => Console.WriteLine(appointments));

                                run = true;
                                break;
                        }
                        break;

                    case 2:
                        Console.WriteLine();
                        Console.WriteLine("Opção de filtro cancelada.");
                        Console.WriteLine();
                        Console.ReadLine();
                        run = false;
                        break;

                    default:
                        Messenger.Send("Opção não encontrada.", MessageLevel.Erro, true);
                        Console.ReadLine();
                        break;
                }
            }
        }

        private void SelectOption()
        {
            int option = Convert.ToInt32(Console.ReadLine());

            switch (option)
            {
                case 1:
                    RegisterContact();
                    break;

                case 2:
                    EditContact();
                    break;

                case 3:
                    DeleteContact();
                    break;

                case 4:
                    ListAllContacts();
                    break;

                case 5:
                    RegisterAppointment();
                    break;

                case 6:
                    EditAppointment();
                    break;

                case 7:
                    DeleteAppointment();
                    break;

                case 8:
                    ListPassedAppointments();
                    break;

                case 9:
                    ListFutureAppointments();
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