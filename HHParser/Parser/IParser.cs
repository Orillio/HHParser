﻿using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    interface IParser
    {
        HtmlParser Parser { get; set; }
    }
}