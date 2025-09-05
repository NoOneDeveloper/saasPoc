using System;
using System.Collections.Generic;

namespace Velzon.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Email { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<AddressDetail> AddressDetails { get; set; } = new List<AddressDetail>();

    public virtual ICollection<CompanyDocument> CompanyDocuments { get; set; } = new List<CompanyDocument>();

    public virtual ICollection<PersonalInfo> PersonalInfos { get; set; } = new List<PersonalInfo>();
}
