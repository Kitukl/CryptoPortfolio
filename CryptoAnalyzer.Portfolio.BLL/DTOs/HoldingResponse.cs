using CryptoAnalyzer.Portfolio.DAL.Entities;

namespace CryptoAnalyzer.Portfolio.BLL.DTOs;

public class HoldingResponse
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    public Coin Coin { get; set; }
    public double AveragePrice { get; set; }
    public double? CurrentPrice { get; set; }
    public double? CurrentProfit { get; set; }
    public double? PredictedPrice { get; set; }
    public double? PredictedProfit { get; set; }
    public DateTime CreatedAt { get; set; }
}