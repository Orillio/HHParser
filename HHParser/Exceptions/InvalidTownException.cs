using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Exceptions
{
    class InvalidTownException : Exception
    {
        public InvalidTownException(string ex) : base(ex)
        {

        }
    }
}
