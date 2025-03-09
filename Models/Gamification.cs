using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace fitapp.Models;

public partial class Gamification
{
    public int GameId { get; set; }

    public int? UserId { get; set; }

    public string? BadgesEarned { get; set; }

    public int? Points { get; set; }

    [ForeignKey("UserID")]
    public required User User { get; set; }
}
