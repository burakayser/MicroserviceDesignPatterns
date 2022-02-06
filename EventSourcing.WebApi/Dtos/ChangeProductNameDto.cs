using System;

namespace EventSourcing.WebApi.Dtos
{
    public class ChangeProductNameDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }

        public string Name { get; set; }

    }
}
