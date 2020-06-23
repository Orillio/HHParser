using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadHunterParser.Parse
{
    public class Vacancy
    {
        public string Page { get; set; }
        public string Name { get; set; }
        public string Town { get; set; }
        public string Requirements { get; set; }
        public string Description { get; set; }
        public string Contacts { get; set; }
        public string PhoneNumber { get; set; }

        

        public Vacancy()
        {
        }

        public override string ToString()
        {
            if (Page == null || Name == null || Town == null || Requirements == null || Description == null)
                return "Не все поля вакансии заполнены";
            return $"Вакансия: {Name}\n\nГород: {Town}\n\nУсловия:\n\n{Requirements}Описание:\n\n{Description}" +
                $"Контакты:\n{(Contacts == null ? "Нет. Перейдите на сайт чтобы оставить отклик на вакансию" : Contacts)}" +
                $"\n\n{(PhoneNumber == null ? string.Empty : PhoneNumber)}";
                
        }
    }
}
