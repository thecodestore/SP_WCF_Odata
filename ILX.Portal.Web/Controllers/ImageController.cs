using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ILX.Portal.Web.Code.Helper.ImageHandler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ILX.Portal.Web.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
    public class ImageController : ControllerBase
    {

		[HttpGet]
		[ActionName("GetImageName")]
		public string GetImageName()
		{
			return (new System.Random()).Next(1000, 1000000000).ToString();
		}

		[HttpGet]
		[ActionName("GetImage")]
		public FileContentResult GetImage()
		{
			string captcha = string.Empty;
			var imgOut= CaptchaImage.GetCaptchaImage(out captcha);
			HttpContext.Session.SetString("captcha", captcha);
			return imgOut;
		}

		[HttpGet]
		[ActionName("IsImageText")]
		public bool IsImageText(string captcha)
		{
			return captcha.Equals(HttpContext.Session.GetString("captcha"));
		}
	}
}