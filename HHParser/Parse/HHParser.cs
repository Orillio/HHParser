using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HeadHunterParser.Static;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HeadHunterParser.Parse
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
            try { towncode = TownSettings.GetTownCode(townName); }
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

        public async Task<Vacancy> GetVacancyAsync(string stringRequest, string townName, int vacancyNumber)
        {
            vacancyNumber--; // 
            if (request != stringRequest)
            {
                request = stringRequest;
                var r = await GetJobLinksAsync(stringRequest, townName);
                links = r.ToList();
            }
            var document = await BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(links[vacancyNumber]);
            Vacancy newVacancy = new Vacancy();
            newVacancy.Page = links[vacancyNumber];
            newVacancy.Name = document.Title;
            newVacancy.Town = townName;
            newVacancy.Requirements += $"Требуемый опыт работы: {document.QuerySelector("span[data-qa='vacancy-experience']").TextContent}\n\n";
            newVacancy.Requirements += $"{document.QuerySelector("p[data-qa='vacancy-view-employment-mode']").TextContent}\n\n";

            foreach (var element in document.QuerySelectorAll(".g-user-content[data-qa='vacancy-description']").Children($"{null}"))
            {
                if (element.NodeName.ToLower() == "ul")
                {
                    int count = 1;
                    foreach (var li in element.Children)
                    {
                        newVacancy.Description += $"{count++}) - {li.TextContent}\n";
                    }
                    newVacancy.Description += "\n";
                }
                else
                {
                    newVacancy.Description += $"{element.TextContent}\n\n";
                }

            }
            try
            {
                newVacancy.PhoneNumber = document.QuerySelector(".vacancy-contacts__phone-mobile").Text();
            }
            catch (System.Exception)
            {
                newVacancy.PhoneNumber = null;
            }

            var contactNodes = document.QuerySelectorAll(".vacancy-contacts__body");

            foreach (var node in contactNodes.Children($"[data-qa^='vacancy-contacts']"))
            {
                if (node.ClassName == "vacancy-contacts__phone-desktop")
                    continue;
                newVacancy.Contacts += $"{node.Text()}\n";
            }
            return newVacancy;

        }

    }
}
