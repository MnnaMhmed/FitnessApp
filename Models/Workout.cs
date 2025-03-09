using System;
using System.Collections.Generic;

namespace fitapp.Models;

public partial class Workout
{
    public int WorkoutId { get; set; }

    public string? Name { get; set; }

    public TimeSpan Duration { get; set; }

    public required string Type { get; set; }
    public ICollection<User> Users { get; set; }

    public virtual ICollection<ProgressTracking> ProgressTrackings { get; set; } = new List<ProgressTracking>();

    public virtual ICollection<Voice> Voices { get; set; } = new List<Voice>();
}
