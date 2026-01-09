using ikm.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Сессия использования десктопного приложения.
    /// </summary>
    [Table("app_sessions")]
    public class AppSession
    {
        [Key]
        [Column("session_id")]
        public int SessionId { get; set; }

        [Column("activation_date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата открытия обязательна")]
        [Display(Name = "Дата открытия")]
        public DateOnly? ActivationDate { get; set; }

        [Column("activation_time")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Время открытия обязательно")]
        [Display(Name = "Время открытия")]
        public TimeOnly? ActivationTime { get; set; }

        [Column("shutdown_date")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата закрытия")]
        public DateOnly? ShutdownDate { get; set; }

        [Column("shutdown_time")]
        [DataType(DataType.Time)]
        [Display(Name = "Время закрытия")]
        public TimeOnly? ShutdownTime { get; set; }

        [Column("process_name")]
        [Required(ErrorMessage = "Выберите приложение")]
        [Display(Name = "Приложение")]
        public string ProcessName { get; set; }

        [ForeignKey("ProcessName")]
        public App? App { get; set; } // Связь с таблицей Apps

        [Column("window_title")]
        [Required(ErrorMessage = "Заголовок окна обязателен")]
        [Display(Name = "Заголовок окна")]
        public string WindowTitle { get; set; }
    }
}