﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    public class Vacancy : IVacancy
    {
        public string Page { get; set; }
        public string Town { get; set; }
        public string Description { get; set; }

        public Vacancy()
        {
                
        }

        public Vacancy(string page, string town, string description)
        {
            Page = page;
            Town = town;
            Description = description;
        }
    }
}