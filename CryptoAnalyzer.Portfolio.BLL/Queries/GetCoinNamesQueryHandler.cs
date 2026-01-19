using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;

namespace CryptoAnalyzer.Portfolio.BLL.Queries;

public record GetCoinNamesQuery : IRequest<IEnumerable<string>>;

public class GetCoinNamesQueryHandler : IRequestHandler<GetCoinNamesQuery, IEnumerable<string>>
{
    private readonly ICoinRepository _coinRepository;

    public GetCoinNamesQueryHandler(ICoinRepository coinRepository)
    {
        _coinRepository = coinRepository;
    }
    public async Task<IEnumerable<string>> Handle(GetCoinNamesQuery request, CancellationToken cancellationToken)
    {
        return await _coinRepository.GetListAsync();
    }
}