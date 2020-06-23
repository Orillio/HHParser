using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    public interface IParse
    {
        Task<IEnumerable<string>> GetJobLinksAsync(string stringRequest, string townName);
        Task<IEnumerable<Vacancy>> GetOnePageVacanciesAsync(string stringRequest, string townName, int pageNumber);
    }
}
