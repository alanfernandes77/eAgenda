using System;
using System.Collections.Generic;
using eAgenda.ConsoleApp.Entities;
using eAgenda.ConsoleApp.Interfaces;

namespace eAgenda.ConsoleApp.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected List<T> _genericList;

        public GenericRepository()
        {
            _genericList = new List<T>();
        }

        public List<T> GetAll() => _genericList;

        public T GetById(Predicate<T> match)
        {
            return _genericList.Find(match);
        }

        public void Insert(T entity)
        {
            _genericList.Add(entity);
        }

        public void Delete(T entity)
        {
            _genericList.Remove(entity);
        }
    }
}