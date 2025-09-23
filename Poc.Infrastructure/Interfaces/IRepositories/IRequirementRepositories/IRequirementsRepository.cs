using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.Services;

namespace Poc.Infrastructure.Interfaces.IRepositories.IRequirementRepositories
{
    public interface IRequirementsRepository
    {
        Task<Result<List<PaymentGatewayDTO>>> GetPaymentGatewaysAsync();

        Task<Result<List<InputDataResponseDTO>>> GetInputDataAsync(Guid paymentGatewayId);
    }
}
