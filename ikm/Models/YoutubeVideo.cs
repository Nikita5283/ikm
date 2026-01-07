using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ikm.Models
{
    /// <summary>
    /// Класс, описывающий видео с YouTube.
    /// </summary>
    [Table("youtube_videos")]
    public class YoutubeVideo
    {
        [Key]
        [Column("video_id")]
        [Required]
        public string VideoId { get; set; }

        [Column("title")]
        [Required]
        [Display(Name = "Название видео")]
        public string Title { get; set; }

        [Column("autor")]
        [Required]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        [Column("video_duration")]
        [Display(Name = "Длительность (сек)")]
        public int VideoDuration { get; set; }

        // Навигационные свойства для связей
        public ICollection<ViewSession>? ViewSessions { get; set; }
    }
}