using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using Eto.Parse.Parsers;

namespace Eto.Parse.Samples.Markdown
{
	public struct MarkdownReference
	{
		public string Url { get; set; }
		public string Title { get; set; }
	}

	public class MarkdownReplacementArgs
	{
		Dictionary<string, MarkdownReference> references = new Dictionary<string, MarkdownReference>();
		public Dictionary<string, MarkdownReference> References { get { return references; } }
		public StringBuilder Output { get; set; }
		public MarkdownEncoding Encoding { get; set; }
		public MarkdownGrammar Grammar { get; set; }
	}

	public interface IMarkdownReplacement
	{
		string Name { get; }

		void Initialize(MarkdownGrammar grammar);

		void Transform(Match match, MarkdownReplacementArgs args);
	}
}