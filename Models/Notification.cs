namespace fitapp.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsFollowed { get; set; } = false; // لمعرفة إذا كان المستخدم يتابع الإشعار
    }
}
