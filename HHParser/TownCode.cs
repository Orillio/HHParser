using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    public static class TownCode
    {
        private static List<(string TownName, int Code)> townTuple;

        static TownCode()
        {
            townTuple = new List<(string TownName, int Code)>();
            townTuple.Add(("Москва", 1));
            townTuple.Add(("Санкт Петербург", 2));
            townTuple.Add(("Екатеринбург", 3));
            townTuple.Add(("Новосибирск", 4));
            townTuple.Add(("Ростов", 76));
        }

        public static int GetTownCode(string townName)
        {
            var e = townTuple.Where(x => x.TownName.ToLower().Contains(townName.ToLower()));
            if (e.Count() == 0)
                throw new ArgumentException("Введено неверное название города или этот город еще недоступен для парсинга");
            if (e.Count() > 1)
                return -1;
            return e.First().Code;
        }
    }
}
