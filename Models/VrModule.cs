using System;
using System.Collections.Generic;

namespace fitapp.Models;

public partial class VrModule
{
    public int Vrid { get; set; }
    public int WorkoutID { get; set; }
    public required string Interactions { get; set; }

    public required string Environment { get; set; }

    public required Workout Workout { get; set; }
}
