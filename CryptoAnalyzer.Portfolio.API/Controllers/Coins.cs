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
        var result = await _mediatr.Send(new UploadCoinsCommand());
        if (!result.isSuccess)
        {
            return BadRequest(result.Errors);
        }
        return Ok(result.Value);
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