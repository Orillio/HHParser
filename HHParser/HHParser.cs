using AngleSharp;
using AngleSharp.Html.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    internal class HHParser : IParse
    {
        private string startPage;

        public HHParser()
        {
            startPage = "https://rostov.hh.ru/search/vacancy?";
        }
        static string request;
        List<string> links;
        /// <summary>
        /// Получает все ссылки на вакансии в пределах одного запроса
        /// </summary>
        /// <param name="request">Запрос</param>
        /// <param name="townName">Город</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetJobLinksAsync(string request, string townName)
        {
            int towncode = -1;
            try { towncode = TownCode.GetTownCode(townName); }
            catch { return null; }

            string page = startPage;

            var splittedText = request.Split(' ');
            page += $"area={towncode}&";
            page += $"text=";

            foreach (var item in splittedText)
            {
                page += $"{item}+";
            }

            List<string> allLinks = new List<string>();

            for (int i = 0; i < int.MaxValue; i++)
            { 
                var newpage = page + $"&page={i}";
                var _document = await BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                    .OpenAsync(newpage);
                var linkElements = _document.QuerySelectorAll("[href][data-qa='vacancy-serp__vacancy-title']");
                var result = linkElements.Cast<IHtmlAnchorElement>().Select(x => x.Href);
                if (result.Count() == 0) break;
                allLinks.AddRange(result);
            }
            return allLinks;
        }

        /// <summary>
        /// Получает информацию о вакансиях в пределах одной страницы
        /// </summary>
        /// <param name="stringRequest">Запрос</param>
        /// <param name="townName">Город</param>
        /// <param name="pageNumber">Номер страницы</param>
        /// <returns></returns>
        public async Task<IEnumerable<Vacancy>> GetOnePageVacanciesAsync(string stringRequest, string townName, int pageNumber)
        {
            pageNumber--; // 
            if (request != stringRequest)
            {
                request = stringRequest;
                var r = await GetJobLinksAsync(stringRequest, townName);
                links = r.ToList();
            }

            for (int i = pageNumber * 50; i < (pageNumber + 1) * 50; i++)
            {
                //var document = await BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                //    .OpenAsync(links[i]);
                Console.WriteLine(links[i]);
            }
            return null;
        }
    }
}
