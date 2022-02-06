using EventSourcing.WebApi.Dtos;
using MediatR;

namespace EventSourcing.WebApi.Commands
{
    public class CreateProductCommand : IRequest
    {
        public CreateProductDto CreateProductDto { get; set; }

    }
}
