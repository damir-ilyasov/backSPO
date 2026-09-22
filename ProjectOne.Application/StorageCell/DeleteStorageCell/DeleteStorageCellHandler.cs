using CSharpFunctionalExtensions;
using ProjectOne.Domain.Common;
using ProjectOne.Domain.VO;

namespace ProjectOne.Application.StorageCell.DeleteStorageCell;

public class DeleteStorageCellHandler
{
    private readonly IStorageCellRepository _repo;

    public DeleteStorageCellHandler(IStorageCellRepository repo)
    {
        _repo = repo;
    }

    public async Task<UnitResult<Error>> DeleteAsync(
        Guid id, CancellationToken cancellationToken = default)
    {
        var storageId = StorageId.Create(id);
        
        var storageCell = await _repo.GetStorageCellByIdAsync(storageId);
        
        if (storageCell.IsFailure)
            return storageCell.Error;

        await _repo.DeleteStorageCellAsync(storageCell.Value, cancellationToken);
        
        return Result.Success<Error>();
    }
}