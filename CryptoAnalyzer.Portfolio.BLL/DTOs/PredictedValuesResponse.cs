namespace CryptoAnalyzer.Portfolio.BLL.DTOs;

public class PredictedValuesResponse
{ 
    public string CoinId { get; set; } 
    public decimal? PredictedPrice { get; set; }
}