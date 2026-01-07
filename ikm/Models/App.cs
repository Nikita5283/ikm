using ikm.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Класс, представляющий приложение в системе мониторинга.
    /// </summary>
    [Table("apps")]
    public class App
    {
        [Key]
        [Column("process_name")]
        [Display(Name = "Имя процесса")]
        [Required(ErrorMessage = "Имя процесса обязательно")]
        public string ProcessName { get; set; }

        [Column("base_name")]
        [Display(Name = "Название приложения")]
        public string? BaseName { get; set; }

        // Навигационные свойства для связей
        public ICollection<AppSession>? AppSessions { get; set; }
        public ICollection<PageSession>? PageSessions { get; set; }
    }
}