using CryptoAnalyzer.Portfolio.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace CryptoAnalyzer.Portfolio.DAL.Repositories;

public interface ICoinRepository
{
    Task<IEnumerable<string>> GetListAsync();
    Task<Coin> GetByNameAsync(string name);
    Task UploadCoinsAsync(IEnumerable<Coin> coins);
}

public class CoinRepository : ICoinRepository
{
    private readonly PortfolioContext _context;

    public CoinRepository(PortfolioContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<string>> GetListAsync()
    {
        return await _context.Coins.Select(x => x.Name).ToListAsync();
    }

    public async Task<Coin> GetByNameAsync(string name)
    {
        var coin = await _context.Coins.FirstOrDefaultAsync(c => c.Name == name);
        return coin;
    }
    public async Task UploadCoinsAsync(IEnumerable<Coin> coins)
    {
        await _context.Coins.AddRangeAsync(coins);
        await _context.SaveChangesAsync();
    }
}