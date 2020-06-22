using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using AngleSharp.Io;

namespace HHParser
{
    class HHParser : IParser
    {
        public HtmlParser Parser { get; set; }

        readonly string page = "https://rostov.hh.ru/search/vacancy?";
        private IConfiguration _config;
        private IDocument _document;

        /// <param name="sourcePage">Ссылка на страницу</param>
        public HHParser(string sourcePage)
        {
            Parser = new HtmlParser();
            page = sourcePage;
            _config = Configuration.Default.WithDefaultLoader();
            _document = BrowsingContext.New(_config).OpenAsync(page).Result;
            
        }
        public void SetConfig(IConfiguration config) => _config = config;

        //public async IEnumerable<string> GetJobs(string request)
        //{

        //}
    }
}
