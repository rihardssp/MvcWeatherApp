using DataAccessLayer.Contexts;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace WebMvc.Controllers
{
    public class LocationController : Controller
    {
        private readonly IWeatherRepositoryWrapper _repository;

        public LocationController(IWeatherRepositoryWrapper repository)
        {
            _repository = repository;
        }

        // GET: Location
        public async Task<IActionResult> Index()
        {
            return View(await _repository.Location.FindAll().ToListAsync());
        }

        // GET: Location/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationModel = await _repository.Location.FindAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationModel == null)
            {
                return NotFound();
            }

            return View(locationModel);
        }

        // GET: Location/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Location/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Country,City,ApiId")] LocationModel locationModel)
        {
            if (ModelState.IsValid)
            {
                _repository.Location.Add(locationModel);
                await _repository.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(locationModel);
        }

        // GET: Location/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationModel = await _repository.Location.FindAsync(id);
            if (locationModel == null)
            {
                return NotFound();
            }
            return View(locationModel);
        }

        // POST: Location/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Country,City,ApiId")] LocationModel locationModel)
        {
            if (id != locationModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _repository.Location.Update(locationModel);
                    await _repository.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LocationModelExists(locationModel.Id))
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
            return View(locationModel);
        }

        // GET: Location/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var locationModel = await _repository.Location.FindAll()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (locationModel == null)
            {
                return NotFound();
            }

            return View(locationModel);
        }

        // POST: Location/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var locationModel = await _repository.Location.FindAsync(id);
            _repository.Location.Remove(locationModel);
            await _repository.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LocationModelExists(int id)
        {
            return _repository.Location.FindAll().Any(e => e.Id == id);
        }
    }
}
