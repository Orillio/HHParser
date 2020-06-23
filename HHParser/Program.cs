using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HeadHunterParser.Parse;

namespace HeadHunterParser
{
    public class Program
    {
        public static async Task Main()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Parser parser = new Parser(new HHParser());
            try
            {
                parser.ChangeParseTown("Москва");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            var result = await parser.GetVacancyAsync("java developer", 4);

            Console.WriteLine(result);

            Console.ReadKey();
        }
        
    }
}
