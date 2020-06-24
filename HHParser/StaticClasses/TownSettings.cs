using HeadHunterParser.Exceptions;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Static
{
    public static class TownSettings
    {
        private static List<(string TownName, int Code)> townTuple;

        static TownSettings()
        {
            townTuple = new List<(string TownName, int Code)>()
            {
                ("Москва", 1),
                ("Санкт Петербург", 2),
                ("Санкт-Петербург", 2),
                ("Питер", 2),
                ("Екатеринбург", 3),
                ("Новосибирск", 4),
                ("Нижний Новгород", 66),
                ("Нижний-Новгород", 66),
                ("Казань", 88),
                ("Челябинск", 104),
                ("Омск", 68),
                ("Самара", 78),
                ("Пермь", 72),
                ("Воронеж", 26),
                ("Волгоград", 24),
            };
        }

        public static int GetTownCode(string townName) =>
            townTuple.Where(x => x.TownName.ToLower().Contains(townName.ToLower())).First().Code;

        public static string GetProperTownName(string townName)
        {
            var e = townTuple.Where(x => x.TownName.ToLower().Contains(townName.ToLower()));
            if (e.Count() == 0)
                throw new InvalidTownException("Введено неверное название города или этот город еще недоступен для парсинга");
            else if(e.Count() > 1)
                throw new InvalidTownException("Введите более четкое название города, так как оно конфликтует с названиями других городов");
            return e.First().TownName;
        }
    }
}
