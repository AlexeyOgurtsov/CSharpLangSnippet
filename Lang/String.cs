using System;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;
public static class StringTest
{
    public static void DoAllTests()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        RegexTest();
        ConstructionTest();
        ComparisonTest();
        SubstringTest();
        StringBuilderTest();
        JoinTest();
        InternTest();
        PadAndTrimTest();
        CompareOrdinalTest();
    }

    public static void InternTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        string MyString = "MyString";
        string Interned = string.IsInterned(MyString);
        string InternedMyString = string.Intern(MyString);
    }

    public static void PadAndTrimTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        {
            char[] CharsToTrim = new char[] { ';', 'T' };
            const string StringToTrimFrom = ";TEST;";
            string TrimmedString = StringToTrimFrom.Trim(CharsToTrim);
            Contract.Assert(TrimmedString == StringToTrimFrom.TrimStart(CharsToTrim).TrimEnd(CharsToTrim));
            Contract.Assert(!TrimmedString.EndsWith(";"));
            Contract.Assert(!TrimmedString.StartsWith(";"));
        }

        {
            // Trim with a single char argument
            string TrimmedString = ";TEST;".Trim(';');
            // Wrong: cannot convert string into char!
            //string TrimmedString2 = ";TEST;".Trim("S;");
        }
    }

    public static void CompareOrdinalTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }

    public static void ConstructionTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

    }

    public static void ComparisonTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        const string S = "abc";
        bool bCompareIgnoreCase = (0 == string.Compare(S.ToUpper(), S, true));
        Contract.Assert(bCompareIgnoreCase);
    }

    public static void SubstringTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        const string S_START = "Start_";
        const string S_MIDDLE = "Middle";
        const string S_END = "_End";

        const string S = S_START + S_MIDDLE + S_END;
        Contract.Assert(S.StartsWith(S_START));
        Contract.Assert(S.Contains(S_MIDDLE));
        Contract.Assert(S.EndsWith(S_END));

        {
            int IndexOf_S_START = S.IndexOf(S_START);
            int IndexOf_S_MIDDLE = S.IndexOf(S_MIDDLE);
            int IndexOf_S_END = S.IndexOf(S_END);

            Contract.Assert(S.Substring(IndexOf_S_MIDDLE, S_MIDDLE.Length) == S_MIDDLE);
        }
    }

    public static void StringBuilderTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var Dict = new System.Collections.Generic.Dictionary<int, string>();
        Dict.Add(1, "A");
        Dict.Add(2, "B");
        Dict.Add(3, "c");

        // WRONG!!!! StringBuilder is not IDisposable!!!
        //using (System.Text.StringBuilder sb = new System.Text.StringBuilder()) {}
        const int InitialCapacity = 64;
        System.Text.StringBuilder sb = new System.Text.StringBuilder(InitialCapacity);
        foreach (System.Collections.Generic.KeyValuePair<int, string> KV in Dict)
        {
            sb.AppendLine($"{{ Key={KV.Key} Value={KV.Value} }}");
        }
        
        Console.WriteLine($"Result sb: {sb}");
        string ResultS = sb.ToString();
    }

    public static void RegexTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        const string InputString = @"Text: abcd, Count: 234";

	// WARNING
	// @ - Verbatim (before the string)
	
	// Regex elements:
	// https://docs.microsoft.com/ru-ru/dotnet/standard/base-types/regular-expression-language-quick-reference

	// Regex character class construction
	const string CHAR_CLASS_DIGIT = @"\d";
	const string CHAR_CLASS__NOT__DIGIT = @"\D";
	const string CHAR_CLASS_ANY_GIVEN_CHARS = @"[abc]";
	const string CHAR_CLASS_ALL_CHARS_EXCEPT_GIVEN = @"[^_$]";
	const string CHAR_CLASS_RANGE = @"[a-zA-Z]";

	// Regex character classes
	const string CHAR_CLASS_ANY_EXCEPT_NEWLINE = @".";
	const string REGEX_MATCH_POINT = @"\.";
	const string CHAR_CLASS_ALPHA = @"\w";
	const string CHAR_CLASS_WHITESPACE = @"\s";

	// Regex anchors
	// In MULTILINE Start of line
	const string REGEX_STRING_START_ANCHOR=@"^";
	const string REGEX_STRICT_END_OF_STRING=@"\z";

	// Regex grouping constructs
	// Capture identifier and index separately
	const string REGEX_GROUPING_CAPTURE_PART=@"(\s{5})(\d{3})";
	const string REGEX_GROUPING_NAMED_CAPTURE_PART=@"(?<NAME>\s{5})(?<INDEX>\d{3})";

	// Regex quantifiers
	// From zero to any chars
	const string REGEX_QUANTIFIER_ANY_COUNT=@"\w*";
	const string REGEX_QUANTIFIER_ONE_OR_MORE=@"\d+";
	// Digit optionally preceded by an unary minu
	const string REGEX_QUANTIFIER_OPTIONAL=@"-?\d";

	// Regex backreference constructs
	//
	// Regex alternation constructs
	// Matches is or as
	const string REGEX_ALTERNATIVE = @"(is|as)";

	// Test expressions	
	const string REGEX_NUMBER = @"\d";
	const string REGEX_MOBILE_PHONE = @"\d-\d{3}-\d{3}-\d{3}-\d";

        {
            // @TODO: How to include System.Text.RegularExpressions?

	    MatchCollection Matches = Regex.Matches(InputString, REGEX_NUMBER, RegexOptions.IgnorePatternWhitespace);
	    Console.WriteLine($"Matches.Count={Matches.Count}");
	    foreach(Match m in Matches)
	    {
		    // @TODO: Output groups
	    }
        }
    }

    public static void JoinTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var arr = new string[] { "float", "int" };
        string joined_arr = string.Join(";", arr);
        Console.WriteLine($"Joined_arr={joined_arr}");
    }
};
