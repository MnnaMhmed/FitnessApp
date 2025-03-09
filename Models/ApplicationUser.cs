using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using fitapp.Models;
namespace fitapp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}


