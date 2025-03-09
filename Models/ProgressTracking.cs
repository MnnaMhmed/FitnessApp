using System;
using System.Collections.Generic;

namespace fitapp.Models;

public partial class ProgressTracking
{
    public int ProgressId { get; set; }

    public int? UserId { get; set; }

    public int? WorkoutId { get; set; }

    public string? Data { get; set; }

    public int? CaloriesBurned { get; set; }

    public virtual User? User { get; set; }

    public virtual Workout? Workout { get; set; }
}
