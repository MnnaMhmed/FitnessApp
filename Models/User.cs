using System;
using System.Collections.Generic;

namespace fitapp.Models;

public partial class User
{
    public int UserId { get; set; }

    public  string FirstName { get; set; }
    public required string PasswordHash { get; set; }
    public  string? OTP { get; set; }

    public string? MidName { get; set; }

    public  string? LastName { get; set; }
    public string Email { get; set; } = null!;
   public bool? IsEmailConfirmed { get; set; }

    public int? Age { get; set; }

    public double? Weight { get; set; }

    public double? Height { get; set; }
    public Boolean EmailConfirmationToken { get; set; }

   // public  string? EmailConfirmationToken { get; set; }
    public string? Gender { get; set; }
    public ICollection<Workout>? Workouts { get; set; }

    public virtual ICollection<Gamification> Gamifications { get; set; } = new List<Gamification>();
    
    public ICollection<DietPlan>? DietPlans { get; set; }

    public virtual ICollection<ProgressTracking> ProgressTrackings { get; set; } = new List<ProgressTracking>();
}
