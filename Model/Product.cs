using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Glasses.Model
{
	
	public class Product
	{
		public int ProductId { get; set; }
		public string Product_Name { get; set; }
		public double Price { get; set; }
		public string Image { get; set; }
		
	}
	
}

