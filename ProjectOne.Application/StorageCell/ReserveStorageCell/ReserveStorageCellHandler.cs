using CSharpFunctionalExtensions;
using ProjectOne.Application.Rentalagreement;
using ProjectOne.Domain.Common;
using ProjectOne.Domain.VO;

namespace ProjectOne.Application.StorageCell.ReserveStorageCell;

public class ReserveStorageCellHandler
{
    private readonly IStorageCellRepository _storageCellRepository;
    private readonly IRentalagreementRepository _rentalagreementRepository;

    public ReserveStorageCellHandler(
        IStorageCellRepository storageCellRepository,
        IRentalagreementRepository rentalagreementRepository)
    {
        _storageCellRepository = storageCellRepository;
        _rentalagreementRepository = rentalagreementRepository;
    }

    public async Task<UnitResult<Error>> ReserveAsync(
        Guid storageId,
        Guid clientId,
        ReserveStorageCellRequest request,
        CancellationToken ct = default)
    {
        var storageIdValue = StorageId.Create(storageId);

        var cellResult = await _storageCellRepository
            .GetStorageCellByIdAsync(storageIdValue);

        if (cellResult.IsFailure)
            return cellResult.Error;

        var cell = cellResult.Value;

        // Меняем статус ячейки на 1
        var reserveResult = cell.Reserve();

        if (reserveResult.IsFailure)
            return reserveResult.Error;

        // Создаём ID аренды
        var rentalagreementId = RentalagreementId.Create(Guid.NewGuid());

        // Создаём Rentalagreement
        var rentalResult = Domain.Classes.Rentalagreement.Create(
            rentalagreementId,
            storageIdValue,
            clientId,
            request.StartDate,
            request.EndDate,
            request.TotalPrice);

        if (rentalResult.IsFailure)
            return rentalResult.Error;

        // Сохраняем изменённый статус ячейки
        await _storageCellRepository.UpdateStorageCellAsync(
            cell,
            ct);

        // Сохраняем аренду
        await _rentalagreementRepository.AddRentalagreementAsync(
            rentalResult.Value,
            ct);

        return UnitResult.Success<Error>();
    }
}
public record ReserveStorageCellRequest(
    DateTime StartDate,
    DateTime EndDate,
    decimal TotalPrice);