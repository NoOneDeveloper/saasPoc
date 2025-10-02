using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.Services;

namespace Poc.Infrastructure.Interfaces.IRepositories.IRequirementRepositories
{
    public interface IRequirementsRepository
    {
        Task<Result<List<PaymentGatewayDTO>>> GetPaymentGatewaysAsync();

        Task<Result<List<InputDataResponseDTO>>> GetInputDataAsync(Guid paymentGatewayId);

        Task<Result<string>> AddCustomerFlowAsync(Guid loggedUser, List<InputDataResponseDTO> input);

        Task<Result<List<InputDataResponseDTO>>> GetCustomerFlowAsync(Guid loggedUser);
    }
}
