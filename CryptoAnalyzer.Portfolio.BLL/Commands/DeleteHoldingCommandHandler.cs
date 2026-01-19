using CryptoAnalyzer.Portfolio.DAL.Repositories;
using MediatR;

namespace CryptoAnalyzer.Portfolio.BLL.Commands;

public record DeleteHoldingCommand(Guid id) : IRequest<string>;

public class DeleteHoldingCommandHandler : IRequestHandler<DeleteHoldingCommand, string>
{
    private readonly IHoldingRepository _repository;

    public DeleteHoldingCommandHandler(IHoldingRepository repository)
    {
        _repository = repository;
    }
    public async Task<string> Handle(DeleteHoldingCommand request, CancellationToken cancellationToken)
    {
        return await _repository.DeleteAsync(request.id);
    }
}