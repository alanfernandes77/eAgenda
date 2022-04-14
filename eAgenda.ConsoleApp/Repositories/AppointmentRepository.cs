using eAgenda.ConsoleApp.Entities;
using System.Collections.Generic;
using System.Linq;

namespace eAgenda.ConsoleApp.Repositories
{
    internal class AppointmentRepository : GenericRepository<Appointment>
    {
        public List<Appointment> GetPassedAppointments() => GetAll().Where(x => x.IsPassed()).ToList();

        public List<Appointment> GetFutureApppointments() => GetAll().Where(x => !x.IsPassed()).ToList();

    }
}