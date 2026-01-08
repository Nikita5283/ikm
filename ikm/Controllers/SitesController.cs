using ikm.Data;
using ikm.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace ikm.Controllers
{
    /// <summary>
    /// Контроллер для управления справочником сайтов.
    /// </summary>
    public class SitesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SitesController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отображает список всех сайтов.
        /// </summary>
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sites.ToListAsync());
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
        public async Task<IActionResult> Create(Site site)
        {
            if (ModelState.IsValid)
            {
                // Проверка на дубликаты
                if (await _context.Sites.AnyAsync(s => s.Domain == site.Domain))
                {
                    ModelState.AddModelError("Domain", "Такой домен уже существует");
                    return View(site);
                }

                _context.Add(site);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(site);
        }

        /// <summary>
        /// Открывает форму редактирования записи по ID.
        /// </summary>
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();
            var site = await _context.Sites.FindAsync(id);
            if (site == null) return NotFound();
            return View(site);
        }

        /// <summary>
        /// Изменяет запись в БД по ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Site site)
        {
            if (id != site.Domain)
            {
                // Если пользователь поменял имя домена
                if (ModelState.IsValid)
                {
                    try
                    {
                        // EF Core не умеет менять Primary Key через Update. 
                        // Делаем это через SQL. PostgreSQL сам обновит связи (CASCADE)
                        await _context.Database.ExecuteSqlRawAsync(
                            "UPDATE sites SET domain = {0} WHERE domain = {1}",
                            site.Domain, id);

                        return RedirectToAction(nameof(Index));
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "Ошибка при переименовании: " + ex.Message);
                    }
                }
            }
            else
            {
                // Если имя не менялось, просто сохраняем (на случай, если добавятся другие поля)
                if (ModelState.IsValid)
                {
                    _context.Update(site);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(site);
        }

        /// <summary>
        /// Открывает форму удаления записи по ID.
        /// </summary>
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            var site = await _context.Sites.FirstOrDefaultAsync(m => m.Domain == id);
            if (site == null) return NotFound();
            return View(site);
        }

        /// <summary>
        /// Удаляет запись из БД по ID.
        /// </summary>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var site = await _context.Sites.FindAsync(id);
            if (site != null)
            {
                _context.Sites.Remove(site);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}