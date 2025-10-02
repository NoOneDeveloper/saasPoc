using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.Requirement;
using Poc.Infrastructure.DTOs.Services;

namespace Poc.Infrastructure.Interfaces.IServices.IRequirementServices
{
    public interface IRequirementService
    {
        Task<Result<ServicesRequestDTO>> GetPaymentGateways();
        Task<Result<List<InputDataResponseDTO>>> GetInputData(Guid paymentGatewayId);
        Task<Result<string>> AddCustomerFlow(Guid loggedUser, List<InputDataResponseDTO> input);

        Task<Result<List<InputDataResponseDTO>>> GetCustomerFlow(Guid loggedUser);


    }
}
