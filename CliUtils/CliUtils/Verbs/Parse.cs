using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AngleSharp.Parser.Html;

namespace CliUtils.Verbs
{
	public static class Parse
	{
		private static readonly HttpClient HttpClient
			= new HttpClient {BaseAddress = new Uri("https://www.adventofcode.com/")};

		private static readonly IDictionary<string, Func<string, string>> FuncMap =
			new Dictionary<string, Func<string, string>>
			{
				{ "H2", ParseTags.Header },
				{ "P", ParseTags.Paragraph },
				{ "CODE", ParseTags.Code },
				{ "EM",ParseTags.Emphasis },
				{ "UL", ParseTags.UnorderedList},
				{ "A", ParseTags.Link },
				{ "SPAN", ParseTags.Span },
				{ "PRE", ParseTags.Pre }
			};

		private static string HandleNode(IElement node)
		{
			var res = FuncMap[node.TagName](node.OuterHtml);
			var middlewareTags = new [] {"SPAN", "A", "EM", "CODE"};
			res = middlewareTags.Aggregate(res, (current, tag) => FuncMap[tag](current));
			return res;
		}
		
		public static async Task<string> GetHtml(int year, int day)
		{
			return await HttpClient
				.GetStringAsync($"{year}/day/{day}");
		}
		
		public static async Task<string> HtmlToMd(string html)
		{
			var parser = new HtmlParser();
			var document = await parser.ParseAsync(html);
			var article = document.QuerySelector("article");
			var sb = new StringBuilder();
			foreach (var child in article.Children)
			{
				sb.Append(HandleNode(child));
				sb.Append(Environment.NewLine);
			}
			return sb.ToString();
		} 
	}
}