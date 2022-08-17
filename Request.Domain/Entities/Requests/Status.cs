using Request.Domain.Base;
using System.Diagnostics.CodeAnalysis;

namespace Request.Domain.Entities.Requests
{
    public class Status : Entity
    {
        [AllowNull]
        public string Name { get; set; }
        [AllowNull]
        public string Description { get; set; }
    }
}