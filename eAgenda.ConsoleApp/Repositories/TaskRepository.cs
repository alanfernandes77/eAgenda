using System.Collections.Generic;
using System.Linq;
using eAgenda.ConsoleApp.Entities;

namespace eAgenda.ConsoleApp.Repositories
{
    internal class TaskRepository : GenericRepository<Task> 
    {
        public List<Task> GetPendingTasks() => GetAll().Where(x => !x.IsCompleted()).ToList();
        public List<Task> GetCompletedTasks() => GetAll().Where(x => x.IsCompleted()).ToList();
    }
}