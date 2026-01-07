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
        [Key]
        [Column("view_id")]
        public int ViewId { get; set; }

        [Column("activation_date")]
        [Display(Name = "Дата начала")]
        [DataType(DataType.Date)]
        public DateOnly ActivationDate { get; set; }

        [Column("activation_time")]
        [Display(Name = "Время начала")]
        [DataType(DataType.Time)]
        public TimeOnly ActivationTime { get; set; }

        [Column("shutdown_date")]
        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        public DateOnly? ShutdownDate { get; set; }

        [Column("shutdown_time")]
        [Display(Name = "Время окончания")]
        [DataType(DataType.Time)]
        public TimeOnly? ShutdownTime { get; set; }

        [Column("viewing_time")]
        [Display(Name = "Время просмотра (сек)")]
        public int? ViewingTime { get; set; }

        // --- СВЯЗЬ 1: Видео (YoutubeVideo) ---

        [Column("video_id")]
        [Required]
        [Display(Name = "ID Видео")]
        public string VideoId { get; set; } // Внешний ключ
        [ForeignKey("VideoId")]

        [Display(Name = "Видео")]
        public YoutubeVideo? Video { get; set; } // Навигационное свойство

        // --- СВЯЗЬ 2: Сайт (Domain) ---

        [Column("domain")]
        [Required]
        [Display(Name = "Домен")]
        public string Domain { get; set; } // Внешний ключ

        [ForeignKey("Domain")]
        [Display(Name = "Платформа")]
        public Site? Site { get; set; } // Навигационное свойство
    }
}