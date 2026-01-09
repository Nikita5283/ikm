using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering; // Нужно для выпадающих списков
using Microsoft.EntityFrameworkCore;
using ikm.Data;
using ikm.Models;

namespace ikm.Controllers
{
    /// <summary>
    /// Контроллер для управления сессиями просмотра видео.
    /// </summary>
    public class AppSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отображает список всех посещений страниц.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Include подгружает данные из связанных таблиц (название видео вместо ID)
            var sessions = _context.AppSessions
                .Include(v => v.App);
            return View(await sessions.ToListAsync());
        }

        /// <summary>
        /// Открывает форму создания новой записи.
        /// </summary>
        public IActionResult Create()
        {
            var apps = _context.Apps.ToList();

            // Проверяем наличие приложений
            if (!apps.Any())
            {
                // Добавляем ошибку в состояние модели, чтобы она отобразилась во View
                ModelState.AddModelError("ProcessName", "Нужно добавить хотя бы одно приложение в справочник");
            }

            ViewData["ProcessName"] = new SelectList(apps, "ProcessName", "BaseName");
            return View();
        }


        /// <summary>
        /// Сохраняет новую запись в БД.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AppSession appSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var apps = _context.Apps.ToList();                

            ViewData["ProcessName"] = new SelectList(apps, "ProcessName", "BaseName", appSession.ProcessName);
            return View(appSession);
        }

        /// <summary>
        /// Открывает форму редактирования записи по ID.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var appSession = await _context.AppSessions.FindAsync(id);
            if (appSession == null) return NotFound();

            var apps = _context.Apps.ToList();

            ViewData["ProcessName"] = new SelectList(apps, "ProcessName", "BaseName", appSession.ProcessName);
            return View(appSession);
        }

        /// <summary>
        /// Изменяет запись в БД по ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AppSession appSession)
        {
            if (id != appSession.SessionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AppSessions.Any(e => e.SessionId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var apps = _context.Apps.ToList();

            ViewData["ProcessName"] = new SelectList(apps, "ProcessName", "BaseName", appSession.ProcessName);
            return View(appSession);
        }


        /// <summary>
        /// Открывает форму удаления записи по ID.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var session = await _context.AppSessions
                .Include(v => v.App)
                .FirstOrDefaultAsync(m => m.SessionId == id);

            if (session == null) return NotFound();

            return View(session);
        }

        /// <summary>
        /// Удаляет запись из БД по ID.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var session = await _context.AppSessions.FindAsync(id);
            if (session != null)
            {
                _context.AppSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}



