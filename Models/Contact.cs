
    using System.ComponentModel.DataAnnotations;

namespace fitapp.Models
{
    namespace fitapp.Models
    {
        public class Contact
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string PhoneNumber { get; set; } = string.Empty;
            public string Status { get; set; } = "Available"; // حالة الاتصال
            public string ProfilePictureUrl { get; set; } = string.Empty; // صورة الملف الشخصي
        }
    }
}


