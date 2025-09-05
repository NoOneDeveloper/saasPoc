using System;
using System.Collections.Generic;

namespace Velzon.Models;

public partial class CompanyDocument
{
    public int DocumentId { get; set; }

    public int UserId { get; set; }

    public string? CertificateOfIncorporation { get; set; }

    public string? MemorandumAndArticles { get; set; }

    public string? TaxIdentificationNumber { get; set; }

    public string? ShareholdingStructure { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual User User { get; set; } = null!;
}
