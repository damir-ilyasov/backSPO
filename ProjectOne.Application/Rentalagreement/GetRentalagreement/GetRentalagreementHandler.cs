using CSharpFunctionalExtensions;
using ProjectOne.Domain.Common;
using ProjectOne.Domain.VO;

namespace ProjectOne.Application.Rentalagreement.GetRentalagreement;

public class GetRentalagreementHandler
{
    private readonly IRentalagreementRepository _repository;
    
    public GetRentalagreementHandler(IRentalagreementRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<GetRentalagreementResponse, Error>> GetAsync(
        Guid id, CancellationToken cancellationToken)
    {
        var storageId = RentalagreementId.Create(id);
        
        var storageCell = await _repository.GetRentalagreementByIdAsync(storageId);

        if(storageCell.IsFailure)
            return storageCell.Error;
        
        var s = storageCell.Value;
        
        return new GetRentalagreementResponse(s.Id.Value, s.StorageId.Value, s.StartDate, s.EndDate,
            s.TotalPrice);
    }
}

public record GetRentalagreementResponse(
    Guid id,
    Guid storageId,
    DateTime startDate,
    DateTime endDate,
    decimal totalPrice);