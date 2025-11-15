using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Exceptions
{
    public sealed class BadRequestException(List<string> errors) : Exception("The request could not be understood or was missing required parameters.")
    {
        public List<string> Errors { get; } = errors;
    }
}
