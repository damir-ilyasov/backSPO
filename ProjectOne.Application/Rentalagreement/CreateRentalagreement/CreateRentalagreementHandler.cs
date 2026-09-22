using CSharpFunctionalExtensions;
using ProjectOne.Application.StorageCell;
using ProjectOne.Domain.Common;
using ProjectOne.Domain.VO;

namespace ProjectOne.Application.Rentalagreement.CreateRentalagreement;

public class CreateRentalagreementHandler
{
    private readonly IStorageCellRepository _storageCellRepository;
    private readonly IRentalagreementRepository _rentalagreementRepository;

    public CreateRentalagreementHandler(
        IStorageCellRepository storageCellRepository,
        IRentalagreementRepository rentalagreementRepository)
    {
        _storageCellRepository = storageCellRepository;
        _rentalagreementRepository = rentalagreementRepository;
    }
    
    
    public async Task<Result<Guid, Error>> CreateAsync(
        CreateRentalagreementRequest createRentalagreementRequest,
        Guid currentUserId,
        CancellationToken cancellationToken = default)
    {
        var rentalagreementId = RentalagreementId.NewRentalagreementId();

        var storageId = StorageId.Create(createRentalagreementRequest.storageId);

        var storageCell = await _storageCellRepository.GetStorageCellByIdAsync
            (storageId);
        
        if (storageCell.IsFailure)
            return storageCell.Error;

        var rentalagreement = Domain.Classes.Rentalagreement.Create(
            rentalagreementId,
            storageId,
            currentUserId,
            createRentalagreementRequest.startDate,
            createRentalagreementRequest.endDate,
            createRentalagreementRequest.totalPrice);
        
        if (rentalagreement.IsFailure)
            return rentalagreement.Error;
        
        await _rentalagreementRepository.AddRentalagreementAsync(rentalagreement.Value, cancellationToken);
        return rentalagreementId.Value;
    }
}