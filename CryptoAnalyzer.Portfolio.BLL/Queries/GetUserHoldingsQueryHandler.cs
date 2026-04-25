using System.Net.Http.Json;
using System.Text.Json.Nodes;
using CryptoAnalyzer.Portfolio.BLL.DTOs;
using CryptoAnalyzer.Portfolio.BLL.Exceptions;
using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;

namespace CryptoAnalyzer.Portfolio.BLL.Queries;

public record GetUserHoldingsQuery(string userEmail, int days) : IRequest<Result<IEnumerable<HoldingResponse>>>;

public class GetUserHoldingsQueryHandler : IRequestHandler<GetUserHoldingsQuery, Result<IEnumerable<HoldingResponse>>>
{
    private readonly IHoldingRepository _repository;
    private readonly HttpClient _httpClient;
    private readonly IDistributedCache _cache;

    public GetUserHoldingsQueryHandler(IHoldingRepository repository, HttpClient httpClient, IDistributedCache cache)
    {
        _repository = repository;
        _httpClient = httpClient;
        _cache = cache;
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CryptoAnalyzer");
    }

    public async Task<Result<IEnumerable<HoldingResponse>>> Handle(GetUserHoldingsQuery request, CancellationToken cancellationToken)
    {
        var holdings = (await _repository.GetListAsync(request.userEmail)).ToList();
        if (!holdings.Any()) return Result<IEnumerable<HoldingResponse>>.Failure("Not found any holdings");

        var coinIds = string.Join(",", holdings.Select(h => h.Coin.Id).Distinct());
        
        string priceCacheKey = $"prices_node:{request.userEmail}";
        JsonNode? pricesNode;
        var cachedPrices = await _cache.GetStringAsync(priceCacheKey, cancellationToken);

        if (cachedPrices is not null)
        {
            pricesNode = JsonNode.Parse(cachedPrices);
        }
        else
        {
            pricesNode = await _httpClient.GetFromJsonAsync<JsonNode>(
                $"https://api.coingecko.com/api/v3/simple/price?ids={coinIds}&vs_currencies=usd", 
                cancellationToken);

            if (pricesNode is not null)
            {
                await _cache.SetStringAsync(priceCacheKey, pricesNode.ToJsonString(), 
                    new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) }, 
                    cancellationToken);
            }
        }

        var result = new List<HoldingResponse>();

        foreach (var holding in holdings)
        {
            string predictCacheKey = $"prediction:{holding.Coin.Id}:{request.days}";
            double predictedPrice = 0;

            var cachedPredict = await _cache.GetStringAsync(predictCacheKey, cancellationToken);

            if (cachedPredict is not null)
            {
                predictedPrice = double.Parse(cachedPredict);
            }
            else
            {
                var response = await _httpClient.GetAsync(
                    $"http://localhost:5081/api/Forecast/{holding.Coin.Id}/predict/{request.days}", 
                    cancellationToken);
                
                if (response.IsSuccessStatusCode)
                {
                    var predictedRaw = await response.Content.ReadFromJsonAsync<JsonNode>(cancellationToken: cancellationToken);
                    predictedPrice = predictedRaw?["predictedPrice"]?.GetValue<double>() ?? 0;

                    if (predictedPrice > 0)
                    {
                        await _cache.SetStringAsync(predictCacheKey, predictedPrice.ToString(), 
                            new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(15) }, 
                            cancellationToken);
                    }
                }
                else
                {
                    predictedPrice = 0;
                }
            }

            var currentPrice = pricesNode?[holding.Coin.Id]?["usd"]?.GetValue<double>();

            if (currentPrice.HasValue)
            {
                double profitPercent = 0;
                double predictedProfitPercent = 0;
                double profit = 0;
                double predictedProfit = 0;

                if (holding.PricePerUnit > 0)
                {
                    profit = currentPrice.Value / holding.PricePerUnit * holding.Quantity;
                    predictedProfit = predictedPrice / holding.PricePerUnit * holding.Quantity;
                    
                    profitPercent = (currentPrice.Value - holding.PricePerUnit) / holding.PricePerUnit * 100;
                    predictedProfitPercent = (predictedPrice - holding.PricePerUnit) / holding.PricePerUnit * 100;
                }

                if (!double.IsFinite(profitPercent)) profitPercent = 0;
                if (!double.IsFinite(predictedProfitPercent)) predictedProfitPercent = 0;

                result.Add(new HoldingResponse
                {
                    Id = holding.Id,
                    UserEmail = holding.UserEmail,
                    Coin = holding.Coin,
                    PricePerUnit = holding.PricePerUnit,
                    Quantity = holding.Quantity,
                    CurrentPrice = currentPrice.Value,
                    CurrentProfitQuantity = profit,
                    PredictedProfitQuantity = predictedProfit,
                    CurrentProfit = Math.Round(profitPercent, 2),
                    CreatedAt = holding.CreatedAt,
                    PredictedPrice = predictedPrice,
                    PredictedProfit = Math.Round(predictedProfitPercent, 2)
                });
            }
        }
        return Result<IEnumerable<HoldingResponse>>.Success(result);
    }
}