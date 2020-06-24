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
        public string CuurentTownName;

        public Parser(IParse mode, string town = "Москва")
        {
            _parseMode = mode;
            CuurentTownName = TownSettings.GetProperTownName(town);
        }
        public void ChangeParseTown(string newTown) => 
            CuurentTownName = TownSettings.GetProperTownName(newTown);

        /// <summary>
        /// Получает все ссылки на вакансии в пределах одного запроса
        /// </summary>
        /// <param name="stringRequest">Запрос</param>
        public async Task<IEnumerable<string>> GetJobLinksAsync(string stringRequest) =>
            await _parseMode.GetJobLinksAsync(stringRequest, CuurentTownName);

        /// <summary>
        /// Получает информацию о вакансии по ее ссылке на сайте hh.ru
        /// </summary>
        /// <param name="address">Запрос</param>
        /// <param name="page">Номер страницы</param>
        public async Task<Vacancy> GetVacancyAsync(string address) =>
            await _parseMode.GetVacancyAsync(address, CuurentTownName);

    }
}
