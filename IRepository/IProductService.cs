using System;
using Glasses.Model;
using Glasses.Repository;

namespace Glasses.IRepository
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();

        Task<Product> GetSingleProduct(int id);

        Task<Product> CreatePRoduct(CreateProductDTO ProductDTO);

        Task<Product> DeleteProduct(int id);

        Task<Product> EditProduct(int id, UpdateProductDTO ProductDTO);
    }
}

