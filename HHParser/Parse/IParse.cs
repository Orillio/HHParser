using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Parse
{
    public interface IParse
    {
        Task<IEnumerable<string>> GetJobLinksAsync(string stringRequest, string townName);
        Task<Vacancy> GetVacancyAsync(string stringRequest, string townName, int vacancyNumber);
    }
}
