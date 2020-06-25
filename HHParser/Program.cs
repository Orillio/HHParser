using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using HeadHunterParser.Parse;
using HeadHunterParser.Telegram;
using Telegram.Bot;

namespace HeadHunterParser
{
    public class Program
    {
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            Parser parser = new Parser(new HHParser());
            TelegramClient client = new TelegramClient("");
            Console.ReadLine();
        }
        
    }
}
