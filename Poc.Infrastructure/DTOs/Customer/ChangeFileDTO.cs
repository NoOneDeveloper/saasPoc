using Microsoft.AspNetCore.Http;

namespace Poc.Infrastructure.DTOs.Customer
{
    public class ChangeFileDTO
    {
        public IFormFile FileContent { get; set; }

        public int Type { get; set; }

        public Guid CustomerId { get; set; }
    }
}
