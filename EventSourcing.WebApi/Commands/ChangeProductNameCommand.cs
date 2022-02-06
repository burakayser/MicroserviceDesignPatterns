using EventSourcing.WebApi.Dtos;
using MediatR;

namespace EventSourcing.WebApi.Commands
{
    public class ChangeProductNameCommand : IRequest
    {
        public ChangeProductNameDto ChangeProductNameDto { get; set; }
    }
}
