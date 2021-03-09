﻿using Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository.Base
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected AppDbContext Context { get; set; }
        private DbSet<T> _entitySet => Context.Set<T>();

        public Repository(AppDbContext context)
        {
            Context = context;
        }

        public async Task<IEnumerable<T>> GetItemsList()
        {
            return await _entitySet.ToListAsync();
        }

        public async Task<T> GetItem(int id)
        {
            return await _entitySet.FindAsync(id);
        }

        public async Task CreateItem(T item)
        {
            await Context.AddAsync(item);
            await SaveAllAsync();
        }

        public async Task UpdateItem(T item)
        {
            Context.Entry(item).State = EntityState.Modified;
            await SaveAllAsync();
        }

        public async Task DeleteItem(T item)
        {
            _entitySet.Remove(item);
            await SaveAllAsync();
        }

        protected async Task SaveAllAsync()
        {
            await Context.SaveChangesAsync();
        }
    }
}
