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
            townTuple = new List<(string TownName, int Code)>();
            townTuple.Add(("Москва", 1));
            townTuple.Add(("Санкт Петербург", 2));
            townTuple.Add(("Санкт-Петербург", 2));
            townTuple.Add(("Питер", 2));
            townTuple.Add(("СПБ", 2));
            townTuple.Add(("Екатеринбург", 3));
            townTuple.Add(("Новосибирск", 4));
            townTuple.Add(("Ростов", 76));
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
