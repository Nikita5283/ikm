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
    public class ViewSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ViewSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отображает список всех просмотров.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            // Include подгружает данные из связанных таблиц (название видео вместо ID)
            var sessions = _context.ViewSessions
                .Include(v => v.Video)
                .Include(v => v.Site);
            return View(await sessions.ToListAsync());
        }

        /// <summary>
        /// Открывает форму создания новой записи.
        /// </summary>
        public IActionResult Create()
        {
            var yt_videos = _context.Videos.ToList();
            var sites = _context.Sites.ToList();

            // Проверяем наличие браузеров
            if (!yt_videos.Any())
            {
                // Добавляем ошибку в состояние модели, чтобы она отобразилась во View
                ModelState.AddModelError("BrowserName", "Нужно добавить хотя бы один браузер в справочник");
            }

            // Проверяем наличие сайтов
            if (!sites.Any())
            {
                ModelState.AddModelError("Domain", "Нужно добавить хотя бы один сайт в справочник");
            }

            // Загружаем списки для выпадающих меню
            ViewData["VideoId"] = new SelectList(yt_videos, "VideoId", "Title");
            ViewData["Domain"] = new SelectList(sites, "Domain", "Domain");
            return View();
        }

        /// <summary>
        /// Сохраняет новую запись в БД.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ViewSession viewSession)
        {
            // Если валидация прошла успешно
            if (ModelState.IsValid)
            {
                _context.Add(viewSession);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            // Если ошибка, снова загружаем списки и показываем форму
            ViewData["VideoId"] = new SelectList(_context.Videos, "VideoId", "Title", viewSession.VideoId);
            ViewData["Domain"] = new SelectList(_context.Sites, "Domain", "Domain", viewSession.Domain);
            return View(viewSession);
        }

        /// <summary>
        /// Открывает форму редактирования записи по ID.
        /// </summary>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var viewSession = await _context.ViewSessions.FindAsync(id);
            if (viewSession == null) return NotFound();

            // Заполняем списки, чтобы они не были пустыми при открытии редактирования
            ViewData["VideoId"] = new SelectList(_context.Videos, "VideoId", "Title", viewSession.VideoId);
            ViewData["Domain"] = new SelectList(_context.Sites, "Domain", "Domain", viewSession.Domain);

            return View(viewSession);
        }

        /// <summary>
        /// Изменяет запись в БД по ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ViewSession viewSession)
        {
            if (id != viewSession.ViewId) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewSession);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ViewSessions.Any(e => e.ViewId == id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            // Восстанавливаем списки, если была ошибка валидации
            ViewData["VideoId"] = new SelectList(_context.Videos, "VideoId", "Title", viewSession.VideoId);
            ViewData["Domain"] = new SelectList(_context.Sites, "Domain", "Domain", viewSession.Domain);
            return View(viewSession);
        }


        /// <summary>
        /// Открывает форму удаления записи по ID.
        /// </summary>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var session = await _context.ViewSessions
                .Include(v => v.Video)
                .FirstOrDefaultAsync(m => m.ViewId == id);

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
            var session = await _context.ViewSessions.FindAsync(id);
            if (session != null)
            {
                _context.ViewSessions.Remove(session);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}