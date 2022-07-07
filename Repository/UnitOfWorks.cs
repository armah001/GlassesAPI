using System;
using Glasses.Data;
using Glasses.IRepository;
using Glasses.Model;

namespace Glasses.Repository
{
	public class UnitOfWorks:iUnitOfWorks

	{
        private readonly GlassesContext _context;
        private IGenericRepository<Product> _products;
        private readonly ILogger<UnitOfWorks> _logger;

        public UnitOfWorks(GlassesContext context, ILogger<UnitOfWorks> logger)
        {
    
            _context = context;
            _logger = logger;
        }

        public IGenericRepository<Product> Products =>_products??=new GenericRepository<Product>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
        
        public async Task Save()
        {
            //try
            //{
                await _context.SaveChangesAsync();
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Something went wrong in the{nameof(Save)}");
                
            //}
        }
    }
}

