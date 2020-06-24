using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using HeadHunterParser.Exceptions;
using HeadHunterParser.Static;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;

namespace HeadHunterParser.Parse
{
    internal class HHParser : IParse
    {
        private string startPage;
        private List<(char symbol, string unicode)> unicodeTable;

        public HHParser()
        {
            unicodeTable = new List<(char symbol, string unicode)>
            {
                ('!',"%21"),
                ('"',"%22"),
                ('#',"%23"),
                ('$',"%24"),
                ('%',"%25"),
                ('&',"%26"),
                ('\'',"%27"),
                ('(',"%28"),
                (')',"%29"),
                ('*',"%30"),
                ('+',"%31"),
                (',',"%32"),
                ('-',"%33"),
                ('.',"%34"),
                ('/',"%35"),
            };
            startPage = "https://rostov.hh.ru/search/vacancy?";
        }

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
                foreach (var symbol in item)
                {
                    bool flag = false;
                    foreach (var symb in unicodeTable)
                    {
                        if (symbol == symb.symbol)
                        {
                            page += $"{symb.unicode}";
                            flag = true;
                            break;
                        }
                    }
                    if (flag)
                        continue;
                    page += $"{symbol}";
                }
                page += $"+";
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

        public async Task<Vacancy> GetVacancyAsync(string address, string townName)
        {
            var document = await BrowsingContext.New(Configuration.Default.WithDefaultLoader())
                .OpenAsync(address) ?? throw new InvalidAddressException("Неправильный адрес");

            Vacancy newVacancy = new Vacancy();
            newVacancy.Page = address;
            newVacancy.Name = document.Title;
            newVacancy.Town = townName;
            newVacancy.Requirements += $"Требуемый опыт работы: {document.QuerySelector("span[data-qa='vacancy-experience']").TextContent}\n\n";
            newVacancy.Requirements += $"{document.QuerySelector("p[data-qa='vacancy-view-employment-mode']").TextContent}\n\n";

            int countParagraphs = 0;
            foreach (var element in document.QuerySelectorAll(".g-user-content[data-qa='vacancy-description']").Children($"{null}"))
            {
                countParagraphs++;
                if (countParagraphs > 4)
                {
                    newVacancy.Description += $"Для более подробного описания загляните на сайт вакансии\n\n";
                    break;
                }
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
                    newVacancy.Description += $"{element.TextContent}\n\n";

            }
            try
            {
                newVacancy.PhoneNumber = document.QuerySelector(".vacancy-contacts__phone-mobile").Text();
            }
            catch
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
