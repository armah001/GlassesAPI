using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Glasses.Model
{
	public class ApiUser
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int UserId { get; set; }
		public string UserName { get; set; }
		public string Password { get; set; }
	}

	public class Jwt
	{
		

		public string key { get; set; }
		public string Issuer { get; set; }
		public string Audience { get; set; }
		public string Subject { get; set; }
	}
}

