using HeadHunterParser.Telegram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.StaticClasses
{
    public static class Extensions
    {
        public static bool UserExists(this List<TelegramUser> users, long chatid) =>
            users.FirstOrDefault(x => x.ChatId == chatid) != null;

        public static TelegramUser FindUser(this List<TelegramUser> users, long chatid) =>
            users.FirstOrDefault(x => x.ChatId == chatid);
    }
}
