using System;
using Glasses.Model;

namespace Glasses.IRepository
{
	public interface iUnitOfWorks : IDisposable
	{
		IGenericRepository<Product> Products { get; }

		Task Save();
	}
}

