using CryptoAnalyzer.Portfolio.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Portfolio.DAL.Repositories;

public interface IHoldingRepository
{
    Task<IEnumerable<Holding>> GetListAsync(string userEmail);
    Task<string> CreateAsync(string userId, Coin coin, double averagePrice, double buyingPrice);
    Task<string> UpdateAsync(Guid id, Coin coin, double averagePrice, double buyingPrice);
    Task<string> DeleteAsync(Guid id);
}

public class HoldingRepository : IHoldingRepository
{
    private readonly PortfolioContext _context;

    public HoldingRepository(PortfolioContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Holding>> GetListAsync(string userEmail)
    {
       return await _context.Holdings
           .Where(h => h.UserEmail == userEmail)
           .Include(h => h.Coin)
           .ToListAsync();
    }

    public async Task<string> CreateAsync(string userEmail, Coin coin, double pricePerUnit, double quantity)
    {
        var holding = new Holding
        {
            Id = new Guid(),
            Coin = coin,
            UserEmail = userEmail,
            PricePerUnit = pricePerUnit,
            Quantity = quantity,
            CreatedAt = DateTime.UtcNow
        };
        await _context.Holdings.AddAsync(holding);
        await _context.SaveChangesAsync();

        return holding.Id.ToString();
    }

    public async Task<string> UpdateAsync(Guid id, Coin coin, double pricePerUnit, double quantity)
    {
        var holding = await _context.Holdings.FirstOrDefaultAsync(h => h.Id == id);
    
        if (holding != null)
        {
            holding.PricePerUnit = pricePerUnit;
            holding.Quantity = quantity;
            holding.Coin = coin;
            await _context.SaveChangesAsync();
        }

        return id.ToString();
    }

    public async Task<string> DeleteAsync(Guid id)
    {
        await _context.Holdings
            .Where(h => h.Id == id)
            .ExecuteDeleteAsync();

        await _context.SaveChangesAsync();

        return id.ToString();
    }
}