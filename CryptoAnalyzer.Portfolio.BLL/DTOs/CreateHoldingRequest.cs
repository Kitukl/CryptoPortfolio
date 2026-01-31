
namespace CryptoAnalyzer.Portfolio.BLL.DTOs;

public class CreateHoldingRequest
{
    public string CoinName { get; set; }
    public double PricePerUnit { get; set; }
    public double Quantity { get; set; }
}