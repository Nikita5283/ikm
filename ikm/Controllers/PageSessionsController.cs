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
    public class PageSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PageSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отображает список всех посещений страниц.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Include подгружает данные из связанных таблиц (название видео вместо ID)
            var sessions = _context.PageSessions
                .Include(v => v.Browser)
                .Include(v => v.Site);
            return View(await sessions.ToListAsync());
        }

        /// <summary>
        /// Открывает форму создания новой записи.
        /// </summary>
        public IActionResult Create()
        {
            var browsers = _context.Apps.ToList()
                .Where(a => a.ProcessName == "chrome.exe" || a.ProcessName == "msedge.exe" || a.ProcessName == "firefox.exe" || a.ProcessName == "yandex.exe");
            var sites = _context.Sites.ToList();

            // Проверяем наличие браузеров
            if (!browsers.Any())
            {
                // Добавляем ошибку в состояние модели, чтобы она отобразилась во View
                ModelState.AddModelError("BrowserName", "Нужно добавить хотя бы один браузер в справочник");
            }

            // Проверяем наличие сайтов
            if (!sites.Any())
            {
                ModelState.AddModelError("Domain", "Нужно добавить хотя бы один сайт в справочник");
            }

            ViewData["BrowserName"] = new SelectList(browsers, "ProcessName", "BaseName");
            ViewData["Domain"] = new SelectList(sites, "Domain", "Domain");
            return View();
        }


        /// <summary>
        /// Сохраняет новую запись в БД.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PageSession pageSession)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pageSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var browsers = _context.Apps
                .Where(a => a.ProcessName == "chrome.exe" || a.ProcessName == "msedge.exe" || a.ProcessName == "firefox.exe" || a.ProcessName == "yandex.exe");

            ViewData["BrowserName"] = new SelectList(browsers, "ProcessName", "BaseName", pageSession.BrowserName);
            ViewData["Domain"] = new SelectList(_context.Sites, "Domain", "Domain", pageSession.Domain);
            return View(pageSession);
        }

        /// <summary>
        /// Открывает форму редактирования записи по ID.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var pageSession = await _context.PageSessions.FindAsync(id);
            if (pageSession == null) return NotFound();

            var browsers = _context.Apps
                .Where(a => a.ProcessName == "chrome.exe" || a.ProcessName == "msedge.exe" || a.ProcessName == "firefox.exe" || a.ProcessName == "yandex.exe");

            ViewData["BrowserName"] = new SelectList(browsers, "ProcessName", "BaseName", pageSession.BrowserName);
            ViewData["Domain"] = new SelectList(_context.Sites, "Domain", "Domain", pageSession.Domain);

            return View(pageSession);
        }

        /// <summary>
        /// Изменяет запись в БД по ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PageSession pageSession)
        {
            if (id != pageSession.SessionId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pageSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ViewSessions.Any(e => e.ViewId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            var browsers = _context.Apps
                .Where(a => a.ProcessName == "chrome.exe" || a.ProcessName == "msedge.exe" || a.ProcessName == "firefox.exe" || a.ProcessName == "yandex.exe");

            ViewData["BrowserName"] = new SelectList(browsers, "ProcessName", "BaseName", pageSession.BrowserName);
            ViewData["Domain"] = new SelectList(_context.Sites, "Domain", "Domain", pageSession.Domain);
            return View(pageSession);
        }


        /// <summary>
        /// Открывает форму удаления записи по ID.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var session = await _context.PageSessions
                .Include(v => v.Browser)
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
            var session = await _context.PageSessions.FindAsync(id);
            if (session != null)
            {
                _context.PageSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}


       
