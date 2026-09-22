using ProjectOne.Application.StorageCell.GetStorageCell;

namespace ProjectOne.Application.Rentalagreement.GetRentalagreement;

public class GetAllRentalagreementHandler
{
    private readonly IRentalagreementRepository _rentalagreementRepository;

    public GetAllRentalagreementHandler(IRentalagreementRepository rentalagreementRepository)
    {
        _rentalagreementRepository = rentalagreementRepository;
    }

    public async Task<List<GetRentalagreementResponse>> GetAllAsync(CancellationToken cancellationToken)
    {
        var rentalagreement = await _rentalagreementRepository.GetAllAsync(cancellationToken);
        
        return rentalagreement
            .Select(r => 
                new GetRentalagreementResponse(r.Id.Value, r.StorageId.Value, r.StartDate, r.EndDate, r.TotalPrice))
            .ToList();
    }
}