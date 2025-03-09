using System;
using System.Collections.Generic;

namespace fitapp.Models;

public partial class DietPlan
{
    public int DietPlanId { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public int? Calories { get; set; }
    public required ICollection<User> Users { get; set; }

}
