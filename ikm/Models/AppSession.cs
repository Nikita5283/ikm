using ikm.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Сессии использования десктопного приложения.
    /// </summary>
    [Table("app_sessions")]
    public class AppSession
    {
        /// <summary>
        /// id сессии.
        /// </summary>
        [Key]
        [Column("session_id")]
        public int SessionId { get; set; }

        /// <summary>
        /// Дата открытия приложения.
        /// </summary>
        [Column("activation_date")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Дата открытия обязательна")]
        [Display(Name = "Дата открытия")]
        public DateOnly? ActivationDate { get; set; }

        /// <summary>
        /// Время открытия приложения.
        /// </summary>
        [Column("activation_time")]
        [DataType(DataType.Time)]
        [Required(ErrorMessage = "Время открытия обязательно")]
        [Display(Name = "Время открытия")]
        public TimeOnly? ActivationTime { get; set; }

        /// <summary>
        /// Дата закрытия приложения.
        /// </summary>
        [Column("shutdown_date")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата закрытия")]
        public DateOnly? ShutdownDate { get; set; }

        /// <summary>
        /// Время закрытия приложения.
        /// </summary>
        [Column("shutdown_time")]
        [DataType(DataType.Time)]
        [Display(Name = "Время закрытия")]
        public TimeOnly? ShutdownTime { get; set; }

        /// <summary>
        /// Название процесса приложения.
        /// </summary>
        [Column("process_name")]
        [Required(ErrorMessage = "Выберите приложение")]
        [Display(Name = "Приложение")]
        public string ProcessName { get; set; }

        /// <summary>
        /// Навигационное свойство для приложения.
        /// </summary>
        [ForeignKey("ProcessName")]
        public App? App { get; set; }

        /// <summary>
        /// Заголовок окна приложения.
        /// </summary>
        [Column("window_title")]
        [Required(ErrorMessage = "Заголовок окна обязателен")]
        [Display(Name = "Заголовок окна")]
        public string WindowTitle { get; set; }
    }
}