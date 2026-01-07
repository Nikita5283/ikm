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
        [Key]
        [Column("domain")]
        [Display(Name = "Домен")]
        [Required(ErrorMessage = "Имя домена обязательно")]
        public string Domain { get; set; }

        // Навигационные свойства для связей
        public ICollection<ViewSession>? ViewSessions { get; set; }
        public ICollection<PageSession>? PageSessions { get; set; }
    }
}