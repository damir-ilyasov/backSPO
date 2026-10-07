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
        
        var rentalagreement = await _repository.GetRentalagreementByIdAsync(storageId);

        if (rentalagreement.IsFailure)
            return rentalagreement.Error;

        var r = rentalagreement.Value;

        return new GetRentalagreementResponse(
            r.Id.Value,
            r.StorageId.Value,
            r.StorageCell.NumberStorageCalls,
            r.StorageCell.Floor,
            r.StorageCell.Price,
            r.StartDate,
            r.EndDate,
            r.TotalPrice);;
    }
}

public record GetRentalagreementResponse(
    Guid id,
    Guid storageId,
    string numberStorageCalls,
    int? floor,
    decimal price,
    DateTime startDate,
    DateTime endDate,
    decimal totalPrice);