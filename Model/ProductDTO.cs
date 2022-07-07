using System;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
namespace Glasses.Model
{

	public class ProductDTO: CreateProductDTO
	{
		public int ID { get; set; }

	}
	public class CreateProductDTO
	{

		[Required]
		//[StringLength(maximumLength:)]
		public string Product_Name { get; set; }
		[Required]
		public double Price { get; set; }
		[Required]
		public string Image { get; set; }




	}

	public class UpdateProductDTO
	{

		[Required]
		//[StringLength(maximumLength:)]
		public string Product_Name { get; set; }
		[Required]
		public double Price { get; set; }
		[Required]
		public string Image { get; set; }




	}
}

