using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HHParser
{
    public class HHParser : IParser
    {
        public HtmlParser Parser { get; set; }

        readonly string page = "https://rostov.hh.ru/search/vacancy?";
        private IConfiguration _config;
        private IDocument _document;
        private string townName;

        public HHParser(string sourcePage, string town = "Ростов")
        {
            Parser = new HtmlParser();
            page = sourcePage;
            townName = town;
            _config = Configuration.Default.WithDefaultLoader();
        }
        public void ChangeTown(string newTown) => townName = newTown;
        public void SetConfig(IConfiguration config) => _config = config;

        //public async Task<IEnumerable<dynamic>> GetJobs(string stringRequest)
        //{
        //    _document = await BrowsingContext.New(_config).OpenAsync(page);

        //}
    }
}
