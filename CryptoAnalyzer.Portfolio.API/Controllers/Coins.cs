using CryptoAnalyzer.Portfolio.BLL.Commands;
using CryptoAnalyzer.Portfolio.BLL.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAnalyzer.Portfolio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Coins : ControllerBase
{
    private readonly ISender _mediatr;

    public Coins(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost]
    public async Task<ActionResult> UploadCoins()
    {
        await _mediatr.Send(new UploadCoinsCommand());
        return Created();
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<string>>> GetNamesCoins()
    {
        var coins = await _mediatr.Send(new GetCoinNamesQuery());
        return Ok(coins);
    }
    
    [HttpGet("{name}")]
    public async Task<ActionResult<IEnumerable<string>>> GetCoinByName(string name)
    {
        var coin = await _mediatr.Send(new GetCoinByNameQuery(name));
        return Ok(coin);
    }
}