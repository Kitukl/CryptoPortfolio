using System.Net.Http.Json;
using System.Text.Json.Nodes;
using CryptoAnalyzer.Portfolio.BLL.DTOs;
using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;

namespace CryptoAnalyzer.Portfolio.BLL.Queries;

public record GetUserHoldingsQuery(string userEmail) : IRequest<IEnumerable<HoldingResponse>>;

public class GetUserHoldingsQueryHandler : IRequestHandler<GetUserHoldingsQuery, IEnumerable<HoldingResponse>>
{
    private readonly IHoldingRepository _repository;
    private readonly HttpClient _httpClient;

    public GetUserHoldingsQueryHandler(IHoldingRepository repository, HttpClient httpClient)
    {
        _repository = repository;
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoAnalyzer");
    }

    public async Task<IEnumerable<HoldingResponse>> Handle(GetUserHoldingsQuery request, CancellationToken cancellationToken)
    {
        var holdings = (await _repository.GetListAsync(request.userEmail)).ToList();
        if (!holdings.Any()) return Enumerable.Empty<HoldingResponse>();

        var coinIds = string.Join(",", holdings.Select(h => h.Coin.Id).Distinct());
        var pricesNode = await _httpClient.GetFromJsonAsync<JsonNode>($"https://api.coingecko.com/api/v3/simple/price?ids={coinIds}&vs_currencies=usd", cancellationToken);

        var result = new List<HoldingResponse>();

        foreach (var holding in holdings)
        {
            var currentPrice = pricesNode?[holding.Coin.Id]?["usd"]?.GetValue<double>();

            if (currentPrice.HasValue)
            {
                double profitPercent = 0;
                if (holding.AveragePrice > 0)
                {
                    profitPercent = (currentPrice.Value - holding.AveragePrice) / holding.AveragePrice * 100;
                }

                if (!double.IsFinite(profitPercent))
                {
                    profitPercent = 0;
                }

                result.Add(new HoldingResponse
                {
                    Id = holding.Id,
                    UserEmail = holding.UserEmail,
                    Coin = holding.Coin,
                    AveragePrice = holding.AveragePrice,
                    CurrentPrice = currentPrice.Value,
                    CurrentProfit = Math.Round(profitPercent, 2),
                    CreatedAt = holding.CreatedAt
                });
            }
        }
        return result;
    }
}