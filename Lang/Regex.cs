using System;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

public static class RegexTests
{
    public static void DoAllRegexTests()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        SimpleRegexTest();
    }

    public static void SimpleRegexTest()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);


        // Matching pair of Name and Value
        // Two groups: named <Name> and <Value>
        const string RegexStr = @"^Name='(?<Name>\w+)'\s+Age=(?<Value>\d+)\z";        
        {
            const string InputString = "Name='Petrov' Age=44";


            {
                bool bMatches = Regex.IsMatch(InputString, RegexStr);
                Console.WriteLine($"Regex.IsMatch={bMatches}");
                Contract.Assert(bMatches);
            }

            {
                Match m = Regex.Match(InputString, RegexStr, RegexOptions.IgnoreCase);
                Console.WriteLine($"Regex.Match result: Success={m.Success}, Index={m.Index}, Value={m.Value}");
                Console.WriteLine($"Group count = {m.Groups.Count}");

                {
                    bool bNameGroupProvided = m.Groups.TryGetValue("Name", out Group NameGroup);
                    Contract.Assert(bNameGroupProvided);
                    Console.WriteLine($"Value={NameGroup.Value}; Index={NameGroup.Index}; Length={NameGroup.Length}; Success={NameGroup.Success}");
                }

                {
                    bool bValueGroupProvided = m.Groups.TryGetValue("Value", out Group ValueGroup);
                    Contract.Assert(bValueGroupProvided);
                    Console.WriteLine($"Value={ValueGroup.Value}; Index={ValueGroup.Index}; Length={ValueGroup.Length}; Success={ValueGroup.Success}");
                }
            }
        }

        // Checking unmatching sequence
        {
        }
    }
};
