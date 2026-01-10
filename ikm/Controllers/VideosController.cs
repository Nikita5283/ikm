using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ikm.Data;
using ikm.Models;

namespace ikm.Controllers
{
    /// <summary>
    /// Контроллер для управления справочником приложений.
    /// </summary>
    public class VideosController : Controller
    {
        /// <summary>
        /// Поле контекста базы данных.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Конструктор контроллера с внедрением контекста базы данных.
        /// </summary>
        /// <param name="context">Контекст базы данных</param>
        public VideosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отображает список всех приложений.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Videos.ToListAsync());
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
        public async Task<IActionResult> Create(Video video)
        {
            if (ModelState.IsValid)
            {
                if (await _context.Videos.AnyAsync(a => a.VideoId == video.VideoId))
                {
                    ModelState.AddModelError("VideoId", "Такое id уже существует");
                    return View(video);
                }

                _context.Add(video);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        /// <summary>
        /// Открывает форму редактирования записи по ID.
        /// </summary>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var video = await _context.Videos.FindAsync(id);
            if (video == null) return NotFound();
            return View(video);
        }

        /// <summary>
        /// Изменяет запись в БД по ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Video video)
        {
            if (id != video.VideoId)
            {
                // Если ID изменился, используем прямой SQL для обхода ограничений EF Core
                await _context.Database.ExecuteSqlRawAsync(
                    "UPDATE videos SET video_id = {0}, title = {1}, autor = {2}, video_duration = {3} WHERE video_id = {4}",
                    video.VideoId,
                    video.Title,
                    video.Author,
                    video.VideoDuration,
                    id);
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(video);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Videos.Any(e => e.VideoId == video.VideoId)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        /// <summary>
        /// Открывает форму удаления записи по ID.
        /// </summary>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var video = await _context.Videos.FirstOrDefaultAsync(m => m.VideoId == id);
            if (video == null) return NotFound();
            return View(video);
        }

        /// <summary>
        /// Удаляет запись из БД по ID.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var video = await _context.Videos.FindAsync(id);
            if (video != null)
            {
                _context.Videos.Remove(video);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}