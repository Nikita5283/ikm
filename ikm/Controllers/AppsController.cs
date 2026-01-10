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
        /// <summary>
        /// Поле контекста базы данных.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Конструктор контроллера с внедрением контекста базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
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
        /// <param name="app">Объект приложения для создания</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(App app)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Apps.AnyAsync(a => a.ProcessName == app.ProcessName))
                {
                    ModelState.AddModelError("ProcessName", "Такое приложение уже существует");
                    return View(app);
                }

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
            // Если ID изменился (пользователь поменял ProcessName)
            if (id != app.ProcessName)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        // Прямой SQL запрос для обновления PK
                        // Также обновляем BaseName
                        await _context.Database.ExecuteSqlRawAsync(
                            "UPDATE apps SET process_name = {0}, base_name = {1} WHERE process_name = {2}",
                            app.ProcessName, app.BaseName ?? (object)DBNull.Value, id);

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Ошибка обновления: " + ex.Message);
                    }
                }
            }
            else
            {
                // Стандартное обновление, если ключ не меняли
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(app);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!_context.Apps.Any(e => e.ProcessName == app.ProcessName)) return NotFound();
                        else throw;
                    }
                    return RedirectToAction(nameof(Index));
                }
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