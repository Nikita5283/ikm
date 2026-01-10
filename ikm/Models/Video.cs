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
        /// <summary>
        /// id видео из url (например, для https://www.youtube.com/watch?v=dQw4w9WgXcQ id = dQw4w9WgXcQ).
        /// </summary>
        [Key]
        [Column("video_id")]
        [Required]
        public string VideoId { get; set; }

        /// <summary>
        /// Название видео.
        /// </summary>
        [Column("title")]
        [Required(ErrorMessage = "Название видео обязательно")]
        [Display(Name = "Название видео")]
        public string Title { get; set; }

        /// <summary>
        /// Автор видео.
        /// </summary>
        [Column("autor")]
        [Required(ErrorMessage = "Автор обязателен")]
        [Display(Name = "Автор")]
        public string Author { get; set; }

        /// <summary>
        /// Длительность видео в секундах.
        /// </summary>
        [Column("video_duration")]
        [Required(ErrorMessage = "Длительность видео обязательна")]
        [Display(Name = "Длительность (сек)")]
        public int? VideoDuration { get; set; }

        /// <summary>
        /// Навигационное свойство для связи с сессиями просмотров.
        /// </summary>
        public ICollection<ViewSession>? ViewSessions { get; set; }
    }
}