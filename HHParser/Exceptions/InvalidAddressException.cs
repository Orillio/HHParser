using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Exceptions
{
    class InvalidAddressException : Exception
    {
        public InvalidAddressException(string name) : base(name)
        {

        }
    }
}
