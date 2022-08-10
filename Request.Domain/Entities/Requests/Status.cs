using Request.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Request.Domain.Entities.Requests
{
    public class Status : Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
