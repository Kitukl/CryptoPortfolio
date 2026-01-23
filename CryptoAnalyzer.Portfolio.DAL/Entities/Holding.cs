using System.Text.Json.Serialization;

namespace CryptoAnalyzer.Portfolio.DAL.Entities;

public class Holding
{
    public Guid Id { get; set; }
    public string UserEmail { get; set; }
    [JsonInclude]
    public Coin Coin { get; set; }
    public double PricePerUnit { get; set; }
    public double Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
}