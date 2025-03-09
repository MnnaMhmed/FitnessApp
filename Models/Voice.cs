using System;
using System.Collections.Generic;

namespace fitapp.Models;

public partial class Voice
{
    public int VoiceId { get; set; }

    public int? WorkoutId { get; set; }

    public string? VoiceType { get; set; }

    public TimeSpan Duration { get; set; }

    public required Workout Workout { get; set; }
}
