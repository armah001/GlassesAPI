using System;
using AutoMapper;
using Glasses.Data;
using Glasses.IRepository;
using Glasses.Model;
using Glasses.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Glasses
{
    public class ProductService: IProductService
    {
        private readonly GlassesContext context;
        private readonly iUnitOfWorks _unitOfWork;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        

        public ProductService(iUnitOfWorks unitOfWork, ILogger<ProductService> logger,
            GlassesContext context, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            this.context = context;
            _mapper = mapper;
            
        }




        public async Task<List<Product>> GetAllProducts()
        {
            
            var products = await _unitOfWork.Products.GetAll();
        
            return products.ToList();
        }

        public async Task<Product> GetSingleProduct(int id)
        {
            var product = await _unitOfWork.Products.Get(q => q.ProductId == id);
            return product;
        }

        public async Task<Product>CreatePRoduct(CreateProductDTO ProductDTO)
        {
            var product = _mapper.Map<Product>(ProductDTO);
            await _unitOfWork.Products.Insert(product);
            await _unitOfWork.Save();
            return  product;
        }

        public async Task<Product> DeleteProduct(int id)
        {
            var product = await _unitOfWork.Products.Get(q => q.ProductId == id);
            if (product == null)
            {
                _logger.LogError($"Invalid Delete Attempt in {nameof(DeleteProduct)}");
                //return BadRequest("Inavlid Data Submitted");
            }
            await _unitOfWork.Products.Delete(id);
            await _unitOfWork.Save();
            return product;
        }

        public async Task<Product> EditProduct(int id, [FromBody] UpdateProductDTO ProductDTO)
        {
            var product = await _unitOfWork.Products.Get(q => q.ProductId == id);

            if (product == null)
            {
                _logger.LogError($"Invalid Update Attempt in {nameof(EditProduct)}");
                //return BadRequest("Cannot process submitted data");
            }
            _mapper.Map(ProductDTO, product);
            _unitOfWork.Products.Update(product);
            await _unitOfWork.Save();
            return product;
        }
    }
}

