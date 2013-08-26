using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using Eto.Parse.Parsers;

namespace Eto.Parse.Samples.Markdown.Encodings
{
	public class BoldEncoding : SequenceParser, IMarkdownReplacement
	{
		public bool AddLinesBefore { get { return true; } }

		public BoldEncoding()
		{
			Name = "bold";
		}

		public void Initialize(MarkdownGrammar grammar)
		{
			Add("**", new RepeatParser(1).Until("**"), "**");
		}

#if PERF_TEST
		protected override ParseMatch InnerParse(ParseArgs args)
		{
			return base.InnerParse(args);
		}
#endif

		public void Replace(Match match, MarkdownReplacementArgs args)
		{
			args.Output.Append("<strong>");
			var text = match.Text;
			args.Encoding.Replace(text.Substring(2, text.Length - 4) , args);
			args.Output.Append("</strong>");
		}
	}
}