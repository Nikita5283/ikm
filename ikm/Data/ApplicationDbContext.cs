using ikm.Models;
using Microsoft.EntityFrameworkCore;

namespace ikm.Data
{
    /// <summary>
    /// Контекст базы данных для взаимодействия с PostgreSQL.
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
        }

        public DbSet<App> Apps { get; set; }
        public DbSet<Site> Sites { get; set; }
        public DbSet<YoutubeVideo> YoutubeVideos { get; set; }
        public DbSet<PageSession> PageSessions { get; set; }
        public DbSet<AppSession> AppSessions { get; set; }
        public DbSet<ViewSession> ViewSessions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка связей, если автоматическая не сработает

            // 1. Связи для AppSession (Сессии приложений)
            modelBuilder.Entity<AppSession>()
                .HasOne(s => s.App)
                .WithMany(a => a.AppSessions)
                .HasForeignKey(s => s.ProcessName)
                .OnDelete(DeleteBehavior.Cascade);


            // 2. Связи для PageSession (Сессии веб-страниц)
            modelBuilder.Entity<PageSession>(entity =>
            {
                // Связь с таблицей Sites (по домену)
                entity.HasOne(ps => ps.Site)
                    .WithMany(p => p.PageSessions)
                    .HasForeignKey(ps => ps.Domain)
                    .OnDelete(DeleteBehavior.Cascade);

                // Связь с таблицей Apps (браузер)
                entity.HasOne(ps => ps.Browser)
                    .WithMany(p => p.PageSessions)
                    .HasForeignKey(ps => ps.BrowserName)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 3. Связи для ViewSession (Просмотры YouTube)
            modelBuilder.Entity<ViewSession>(entity =>
            {
                // Связь с видео
                entity.HasOne(vs => vs.Video)
                    .WithMany(v => v.ViewSessions)
                    .HasForeignKey(vs => vs.VideoId)
                    .OnDelete(DeleteBehavior.Cascade);

                // Связь с сайтом
                entity.HasOne(vs => vs.Site)
                    .WithMany(v => v.ViewSessions)
                    .HasForeignKey(vs => vs.Domain)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);

            
        }
    }
}