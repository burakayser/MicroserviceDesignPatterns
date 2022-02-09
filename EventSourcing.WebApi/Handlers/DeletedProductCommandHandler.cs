using EventSourcing.WebApi.Commands;
using EventSourcing.WebApi.EventStores;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing.WebApi.Handlers
{
    public class DeletedProductCommandHandler : IRequestHandler<DeleteProductCommand>
    {
        private readonly ProductStream _productStream;

        public DeletedProductCommandHandler(ProductStream productStream)
        {
            _productStream = productStream;
        }

        public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            _productStream.Deleted(request.Id);

            await _productStream.SaveAsync();

            return Unit.Value;
        }
    }
}
