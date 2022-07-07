using System;
using System.Linq.Expressions;
using System.Collections.Generic;
namespace Glasses.IRepository
{
	public interface IGenericRepository<T> where T: class
	{
		Task<IList<T>> GetAll(
            Expression<Func<T, bool>> expression = null,
			Func<IQueryable<T>,
				IOrderedQueryable<T>> orderBy =null,
			List<String>includes=null
			);

		Task<T> Get(
			Expression<Func<T, bool>> expression,
			List<String> includes = null
			);

		Task Insert(T entity);

		Task InsertRange(IEnumerable<T> entities); // for bulk operations 

		Task Delete(int id);

		void DeleteRange(IEnumerable<T> entities);

		void Update(T entity);
		

	}
}


