using HeadHunterParser.Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace HeadHunterParser.Telegram
{
    public class TelegramUser 
    {

        public long ChatId { get; set; }
        public string Name { get; set; }
        public List<Vacancy> FavouriteVacancies { get; set; }

        public List<(int MessageId, int Page)> MessageTuple { get; set; }

        public List<string> ReturnedVacancies { get; set; }
        public Parser Parser;
        public List<Message> Messages { get; set; }

        public TelegramUser(IParse mode, string name, long chatid)
        {
            Name = name;
            ChatId = chatid;
            FavouriteVacancies = new List<Vacancy>();
            Parser = new Parser(mode);
            Messages = new List<Message>();
            MessageTuple = new List<(int MessageId, int Page)>();
        }
    }
}
