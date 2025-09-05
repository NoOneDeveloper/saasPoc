using System;
using System.Collections.Generic;

namespace Velzon.Models;

public partial class PersonalInfo
{
    public int PersonalInfoId { get; set; }

    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? Nationality { get; set; }

    public string? EmailAddress { get; set; }

    public string? ParentName { get; set; }

    public string? ContactNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
