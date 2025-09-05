using System;
using System.Collections.Generic;

namespace Velzon.Models;

public partial class AddressDetail
{
    public int AddressId { get; set; }

    public int UserId { get; set; }

    public string? PermanentAddress { get; set; }

    public string? CurrentAddress { get; set; }

    public string? City { get; set; }

    public string? StateProvince { get; set; }

    public string? Country { get; set; }

    public string? ZipPostalCode { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
