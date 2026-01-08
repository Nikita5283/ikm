using ikm.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Класс, описывающий сессию посещения веб-страницы.
    /// Хранит информацию о времени, домене, заголовке страницы и браузере.
    /// </summary>
    [Table("page_sessions")]
    public class PageSession
    {
        [Key]
        [Column("session_id")]
        public int SessionId { get; set; }

        [Column("activation_date")]
        [Required(ErrorMessage = "Дата открытия обязательна")]
        [Display(Name = "Дата открытия")]
        [DataType(DataType.Date)]
        public DateOnly? ActivationDate { get; set; }

        [Column("activation_time")]
        [Required(ErrorMessage = "Время открытия обязательно")]
        [Display(Name = "Время открытия")]
        [DataType(DataType.Time)]
        public TimeOnly? ActivationTime { get; set; }

        [Column("shutdown_date")]
        [Display(Name = "Дата закрытия")]
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDate { get; set; }

        [Column("shutdown_time")]
        [Display(Name = "Время закрытия")]
        [DataType(DataType.Time)]
        public TimeOnly? ShutdownTime { get; set; }

        [Column("page_title")]
        [Required(ErrorMessage = "Заголовок страницы обязателен")]
        [Display(Name = "Заголовок страницы")]
        public string PageTitle { get; set; }

        // связь 1: Сайт (Domain)

        [Column("domain")]
        [Required(ErrorMessage = "Выберите сайт")]
        [Display(Name = "Домен сайта")]
        public string Domain { get; set; } // Поле для внешнего ключа

        [ForeignKey("Domain")]
        [Display(Name = "Сайт")]
        public Site? Site { get; set; } // Навигационное свойство

        // связь 2: Браузер (App)

        [Column("browser_name")]
        [Required(ErrorMessage = "Выберите браузер")]
        [Display(Name = "Название браузера")]
        public string BrowserName { get; set; } // Поле для внешнего ключа

        [ForeignKey("BrowserName")]
        [Display(Name = "Браузер")]
        public App? Browser { get; set; } // Навигационное свойство
    }
}