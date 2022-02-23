using EventSourcing.WebApi.Dtos;
using MediatR;
using System.Collections.Generic;

namespace EventSourcing.WebApi.Queries
{
    public class GetProductAllListByUserId : IRequest<List<ProductDto>>
    {
        public int UserId { get; set; }
    }
}
