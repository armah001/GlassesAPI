using System;
using AutoMapper;
using Glasses.Model;
using Glasses.Data;

namespace Glasses.Configurations
{
	public class MapperInitializer : Profile
	{
		public MapperInitializer()
		{
			CreateMap<Product, ProductDTO>().ReverseMap();

			CreateMap<Product, CreateProductDTO>().ReverseMap();

			CreateMap<Product, UpdateProductDTO>().ReverseMap();

			CreateMap<ApiUser, UserDTO>().ReverseMap();

			//CreateMap<ApiUser, LoginUserDTO>().ReverseMap();
		}
	}
}

