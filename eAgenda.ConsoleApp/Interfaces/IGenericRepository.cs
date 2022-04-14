using System;
using System.Collections.Generic;

namespace eAgenda.ConsoleApp.Interfaces
{
    internal interface IGenericRepository<T> where T : class
    {
        List<T> GetAll();
        T GetById(Predicate<T> match);
        void Insert(T entity);
        void Delete(T entity);
    }
}