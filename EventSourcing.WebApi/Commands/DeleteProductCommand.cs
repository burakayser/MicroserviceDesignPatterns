using MediatR;
using System;

namespace EventSourcing.WebApi.Commands
{
    public class DeleteProductCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}
