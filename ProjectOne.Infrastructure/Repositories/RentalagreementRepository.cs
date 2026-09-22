using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using ProjectOne.Application.Rentalagreement;
using ProjectOne.Domain.Classes;
using ProjectOne.Domain.Common;
using ProjectOne.Domain.VO;

namespace ProjectOne.Infrastructure.Repositories;

public class RentalagreementRepository : IRentalagreementRepository
{
    
    private readonly ApplicationDbContext _dbContext;
    
    public RentalagreementRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task AddRentalagreementAsync(Rentalagreement rentalagreement, CancellationToken cancellationToken = default)
    {
        await _dbContext.Rentalagreements.AddAsync(rentalagreement, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<Result<Rentalagreement, Error>> GetRentalagreementByIdAsync(RentalagreementId id)
    {
        var rentalagreement = await _dbContext.Rentalagreements.FirstOrDefaultAsync(r => r.Id == id);

        if (rentalagreement is null)
            return Errors.General.NotFound();
        
        return rentalagreement;
    }

    public async Task DeleteRentalagreementAsync(Rentalagreement rentalagreement,
        CancellationToken cancellationToken = default)
    {
        _dbContext.Rentalagreements.Remove(rentalagreement);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Rentalagreement>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Rentalagreements.ToListAsync(cancellationToken);
    }

    public async Task UpdateRentalagreementAsync(Rentalagreement rentalagreement,
        CancellationToken cancellationToken = default)
    {
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}