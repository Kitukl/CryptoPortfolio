using CryptoAnalyzer.Portfolio.BLL.Exceptions;
using CryptoAnalyzer.Portfolio.DAL.Entities;
using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;

namespace CryptoAnalyzer.Portfolio.BLL.Commands;

public record UpdateHoldingCommand(Guid id, string coinName, double averagePrice, double buyingPrice) : IRequest<Result<string>>;

public class UpdateHoldingCommandHandler : IRequestHandler<UpdateHoldingCommand, Result<string>>
{
    private readonly IHoldingRepository _holdingRepository;
    private readonly ICoinRepository _coinRepository;

    public UpdateHoldingCommandHandler(IHoldingRepository holdingRepository, ICoinRepository coinRepository)
    {
        _holdingRepository = holdingRepository;
        _coinRepository = coinRepository;
    }
    public async Task<Result<string>> Handle(UpdateHoldingCommand request, CancellationToken cancellationToken)
    {
        var coin = await _coinRepository.GetByNameAsync(request.coinName);
        if (coin == null) return Result<string>.Failure("Coin with this name not found");
        return Result<string>.Success(await _holdingRepository.UpdateAsync(request.id, coin, request.averagePrice, request.buyingPrice));
    }
}