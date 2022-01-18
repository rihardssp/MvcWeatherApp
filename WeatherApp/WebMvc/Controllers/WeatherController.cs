using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Controllers
{
    public class WeatherController : Controller
    {
        private readonly IWeatherRepositoryWrapper _repository;

        public WeatherController(IWeatherRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: Weather
        public async Task<IActionResult> Index()
        {
            return View(await _repository.WeatherEntry.FindAll().ToListAsync());
        }

        // GET: Weather/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var weatherEntryModel = await _repository.WeatherEntry.FindAll()
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
                _repository.WeatherEntry.Add(weatherEntryModel);
                await _repository.SaveChangesAsync();
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

            var weatherEntryModel = await _repository.WeatherEntry.FindAsync(id);
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
                    _repository.WeatherEntry.Update(weatherEntryModel);
                    await _repository.SaveChangesAsync();
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

            var weatherEntryModel = await _repository.WeatherEntry.FindAll()
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
            var weatherEntryModel = await _repository.WeatherEntry.FindAsync(id);
            _repository.WeatherEntry.Remove(weatherEntryModel);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WeatherEntryModelExists(int id)
        {
            return _repository.WeatherEntry.FindAll().Any(e => e.Id == id);
        }
    }
}
