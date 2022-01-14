using CoreWeatherApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using WebMvc.Data;

namespace WebMvc.Controllers
{
    public class WeatherController : Controller
    {
        private readonly WeatherContext _context;

        public WeatherController(WeatherContext context)
        {
            _context = context;
        }

        // GET: Weather
        public async Task<IActionResult> Index()
        {
            return View(await _context.WeatherEntryModel.ToListAsync());
        }

        // GET: Weather/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherEntryModel = await _context.WeatherEntryModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherEntryModel == null)
            {
                return NotFound();
            }

            return View(weatherEntryModel);
        }

        // GET: Weather/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Weather/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Date,Temperature,WindSpeed")] WeatherEntryModel weatherEntryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(weatherEntryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(weatherEntryModel);
        }

        // GET: Weather/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherEntryModel = await _context.WeatherEntryModel.FindAsync(id);
            if (weatherEntryModel == null)
            {
                return NotFound();
            }
            return View(weatherEntryModel);
        }

        // POST: Weather/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Temperature,WindSpeed")] WeatherEntryModel weatherEntryModel)
        {
            if (id != weatherEntryModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(weatherEntryModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WeatherEntryModelExists(weatherEntryModel.Id))
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
            return View(weatherEntryModel);
        }

        // GET: Weather/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherEntryModel = await _context.WeatherEntryModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (weatherEntryModel == null)
            {
                return NotFound();
            }

            return View(weatherEntryModel);
        }

        // POST: Weather/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var weatherEntryModel = await _context.WeatherEntryModel.FindAsync(id);
            _context.WeatherEntryModel.Remove(weatherEntryModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherEntryModelExists(int id)
        {
            return _context.WeatherEntryModel.Any(e => e.Id == id);
        }
    }
}
