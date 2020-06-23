using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    interface IVacancy
    {
        string Name { get; set; }
        string Page { get; set; }
        string Town { get; set; }
        string Description { get; set; }
        string ContactNumber { get; set; }
    }
}
