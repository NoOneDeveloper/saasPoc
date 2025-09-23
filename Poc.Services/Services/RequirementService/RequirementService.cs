using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.Requirement;
using Poc.Infrastructure.DTOs.Services;
using Poc.Infrastructure.Interfaces.IRepositories.IRequirementRepositories;
using Poc.Infrastructure.Interfaces.IServices.IRequirementServices;

namespace Poc.Implementation.Services.RequirementService
{
    public class RequirementService(IRequirementsRepository repo) : IRequirementService
    {
        private readonly IRequirementsRepository _repo = repo;

        #region get payment gateways
        public async Task<Result<ServicesRequestDTO>> GetPaymentGateways()
        {
            var result = new Result<ServicesRequestDTO>
            {
                Data = new ServicesRequestDTO()
            };

            var paymentGatewayRequest = await _repo.GetPaymentGatewaysAsync();

            if (!paymentGatewayRequest.Success)
            {
                result.Success = false;
                result.Message = "Failed to fetch payment gateways";
            }
            else
            {
                result.Data.PaymentGatewayDTO = paymentGatewayRequest.Data;
            }

            return result;
        }

        #endregion

        #region get input data by payment gateway id
        public async Task<Result<List<InputDataResponseDTO>>> GetInputData(Guid paymentGatewayId)
        {
            var result = new Result<List<InputDataResponseDTO>>();

            var inputDataRequest = await _repo.GetInputDataAsync(paymentGatewayId);
            if (!inputDataRequest.Success || inputDataRequest.Data == null ||!inputDataRequest.Data.Any())
            {
                result.Success = false;
                result.Message = "No Input data found";
            }
            else
            {
                result.Data = inputDataRequest.Data;
            }
            return result;
        }
        #endregion
    }
}
