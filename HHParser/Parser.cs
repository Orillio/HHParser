using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    public class Parser
    {
        private IParse _parseMode;

        private string _townName;
        public IEnumerable<Vacancy> ReturnedVacancies { get; private set; }

        public Parser(IParse mode, string town = "Ростов")
        {
            _parseMode = mode;
            _townName = town;
        }
        public void ChangeParseTown(string newTown) => _townName = newTown;

        public async Task<IEnumerable<string>> GetJobLinksAsync(string stringRequest) =>
            await _parseMode.GetJobLinksAsync(stringRequest, _townName);
        public async Task<IEnumerable<Vacancy>> GetOnePageVacanciesAsync(string request, int page) =>
            await _parseMode.GetOnePageVacanciesAsync(request, _townName, page);

    }
}
