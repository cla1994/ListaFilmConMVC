using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ListaFilmConMVC.Models;
using System.Text.Json.Nodes;

namespace ListaFilmConMVC.Controllers
{
    public class FilmsController : Controller
    {
        private readonly DataContext _context;

        public FilmsController(DataContext context)
        {
            _context = context;



            if (_context.Films.Count() == 0)
            {
                var json = JsonObject.Parse(System.IO.File.ReadAllText("movieslist.json"));
                var htp = new HttpClient();
                foreach (var MyFilm in json.AsArray())
                {
                    Film NuovoFilm = new Film()
                    {
                        Title = MyFilm!["Title"].AsValue().ToString(),
                        Year = MyFilm!["Year"].AsValue().ToString(),
                        imdbIdentifier = MyFilm!["imdbID"].AsValue().ToString(),
                        Type = MyFilm!["Type"].AsValue().ToString()
                    };

                    var res = htp.GetAsync(MyFilm["Poster"].AsValue().ToString()).Result;
                    if (res.IsSuccessStatusCode)
                    {
                        byte[] img = res.Content.ReadAsByteArrayAsync().Result;
                        Picture pic = new Picture()
                        {
                            PictureName = NuovoFilm.Title,
                            RawData = img
                        };
                        _context.Pictures.Add(pic);
                        NuovoFilm.Picture = pic;
                    }

                    _context.Films.Add(NuovoFilm);
                }
                _context.SaveChanges();
            }



        }

        // GET: Films
        public async Task<IActionResult> Index(int pagenum = 1, int pagesize = 20)
        {

            ViewData["pagenum"] = pagenum;
            ViewData["pagesize"] = pagesize;

            return _context.Films != null ?
                          View(await _context.Films
                          .Include(x => x.Picture)
                          .Skip(pagesize * (pagenum - 1))
                          .Take(pagesize)
                          .ToListAsync()) :
                          Problem("Entity set 'DataContext.Films'  is null.");

        }

        // GET: Films/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.FilmID == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // GET: Films/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmID,Title,Year,imdbIdentifier,Type,Poster")] Film film)
        {
            if (ModelState.IsValid)
            {
                _context.Add(film);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films.FindAsync(id);
            if (film == null)
            {
                return NotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FilmID,Title,Year,imdbIdentifier,Type,Poster")] Film film)
        {
            if (id != film.FilmID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(film);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmExists(film.FilmID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(film);
        }

        // GET: Films/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Films == null)
            {
                return NotFound();
            }

            var film = await _context.Films
                .FirstOrDefaultAsync(m => m.FilmID == id);
            if (film == null)
            {
                return NotFound();
            }

            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Films == null)
            {
                return Problem("Entity set 'DataContext.Films'  is null.");
            }
            var film = await _context.Films.FindAsync(id);
            if (film != null)
            {
                _context.Films.Remove(film);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmExists(int id)
        {
            return (_context.Films?.Any(e => e.FilmID == id)).GetValueOrDefault();
        }
    }
}
