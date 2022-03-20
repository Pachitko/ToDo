using System;

namespace Core.Application.Features
{
    public interface IWithUserId
    {
        public Guid UserId { get; set; }
    }
}