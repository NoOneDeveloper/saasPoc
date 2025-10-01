namespace Poc.Infrastructure.DTOs.Services
{
    public class InputDataResponseDTO
    {
        public Guid Id { get; set; }

        public string FieldName { get; set; }

        public string FieldType { get; set; }

        public bool? IsRequired { get; set; }

        public string Description { get; set; }

        public int? OrderIndex { get; set; }

        public Guid PaymentGatewayId { get; set; }

        public bool IsSelected { get; set; }

        public string GroupName { get; set; }

    }
}
