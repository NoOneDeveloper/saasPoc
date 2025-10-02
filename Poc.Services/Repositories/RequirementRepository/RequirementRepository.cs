using Microsoft.EntityFrameworkCore;
using Poc.Common.StaticClasses;
using Poc.EF.Context;
using Poc.EF.Entities;
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

        #region get fields for custoerm flow by payment gateway id
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
                    PaymentGatewayId = paymentGatewayId
                })
                .ToListAsync();

            return result;

        }

        #endregion

        #region Add customer Fields for the flow
        public async Task<Result<string>> AddCustomerFlowAsync(Guid loggedUser, List<InputDataResponseDTO> input)
        {
            var flowID = BusinessManager.GenerateFlowId();
            try
            {
                var entities = input.Select(x => new CustomerFlow
                {
                    Id = Guid.NewGuid(),
                    CustomerId = loggedUser,
                    PaymentGatewayId = x.PaymentGatewayId,
                    FieldName = x.FieldName, // handle nulls safely
                    FieldType = x.FieldType,
                    IsRequired = x.IsRequired ?? false, // if null => false
                    Description = x.Description,
                    OrderIndex = x.OrderIndex ?? 0, // default to 0 if null
                    GroupName = x.GroupName,
                    FlowId = flowID,
                    CreatedDate = DateTime.UtcNow,
                }).ToList();

                await _db.AddRangeAsync(entities);
                await _db.SaveChangesAsync();

                return new Result<string>
                {
                    Data = "Customer flow added successfully"
                };
            }
            catch (Exception ex) 
            {
                return new Result<string>
                {

                    Success = false,
                    Message = "Customer flow added successfully"
                };
            }

        }

        #endregion

        #region Get customer flow by customer id

        public async Task<Result<List<InputDataResponseDTO>>> GetCustomerFlowAsync(Guid loggedUser)
        {
            var result = new Result<List<InputDataResponseDTO>>();

            result.Data = await _db.CustomerFlows
                .AsNoTracking()
                .Where(a => a.CustomerId == loggedUser)
                .Select(a => new InputDataResponseDTO
                {
                    Id = a.Id,
                    FieldName = a.FieldName,
                    FieldType = a.FieldType,
                    IsRequired = a.IsRequired,
                    Description = a.Description,
                    OrderIndex = a.OrderIndex,
                    GroupName = a.GroupName,
                    PaymentGatewayId = a.PaymentGatewayId
                })
                .ToListAsync();

            return result;
        }
        #endregion
    }
}
