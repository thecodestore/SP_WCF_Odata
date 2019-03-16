using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ILX.Helper;
using ILX.Portal.Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ILX.Portal.Web.Areas.Identity.Pages.Account.Manage
{
	public class manageusersModel : PageModel
	{
		private readonly UserManager<ApplicationUser> _userManager;
		[TempData]
		public string StatusMessage { get; set; }
		public manageusersModel(
			UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			IEmailSender emailSender)
		{
			_userManager = userManager;
		}
		[BindProperty]
		public InputModel Input { get; set; }

		public class InputModel
		{
			[Required]
			[EmailAddress]
			public string Email { get; set; }

			[Phone]
			[Display(Name = "Phone number")]
			public string PhoneNumber { get; set; }

			[Required]
			[Display(Name = "First Name")]
			public string FirstName { get; set; }

			[Required]
			[Display(Name = "Last Name")]
			public string LastName { get; set; }
			[Required]
			[Display(Name = "Address")]
			public string Address { get; set; }
		}
		public string ReturnUrl { get; set; }
		public string Username { get; set; }

		public bool IsEmailConfirmed { get; set; }

		
		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}
			Input = user.ConvertObject<ApplicationUser, InputModel>();
			_userManager.Users.ToList().ForEach(usr => {
				Username += usr.UserName + ",";
			});
			return Page();
		}
	}
}