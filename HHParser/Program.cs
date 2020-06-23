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
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            HHParser ParseMode = new HHParser();

            Parser parser = new Parser(ParseMode, "Ростов");
            parser.ChangeParseTown("Москва");

            var result = await parser.GetOnePageVacanciesAsync("c разработчик", 2);

            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            //Console.WriteLine(sb);
            Console.ReadKey();
        }
        
    }
}
