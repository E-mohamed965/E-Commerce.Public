using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public sealed class BasketNotFoundException (string Id) : NotFoundException($"Baske with Id = {Id} Not Found")
    {

    }
}
