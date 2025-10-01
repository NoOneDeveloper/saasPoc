using Microsoft.EntityFrameworkCore;
using Poc.EF.Context;
using Poc.Infrastructure.DTOs.Global;
using Poc.Infrastructure.DTOs.Services;
using Poc.Infrastructure.Interfaces.IRepositories.IRequirementRepositories;

namespace Poc.Implementation.Repositories.RequirementRepository
{
    public class RequirementRepository(ApplicationDBContext db) : IRequirementsRepository
    {
        private readonly ApplicationDBContext _db = db;

        #region get all payment gateways

        public async Task<Result<List<PaymentGatewayDTO>>> GetPaymentGatewaysAsync()
        {
            var result = new Result<List<PaymentGatewayDTO>>();
            try
            {
                result.Data = await _db.PaymentGateways
                    .Select(pg => new PaymentGatewayDTO
                    {
                        Id = pg.Id,
                        Name = pg.GatewayName
                    })
                    .ToListAsync();

                result.Data = result.Data ?? new List<PaymentGatewayDTO>();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = $"An error occurred while fetching payment gateways: {ex.Message}";
            }

            return result;
        }

        #endregion

        #region
        public async Task<Result<List<InputDataResponseDTO>>> GetInputDataAsync(Guid paymentGatewayId)
        {
            var result = new Result<List<InputDataResponseDTO>>();

            result.Data = await _db.InputData
                .AsNoTracking()
                .Where(a => a.PaymentGatewayId == paymentGatewayId)
                .Select(a => new InputDataResponseDTO
                {
                    Id = a.Id,
                    FieldName = a.FieldName,
                    FieldType = a.FieldType,
                    IsRequired = a.IsRequired,
                    Description = a.Description,
                    OrderIndex = a.OrderIndex,
                    GroupName = a.GroupName,
                })
                .ToListAsync();

            return result;

        }

        #endregion
    }
}
