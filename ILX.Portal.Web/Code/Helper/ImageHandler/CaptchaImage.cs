using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;

namespace ILX.Portal.Web.Code.Helper.ImageHandler
{
	public class CaptchaImage
	{
		private static string RearrangeString(string startingPoint)
		{
			Random num = new Random();
			string rand = new string(startingPoint.
				OrderBy(s => (num.Next(2) % 2) == 0).ToArray());
			return rand;
		}
		public static FileContentResult GetCaptchaImage(out string captcha, bool noisy = true)
		{
			string[] letters =  { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z" };
			Random oRandom = new Random();
			int[] aBackgroundNoiseColor = new int[] { 150, 150, 150 };
			int[] aTextColor = new int[] { 0, 0, 0 };
			int[] aFontEmSizes = new int[] { 15, 20, 25, 12, 22 };

			string[] aFontNames = new string[]
											{
												"Comic Sans MS",
												"Arial",
												"Times New Roman",
												"Georgia",
												"Verdana",
												"Geneva"
											};
			FontStyle[] aFontStyles = new FontStyle[]
											   {
												 FontStyle.Bold,
												 FontStyle.Italic,
												 FontStyle.Regular,
												 FontStyle.Strikeout,
												 FontStyle.Underline
											   };
			HatchStyle[] aHatchStyles = new HatchStyle[]
												   {
												 HatchStyle.BackwardDiagonal, HatchStyle.Cross,
													HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
												 HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
													HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross,
												 HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid,
													HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
												 HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard,
													HatchStyle.LargeConfetti, HatchStyle.LargeGrid,
												 HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal,
													HatchStyle.LightUpwardDiagonal, HatchStyle.LightVertical,
												 HatchStyle.Max, HatchStyle.Min, HatchStyle.NarrowHorizontal,
													HatchStyle.NarrowVertical, HatchStyle.OutlinedDiamond,
												 HatchStyle.Plaid, HatchStyle.Shingle, HatchStyle.SmallCheckerBoard,
													HatchStyle.SmallConfetti, HatchStyle.SmallGrid,
												 HatchStyle.SolidDiamond, HatchStyle.Sphere, HatchStyle.Trellis,
													HatchStyle.Vertical, HatchStyle.Wave, HatchStyle.Weave,
												 HatchStyle.WideDownwardDiagonal, HatchStyle.WideUpwardDiagonal, HatchStyle.ZigZag
												   };

			Random rand = new Random((int)DateTime.Now.Ticks);
			int iHeight = 50;
			int iWidth = 180;
			//generate new question 
			int a = rand.Next(0, 9);
			int b = rand.Next(0, 9);
			int c = rand.Next(0, 9);
			captcha = string.Format("{0}{1}{2}{3}{4}", letters[rand.Next(0, 26)], a, b, c,
				letters[rand.Next(0,26)]);
			captcha = RearrangeString(captcha);

			//store answer 
			//image stream 
			FileContentResult img = null;

			using (MemoryStream mem = new MemoryStream())
			using (Bitmap bmp = new Bitmap(iWidth, iHeight))
			using (Graphics gfx = Graphics.FromImage(bmp))
			{
				Rectangle rectangle = new Rectangle(0, 0, bmp.Width-1, bmp.Height-1);

				//rectangle.

				gfx.TextRenderingHint = TextRenderingHint.SingleBitPerPixelGridFit;//.ClearTypeGridFit;
				gfx.SmoothingMode = SmoothingMode.AntiAlias;
				gfx.FillRectangle(Brushes.BlanchedAlmond, rectangle);


				{
					int i, r, x, y;
					Pen pen = new Pen(Color.Azure);
					for (i = 1; i < 50; i++)
					{
						pen.Color = Color.FromArgb(
						(rand.Next(0, 255)),
						(rand.Next(0, 255)),
						(rand.Next(0, 255)));

						r = rand.Next(0, (iWidth / 10));
						x = rand.Next(0, iWidth);
						y = rand.Next(0, iHeight);

						gfx.DrawEllipse(pen, x - r, y - r, r, r);
					}
					//Pen pen = new Pen(Color.Black, 2);
					pen.Alignment = PenAlignment.Inset; //<-- this
					gfx.DrawRectangle(pen, rectangle);
				}


				System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
				//add question 
				gfx.DrawString("www.InstaLogix.com", new Font("Georgia", 12), Brushes.Gray, 12, 30);
				int ai = 0;
				for (ai = 0; ai <= captcha.Length - 1; ai++)
				{
					oMatrix.Reset();
					int iChars = captcha.Length;
					int x = iWidth / (iChars + 1) * ai;
					int y = iHeight / 4;

					//Rotate text Random
					oMatrix.RotateAt(oRandom.Next(0, 40), new PointF(x, y));
					gfx.Transform = oMatrix;
					//--
					gfx.DrawString
							(
							//Text
							captcha.Substring(ai, 1),
							//Random Font Name and Style
							new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
							   aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
							   aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
							//Random Color (Darker colors RGB 0 to 100)
							new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
							   oRandom.Next(0, 100), oRandom.Next(0, 100))),
							x+10,y-10
							//oRandom.Next(10, iHeight-10)
							);
					//---
				}
				//gfx.DrawString(captcha, new Font("Tahoma", 15), Brushes.Black, 2, 3);

				//render as Jpeg 
				bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
				img = new FileContentResult(mem.GetBuffer(), "image/Jpeg");
			}

			return img;
		}

		public static byte[] GetCaptcha()
		{
			byte[] imgData = null;
			int width = 100;
			int height = 80;
			using (Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
			{

				Graphics graphics = Graphics.FromImage(bitmap);

				Pen pen = new Pen(Color.Red, 5);

				graphics.FillRectangle(new SolidBrush(Color.Silver), new Rectangle(0, 0, width, height));
				using (MemoryStream ms = new MemoryStream())
				{
					bitmap.Save(ms, ImageFormat.Jpeg);
					imgData = ms.ToArray();//.Read(imgData,0, ms.);
				}
			}
			return imgData;
		}

		private static void GetImage(string[] args)
		{
			int width = 128;
			int height = 128;
			string file = args[0];
			Console.WriteLine($"Loading {file}");
			using (FileStream pngStream = new FileStream(args[0], FileMode.Open, FileAccess.Read))
			using (Bitmap image = new Bitmap(pngStream))
			{
				Bitmap resized = new Bitmap(width, height);
				using (Graphics graphics = Graphics.FromImage(resized))
				{
					graphics.CompositingQuality = CompositingQuality.HighSpeed;
					graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
					graphics.CompositingMode = CompositingMode.SourceCopy;
					graphics.DrawImage(image, 0, 0, width, height);
					resized.Save($"resized-{file}", ImageFormat.Png);
					Console.WriteLine($"Saving resized-{file} thumbnail");
				}
			}
		}
	}
}
