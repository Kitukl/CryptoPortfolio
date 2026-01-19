
namespace CryptoAnalyzer.Portfolio.BLL.DTOs;

public class CreateHoldingRequest
{
    public string CoinName { get; set; }
    public double AveragePrice { get; set; }
    public double BuyingPrice { get; set; }
}