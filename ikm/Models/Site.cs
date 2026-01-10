using ikm.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Класс, представляющий сайт в системе мониторинга.
    /// </summary>
    [Table("sites")]
    public class Site
    {
        /// <summary>
        /// Домен сайта.
        /// </summary>
        [Key]
        [Column("domain")]
        [Display(Name = "Домен")]
        [Required(ErrorMessage = "Домен обязателен")]
        public string Domain { get; set; }

        /// <summary>
        /// Навигационное свойство для сессий просмотров.
        /// </summary>
        public ICollection<ViewSession>? ViewSessions { get; set; }

        /// <summary>
        /// Навигационное свойство для сессий страниц.
        /// </summary>
        public ICollection<PageSession>? PageSessions { get; set; }
    }
}