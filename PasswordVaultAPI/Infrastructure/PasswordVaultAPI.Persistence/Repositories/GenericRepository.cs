﻿using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using PasswordVaultAPI.Application.Repositories;
using PasswordVaultAPI.Domain.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using PasswordVaultAPI.Persistence.Context;
using Microsoft.AspNetCore.Mvc;

namespace PasswordVaultAPI.Persistence.Repositories
{
	public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
	{

		private readonly MyContext _context;

		public GenericRepository(MyContext context)
		{
			_context = context;
		}



		public DbSet<T> Table => _context.Set<T>();



		// read repository

		public IQueryable<T> GetAll(bool tracking = true)
		{
			var query = Table.AsQueryable();
			if (!tracking)
				query = query.AsNoTracking();
			return query;
		}



		public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
		{
			var query = Table.AsQueryable();
			if (!tracking)
				query = query.AsNoTracking();
			return query;
		}



		public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
		{
			var query = Table.AsQueryable();
			if (!tracking)
				query = query.AsNoTracking();
			return await query.FirstOrDefaultAsync(method);
		}



		public async Task<T> GetByIdAsync(string id, bool tracking = true)
		{
			var query = Table.AsQueryable();
			if (!tracking)
				query = query.AsNoTracking();
			return await query.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));
		}




	
		// write repository

		public async Task<bool> AddAsync(T model)
		{
			EntityEntry<T> entityEntry = await Table.AddAsync(model);
			return entityEntry.State == EntityState.Added;
		}



		public async Task<bool> AddRangeAsync(List<T> datas)
		{
			await Table.AddRangeAsync(datas);
			return true;
		}



		public bool Remove(T model)
		{
			EntityEntry<T> entityEntry = Table.Remove(model);
			return entityEntry.State == EntityState.Deleted;
		}



		public bool RemoveRange(List<T> datas)
		{
			Table.RemoveRange(datas);
			return true;
		}



		public async Task<bool> RemoveAsync(string id)
		{
			var model = await GetByIdAsync(id);
			if (model == null) return false;

			Table.Remove(model);
			await _context.SaveChangesAsync();
			return true;
		}



		public async Task<bool> UpdateAsync(T model)
		{
			EntityEntry<T> entityEntry = Table.Update(model);
			return entityEntry.State == EntityState.Modified; ;
		}



		public async Task<int> SaveAsync()
			=> await _context.SaveChangesAsync();

	}
}