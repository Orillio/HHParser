using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using HeadHunterParser.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Parse
{
    public class Parser
    {
        private IParse _parseMode;
        private string _townName;

        public Parser(IParse mode, string town = "Ростов")
        {
            _parseMode = mode;
            _townName = TownSettings.GetProperTownName(town);
        }
        public void ChangeParseTown(string newTown) => 
            _townName = TownSettings.GetProperTownName(newTown);

        /// <summary>
        /// Получает все ссылки на вакансии в пределах одного запроса
        /// </summary>
        /// <param name="stringRequest">Запрос</param>
        public async Task<IEnumerable<string>> GetJobLinksAsync(string stringRequest) =>
            await _parseMode.GetJobLinksAsync(stringRequest, _townName);

        /// <summary>
        /// Получает информацию о вакансии по ее номеру в поиске HH.ru
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="page">Номер страницы</param>
        public async Task<Vacancy> GetVacancyAsync(string request, int page) =>
            await _parseMode.GetVacancyAsync(request, _townName, page);

    }
}
