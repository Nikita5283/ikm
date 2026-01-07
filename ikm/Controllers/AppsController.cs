using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ikm.Data;
using ikm.Models;

namespace ikm.Controllers
{
    /// <summary>
    /// Контроллер для управления справочником приложений.
    /// </summary>
    public class AppsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отображает список всех приложений.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Apps.ToListAsync());
        }

        /// <summary>
        /// Открывает форму создания новой записи.
        /// </summary>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Сохраняет новую запись в БД.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(App app)
        {
            if (ModelState.IsValid)
            {
                _context.Add(app);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(app);
        }

        /// <summary>
        /// Открывает форму редактирования записи по ID.
        /// </summary>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            var app = await _context.Apps.FindAsync(id);
            if (app == null) return NotFound();

            return View(app);
        }

        /// <summary>
        /// Изменяет запись в БД по ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, App app)
        {
            if (id != app.ProcessName) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(app);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Apps.Any(e => e.ProcessName == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(app);
        }

        /// <summary>
        /// Открывает форму удаления записи по ID.
        /// </summary>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();

            var app = await _context.Apps.FirstOrDefaultAsync(m => m.ProcessName == id);
            if (app == null) return NotFound();

            return View(app);
        }

        /// <summary>
        /// Удаляет запись из БД по ID.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var app = await _context.Apps.FindAsync(id);
            if (app != null)
            {
                _context.Apps.Remove(app);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}