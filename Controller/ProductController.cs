using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Glasses.Model;
using Glasses.Data;
using Microsoft.EntityFrameworkCore;
using Glasses.IRepository;
using System.Data.Entity;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;

namespace Glasses.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly GlassesContext context;
     
        private readonly ILogger<ProductController> _logger;
        private readonly IMapper _mapper;
        private readonly IProductService
            _productService;

      

        public ProductController( ILogger<ProductController> logger,
             IMapper mapper, IProductService productService)
        {
       
            _logger = logger;
          
            _mapper = mapper;
            _productService = productService;
        }



        [HttpGet]
        public async Task<IActionResult> GetProdcuts()
        {
            try
            {
                var products = await _productService.GetAllProducts();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the{nameof(GetProdcuts)}");
                return StatusCode(500, "Internal Server Error. Try Again later.");
            }
        }

        [HttpGet("{id:int}", Name = "GetProduct")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _productService.GetSingleProduct(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the{nameof(GetProduct)}");
                return StatusCode(500, "Internal Server Error. Try Again later.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductDTO ProductDTO)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid Post Attempt in {nameof(CreateProduct)}");
                return BadRequest(ModelState);
            }
            try
            {

                //var product = _mapper.Map<Product>(ProductDTO);
                var product = await _productService.CreatePRoduct(ProductDTO);
               

                return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the{nameof(CreateProduct)}");
                return StatusCode(500, "Internal Server Error. Try Again later.");
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invalid Delete Attempt in {nameof(DeleteProduct)}");
                return BadRequest();
            }
            try
            {
                var product = await _productService.DeleteProduct( id);
       

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the{nameof(DeleteProduct)}");
                return StatusCode(500, "Internal Server Error. Try Again later.");
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductDTO ProductDTO)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid Update Attempt in {nameof(UpdateProduct)}");
                return BadRequest(ModelState);
            }
            try
            {
                var product = await _productService.EditProduct(id, ProductDTO);

                //_unitOfWork.Products.Update(product);
                //await context.SaveChangesAsync();

                return CreatedAtRoute("GetProduct", new { id = product.ProductId }, product);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the{nameof(UpdateProduct)}");
                return StatusCode(500, "Internal Server Error. Try Again later.");
            }
        }


    }
}
