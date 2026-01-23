using CryptoAnalyzer.Portfolio.BLL.Exceptions;
using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;

namespace CryptoAnalyzer.Portfolio.BLL.Commands;

public record CreateHoldingCommand(string userEmail, string coinName, double averagePrice, double buyingPrice) : IRequest<Result<string>>;

public class CreateHoldingCommandHanlder : IRequestHandler<CreateHoldingCommand, Result<string>>
{
    private readonly ICoinRepository _coinRepository;
    private readonly IHoldingRepository _holdingRepository;

    public CreateHoldingCommandHanlder(ICoinRepository coinRepository, IHoldingRepository holdingRepository)
    {
        _coinRepository = coinRepository;
        _holdingRepository = holdingRepository;
    }
    public async Task<Result<string>> Handle(CreateHoldingCommand request, CancellationToken cancellationToken)
    {
        var coin = await _coinRepository.GetByNameAsync(request.coinName);
        if (coin == null) return Result<string>.Failure("Coin with this name not found");
        return Result<string>.Success(await _holdingRepository.CreateAsync(request.userEmail, coin, request.averagePrice, request.buyingPrice));
    }
}