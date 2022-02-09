using EventSourcing.WebApi.Commands;
using EventSourcing.WebApi.EventStores;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace EventSourcing.WebApi.Handlers
{
    public class ChangeProductNameCommandHandler : IRequestHandler<ChangeProductNameCommand>
    {
        private readonly ProductStream _productStream;

        public ChangeProductNameCommandHandler(ProductStream productStream)
        {
            _productStream = productStream;
        }

        public async Task<Unit> Handle(ChangeProductNameCommand request, CancellationToken cancellationToken)
        {
            _productStream.NameChanged(request.ChangeProductNameDto);
            await _productStream.SaveAsync();

            return Unit.Value;
        }
    }
}
