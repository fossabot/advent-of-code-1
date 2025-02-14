using System.Text.RegularExpressions;

namespace CliUtils.Executors
{
	public static class ParseTags
	{
		public static string Header(string html)
		{
			return $"# {Regex.Replace(html, @"^(<h2.*?>)|(<\/h2>)$", string.Empty)}";
		}
		
		public static string Paragraph(string html)
		{
			return Regex.Replace(html, @"(<p>)|(<\/p>)", string.Empty);
		}
		
		public static string Code(string html)
		{
			return Regex.Replace(html, @"(<code.*?>)|(<\/code>)", "```");
		}
		
		public static string Emphasis(string html)
		{
			return Regex.Replace(html, "(<em(>|\\sclass=\"star\">))|(<\\/em>)", "*");
		}
		
		public static string UnorderedList(string html)
		{
			return Regex.Replace(html, @"(<ul>)|(<\/ul>)", string.Empty)
				.Replace(@"<li>", "* ")
				.Replace("</li>", string.Empty);
		}
		
		public static string Link(string html)
		{
			if (!Regex.IsMatch(html, @"(<a(.*))|(</a>)"))
				return html;
			
			var href = Regex.Replace(html, "((.*)<a(.*)href=\")|(\"(.*)a>(.*))", string.Empty);
			var res = Regex.Replace(html, "(<a(.*?)>)", "[")
				.Replace("</a>", $"]({href})");
			return res;
		}
		
		public static string Span(string html)
		{
			if (!Regex.IsMatch(html, @"(<span(.*))|(</span>)"))
				return html;

			var title = Regex.Replace(html.Trim('\n'), "((.*)<span(.*)title=\")|(\"(.*)span>(.*))", string.Empty);
			var res = Regex.Replace(html, "(<span(.*?)>)", string.Empty)
				.Replace("</span>", $"({title})");
			return res;
		}
		
		public static string Pre(string html)
		{
			return Regex.Replace(html, @"(<pre(.*?)>)|(<\/pre>)", "\n");
		}
	}
}