using CSharpFunctionalExtensions;
using ProjectOne.Domain.Common;
using ProjectOne.Domain.VO;

namespace ProjectOne.Application.StorageCell.ReserveStorageCell;

public class ReserveStorageCellHandler
{
    private readonly IStorageCellRepository _repo;
    public ReserveStorageCellHandler(IStorageCellRepository repo) => _repo = repo;

    public async Task<UnitResult<Error>> ReserveAsync(Guid storageId, CancellationToken ct = default)
    {
        var id = StorageId.Create(storageId);
        var cellResult = await _repo.GetStorageCellByIdAsync(id);
        if (cellResult.IsFailure) return cellResult.Error;

        var reserveResult = cellResult.Value.Reserve();
        if (reserveResult.IsFailure) return reserveResult.Error;

        await _repo.UpdateStorageCellAsync(cellResult.Value, ct);
        return UnitResult.Success<Error>();
    }
}