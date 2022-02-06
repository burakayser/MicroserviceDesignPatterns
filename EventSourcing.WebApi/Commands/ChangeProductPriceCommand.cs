using EventSourcing.WebApi.Dtos;
using MediatR;

namespace EventSourcing.WebApi.Commands
{
    public class ChangeProductPriceCommand : IRequest
    {
        public ChangeProductPriceDto ChangeProductPriceDto { get; set; }
    }
}
