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
        [Display(Name = "Дата начала")]
        public DateOnly ActivationDate { get; set; }

        [Column("activation_time")]
        [DataType(DataType.Time)]
        [Display(Name = "Время начала")]
        public TimeOnly ActivationTime { get; set; }

        [Column("shutdown_date")]
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDate { get; set; }

        [Column("shutdown_time")]
        [DataType(DataType.Time)]
        public TimeOnly? ShutdownTime { get; set; }

        [Column("process_name")]
        [Display(Name = "Приложение")]
        public string ProcessName { get; set; }

        [ForeignKey("ProcessName")]
        public App? App { get; set; } // Связь с таблицей Apps

        [Column("window_title")]
        [Display(Name = "Заголовок окна")]
        public string WindowTitle { get; set; }
    }
}