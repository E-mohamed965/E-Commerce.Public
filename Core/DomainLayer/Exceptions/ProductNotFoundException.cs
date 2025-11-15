using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public class ProductNotFoundException(int id) : NotFoundException($"The Product With id:{id} is Not Fount")
    {
    }
}
