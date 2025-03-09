namespace fitapp.Models
{
   
        public class Message
        {
            public int Id { get; set; }
            public int SenderId { get; set; } // معرف المرسل
            public int ReceiverId { get; set; } // معرف المستلم
            public string Content { get; set; } = string.Empty;
            public DateTime Timestamp { get; set; } = DateTime.UtcNow;
            public bool IsRead { get; set; } = false;
        }
    }


