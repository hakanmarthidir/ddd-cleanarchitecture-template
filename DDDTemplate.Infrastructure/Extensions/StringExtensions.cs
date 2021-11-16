using System;
namespace DDDTemplate.Infrastructure.Extensions
{
    public static class StringExtensions
    {
        public static string CleanHtml(this string input)
        {
            HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
            htmlDocument.LoadHtml(input);
            string result = htmlDocument.DocumentNode.InnerText;
            return result;
        }
    }
}
