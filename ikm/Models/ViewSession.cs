using ikm.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Класс, описывающий сессию просмотра видео на YouTube.
    /// Связывает просмотр с конкретным видео и доменом.
    /// </summary>
    [Table("views_sessions")]
    public class ViewSession
    {
        /// <summary>
        /// id сессии просмотра.
        /// </summary>
        [Key]
        [Column("view_id")]
        public int ViewId { get; set; }

        /// <summary>
        /// Дата начала просмотра.
        /// </summary>
        [Column("activation_date")]
        [Required(ErrorMessage = "Дата начала обязательна")]
        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateOnly? ActivationDate { get; set; }

        /// <summary>
        /// Время начала просмотра.
        /// </summary>
        [Column("activation_time")]
        [Required(ErrorMessage = "Время начала обязательно")]
        [Display(Name = "Время начала")]
        [DataType(DataType.Time)]
        public TimeOnly? ActivationTime { get; set; }

        /// <summary>
        /// Дата окончания просмотра.
        /// </summary>
        [Column("shutdown_date")]
        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDate { get; set; }

        /// <summary>
        /// Время окончания просмотра.
        /// </summary>
        [Column("shutdown_time")]
        [Display(Name = "Время окончания")]
        [DataType(DataType.Time)]
        public TimeOnly? ShutdownTime { get; set; }

        /// <summary>
        /// Время просмотра в секундах.
        /// </summary>
        [Column("viewing_time")]
        [Display(Name = "Время просмотра (сек)")]
        public int? ViewingTime { get; set; }

        /// <summary>
        /// id видео.
        /// </summary>
        [Column("video_id")]
        [Required(ErrorMessage = "Выберите видео")]
        [Display(Name = "ID Видео")]
        public string VideoId { get; set; } // Внешний ключ
        [ForeignKey("VideoId")]

        /// <summary>
        /// Навигационное свойство для связи со справочником видео.
        /// </summary>
        [Display(Name = "Видео")]
        public Video? Video { get; set; } // Навигационное свойство

        /// <summary>
        /// Домен сайта.
        /// </summary>
        [Column("domain")]
        [Required(ErrorMessage = "Выберите домен")]
        [Display(Name = "Домен")]
        public string Domain { get; set; } // Внешний ключ

        /// <summary>
        /// Навигационное свойство для связи со справочником сайтов.
        /// </summary>
        [ForeignKey("Domain")]
        [Display(Name = "Платформа")]
        public Site? Site { get; set; } // Навигационное свойство
    }
}