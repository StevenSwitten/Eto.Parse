using System;
using System.Linq;
using Eto.Parse.Parsers;
using Eto.Parse.Testers;

namespace Eto.Parse
{
	public static class FluentParserExtensions
	{
		public static Parser Then(this Parser parser, params Parser[] parsers)
		{
			var sequence = parser as SequenceParser;
			if (sequence == null || !sequence.Reusable)
				sequence = new SequenceParser(parser) { Reusable = true };
			sequence.Items.AddRange(parsers);
			return sequence;
		}

		public static T Inverse<T>(this T parser)
			where T: Parser, IInverseParser
		{
			parser.Inverse = !parser.Inverse;
			return parser;
		}

		public static Parser Not(this Parser parser)
		{
			return new LookAheadParser(parser);
		}

		public static Parser Not(this Parser parser, Parser inner)
		{
			return new SequenceParser(parser, new LookAheadParser(inner));
		}

		public static RepeatParser Repeat(this Parser parser, int minimum = 1, int maximum = Int32.MaxValue)
		{
			return new RepeatParser(parser, minimum, maximum);
		}

		public static RepeatParser Until(this RepeatParser parser, Parser until)
		{
			parser.Until = until;
			return parser;
		}

		public static Parser Optional(this Parser parser)
		{
			return new OptionalParser(parser);
		}

		public static Parser Or(this Parser left, Parser right)
		{
			var alternative = left as AlternativeParser;
			if (alternative != null && alternative.Reusable)
			{
				alternative.Items.Add(right);
				return alternative;
			}
			alternative = right as AlternativeParser;
			if (alternative != null && alternative.Reusable)
			{
				alternative.Items.Insert(0, left);
				return alternative;
			}
			return new AlternativeParser(left, right) { Reusable = true };
		}

		public static NonTerminalParser NonTerminal(this Parser parser, string id, Action<NonTerminalMatch> matched = null, Action<NonTerminalMatch> preMatch = null)
		{
			var namedParser = new NonTerminalParser(id ?? Guid.NewGuid().ToString(), parser);
			if (matched != null)
				namedParser.Matched += match => {
					matched(match);
				};
			if (preMatch != null)
				namedParser.PreMatch += match => {
					preMatch(match);
				};
			return namedParser;
		}

		public static CharParser Include(this CharParser parser, CharParser include)
		{
			return new CharParser(new IncludeTester(parser.Tester, parser.Inverse, include.Tester, include.Inverse));
		}

		public static CharParser Include(this CharParser parser, params char[] include)
		{
			return new CharParser(new IncludeTester(parser.Tester, parser.Inverse, new CharSetTester(include), false));
		}

		public static CharParser Exclude(this CharParser include, CharParser exclude)
		{
			return new CharParser(new ExcludeTester(include.Tester, include.Inverse, exclude.Tester, exclude.Inverse));
		}

		public static CharParser Exclude(this CharParser include, params char[] exclude)
		{
			return new CharParser(new ExcludeTester(include.Tester, include.Inverse, new CharSetTester(exclude), false));
		}

		public static T Separate<T>(this T parser)
			where T: Parser
		{
			parser.Reusable = false;
			return parser;
		}
	}
}

