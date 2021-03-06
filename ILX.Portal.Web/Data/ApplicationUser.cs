﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ILX.Portal.Web.Data
{
	public class ApplicationUser: IdentityUser
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Address { get; set; }
		public DateTime DateOfBirth { get; set; }

		public string UserType { get; set; }
		public bool	 IsDisabled { get; set; }
		public bool	IsDeleted { get; set; }

	public string DisplayName
		{
			get
			{
				return string.Format("{0} {1}",FirstName, LastName);
			}
		}
	}
}
