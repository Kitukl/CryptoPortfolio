using System.Security.Claims;
using CryptoAnalyzer.Portfolio.BLL.Commands;
using CryptoAnalyzer.Portfolio.BLL.DTOs;
using CryptoAnalyzer.Portfolio.BLL.Queries;
using CryptoAnalyzer.Portfolio.DAL.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CryptoAnalyzer.Portfolio.Controllers;

[ApiController]
[Route("api/[controller]")]
public class Holdings : ControllerBase
{
    private readonly ISender _mediatr;

    public Holdings(ISender mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateHolding([FromBody] CreateHoldingRequest request)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return Unauthorized("User email not provided in claims");
        }

        var result = await _mediatr.Send(new CreateHoldingCommand(userEmail, request.CoinName, request.PricePerUnit, request.Quantity));

        if (!result.isSuccess)
        {
            return NotFound(result.Errors);
        }
        
        return Ok(result.Value);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<string>> UpdateHolding([FromRoute] Guid id, [FromBody] UpdateHoldingRequest request)
    {
        return Ok(await _mediatr.Send(new UpdateHoldingCommand(id, request.CoinName, request.PricePerUnit, request.Quantity)));
    }

    [HttpGet("{days}")]
    public async Task<ActionResult<IEnumerable<Holding>>> GetList(int days)
    {
        var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return Unauthorized("User email not provided in claims");
        }

        var result = await _mediatr.Send(new GetUserHoldingsQuery(userEmail, days));

        if (!result.isSuccess)
        {
            return NotFound(result.Errors);
        }
        
        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(Guid id)
    {
        return Ok(await _mediatr.Send(new DeleteHoldingCommand(id)));
    }
}