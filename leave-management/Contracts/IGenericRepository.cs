﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace leave_management.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<ICollection<T>> FindAll(
            Expression<Func<T, bool>> expression = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, Object>> includes = null
        );

        Task<T> Find(
            Expression<Func<T, bool>> expression,
            Func<IQueryable<T>, IIncludableQueryable<T, Object>> includes = null
        );

        Task<bool> isExist(Expression<Func<T, bool>> expression = null);
        Task Create(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
