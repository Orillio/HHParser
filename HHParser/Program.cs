using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;

namespace HHParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var page = "https://rostov.hh.ru/vacancy/37453033?query=C%23%20разработчик%20ростов";
            var parser = new HtmlParser();
            StringBuilder sb = new StringBuilder();

            var config = Configuration.Default.WithDefaultLoader();
            var document = BrowsingContext.New(config).OpenAsync(page).Result;

            var result = document.Body.QuerySelectorAll(".g-user-content");

            var strongs = result.Children("strong");

            sb.Append($"{strongs.ToList()[0].Text()}\n\n");

            foreach (var uls in result.Children("ul").ToList()[0].Children)
            {
                sb.Append(uls.Text() + "\n");
            }

            sb.Append($"\n{strongs.ToList()[1].Text()}\n\n");

            foreach (var uls in result.Children("ul").ToList()[1].Children)
            {
                sb.Append(uls.Text() + "\n");
            }

            sb.Append($"\n{strongs.ToList()[2].Text()}\n\n");

            foreach (var uls in result.Children("ul").ToList()[2].Children)
            {
                sb.Append(uls.Text() + "\n");
            }
            Console.WriteLine(sb);
            Console.ReadKey();
        }
    }
}
