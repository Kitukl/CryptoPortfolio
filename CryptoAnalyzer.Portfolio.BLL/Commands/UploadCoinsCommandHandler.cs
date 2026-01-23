using System.Net.Http.Json;
using System.Text.Json;
using CryptoAnalyzer.Portfolio.BLL.Exceptions;
using CryptoAnalyzer.Portfolio.DAL.Entities;
using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;

namespace CryptoAnalyzer.Portfolio.BLL.Commands;

public record UploadCoinsCommand() : IRequest<Result<string>>;

public class UploadCoinsCommandHandler : IRequestHandler<UploadCoinsCommand, Result<string>>
{
    private readonly ICoinRepository _coinRepository;
    private readonly HttpClient _httpClient;

    public UploadCoinsCommandHandler(ICoinRepository coinRepository)
    {
        _coinRepository = coinRepository;
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoAnalyzer");
    }
    
    public async Task<Result<string>> Handle(UploadCoinsCommand request, CancellationToken cancellationToken)
        {
            var coins = await _httpClient.GetFromJsonAsync<IEnumerable<Coin>>("https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&order=market_cap_desc&per_page=100&page=1&sparkline=false&locale=en", cancellationToken);

            if (coins != null)
            {
                var distinct = coins.DistinctBy(c => c.Symbol);
                await _coinRepository.UploadCoinsAsync(distinct);
                return Result<string>.Success("Coins upload successfully");
            }

            return Result<string>.Failure("Coins fetch failed");
        }
}