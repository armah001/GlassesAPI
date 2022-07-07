using System;
using Glasses.Data;
using Glasses.Model;
using Microsoft.AspNetCore.Identity;

namespace Glasses
{
	public static class ServiceExtension
	{
		public static void ConfigureIdentity(this IServiceCollection services)
		{
			var builder = services.AddIdentityCore<ApiUser>(q=> q.User.RequireUniqueEmail=true);
			builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
			builder.AddEntityFrameworkStores<GlassesContext>().AddDefaultTokenProviders();

		}
	}
}

