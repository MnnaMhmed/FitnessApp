namespace fitapp.Models
{
     public class Call
        {
            public int Id { get; set; }
            public int CallerId { get; set; } // معرف المتصل
            public int ReceiverId { get; set; } // معرف المستقبل
            public DateTime CallTime { get; set; } = DateTime.UtcNow;
            public bool IsMissed { get; set; } = false;
        }
    


}
