using ListaFilmConMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

namespace Day14Lab1PokeDex.Controllers
{
    public class FileUploadController : Controller
    {
        private readonly DataContext _context;

        public FileUploadController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadFile()
        {
            if(Request.Form.Files.Count > 0)
            {
                foreach(var file in Request.Form.Files) 
                {    
                    var filename = file.FileName;

                 /*   //to file
                    using (var outputFile = System.IO.File.Create($"UploadFiles/{filename}"))
                    {
                        //file.CopyTo(outputFile);
                    } */

                      // to DB
                    using(MemoryStream ms = new MemoryStream())
                    { 
                        file.CopyTo(ms);
                        Picture pic = new Picture
                        {
                            PictureName = filename,
                            RawData = ms.ToArray()
                        };
                        _context.Pictures.Add(pic);
                    }                    
                } 
                _context.SaveChanges();
            }
            return Ok("Saved!");
        }

        [HttpGet]
        public IActionResult GetFile(string fileName) 
        {
            string fullname = $"UploadFiles/{fileName}";

            if(!System.IO.File.Exists(fullname)) 
            {
                return NotFound();
            }
            //

            //
            var rawData = System.IO.File.ReadAllBytes(fullname);
            //watermark
            Bitmap bitmap = new Bitmap(fullname, true);

            Graphics graphic = Graphics.FromImage(bitmap);

            string myWater = "LBLogo";

            Size l_w = graphic.MeasureString(myWater, SystemFonts.DefaultFont).ToSize();

            Point p = new Point(62, 12); // 10 20, 11 21

            Point p1 = new Point(63, 13);

            //background
            graphic.FillRectangle(new SolidBrush(Color.DarkGray), new Rectangle(p, l_w));
            //subpixeling
            graphic.DrawString(myWater, SystemFonts.DefaultFont, Brushes.Black, p1);
            //
            graphic.DrawString(myWater, SystemFonts.DefaultFont, Brushes.White, p);

            using(MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.ToArray(),"image/png");
            }


            //We fix this is an image
           // return File(rawData, "image/png");
        }

        //funzione sopra per file sotto per db

        [HttpGet]
        public IActionResult GetDB(int Id = 1)
        {
            Picture pic = _context.Pictures.Find(Id);
            if (pic == null)
            {
                return NotFound();
            }

            var rawData = pic.RawData;

            MemoryStream ms1 = new MemoryStream(rawData);
            //watermark
            Bitmap bitmap = new Bitmap(Bitmap.FromStream(ms1));

            Graphics graphic = Graphics.FromImage(bitmap);

            string myWater = "CM";

            Size l_w = graphic.MeasureString(myWater, SystemFonts.DefaultFont).ToSize();

            Point p = new Point(10, 20); 

            Point p1 = new Point(11, 21);

            //background
            graphic.FillRectangle(new SolidBrush(Color.DarkGray), new Rectangle(p, l_w));
            //subpixeling
            graphic.DrawString(myWater, SystemFonts.DefaultFont, Brushes.Black, p1);
            //
            graphic.DrawString(myWater, SystemFonts.DefaultFont, Brushes.White, p);

            using (MemoryStream ms = new MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return File(ms.ToArray(), "image/png");
            }


            //We fix this is an image
            // return File(rawData, "image/png");
        }
    }
}
