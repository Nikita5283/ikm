using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Класс, описывающий справочник видео.
    /// </summary>
    [Table("videos")]
    public class Video
    {
        [Key]
        [Column("video_id")]
        [Required]
        public string VideoId { get; set; }

        [Column("title")]
        [Required(ErrorMessage = "Название видео обязательно")]
        [Display(Name = "Название видео")]
        public string Title { get; set; }

        [Column("autor")]
        [Required(ErrorMessage = "Автор обязателен")]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Column("video_duration")]
        [Required(ErrorMessage = "Длительность видео обязательна")]
        [Display(Name = "Длительность (сек)")]
        public int? VideoDuration { get; set; }

        // Навигационные свойства для связей
        public ICollection<ViewSession>? ViewSessions { get; set; }
    }
}