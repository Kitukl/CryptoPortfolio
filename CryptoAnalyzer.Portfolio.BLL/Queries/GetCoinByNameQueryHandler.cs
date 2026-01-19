using CryptoAnalyzer.Portfolio.DAL;
using CryptoAnalyzer.Portfolio.DAL.Entities;
using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;

namespace CryptoAnalyzer.Portfolio.BLL.Queries;

public record GetCoinByNameQuery(string Name) : IRequest<Coin>;

public class GetCoinByNameQueryHandler : IRequestHandler<GetCoinByNameQuery, Coin>
{
    private readonly ICoinRepository _repository;

    public GetCoinByNameQueryHandler(ICoinRepository repository)
    {
        _repository = repository;
    }
    public async Task<Coin> Handle(GetCoinByNameQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByNameAsync(request.Name);
    }
}