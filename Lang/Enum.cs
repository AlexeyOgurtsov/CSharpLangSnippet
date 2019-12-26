using System;

// By default int is used for enum
public enum ESeason
{
    Summer,
	Spring,
	Autumn,
};

//namespace SeasonExt
//{
    public static class SeasonExtMethods
    {
        // We cannot define operations INSIDE enum, only EXT methods (see this)!
        public static bool IsGreen(this ESeason Season)
        {
            return Season == ESeason.Summer;
        }
    };
//} // SeasonExt
//using SeasonExt;

[Flags]
enum EMyFlags
{
    None                = 0,//= 0b_0000_0000,
	LogTime             = 1 << 0,//= 0b_0000_0001,
	LogFunctions        = 1 << 1,//= 0b_0000_0010,
};

static class Enums
{
    public static void TestFlagEnum()
    {
        Console.WriteLine("TestFlagEnum");

        // Parsing flags enum from comma-separated list of EMyFlags-members
        {
            Console.WriteLine("Parsing comma-separated list of EMyFlags-members");
            string InputString = $"{nameof(EMyFlags.None)},{nameof(EMyFlags.LogTime)},{nameof(EMyFlags.LogTime)},\t {nameof(EMyFlags.LogFunctions)}";
            //string InputString = "   \t1";// Parses well
            //string InputString = $"1, 0"; // Returns false trying to parse it!            
            EMyFlags MyFlags = 0;
            if (Enum.TryParse<EMyFlags>(InputString, out MyFlags))
            {
                Console.WriteLine($"MyFlags={MyFlags}");
            }
            else
            {
                Console.WriteLine("Unable to parse MyFlags from string");
            }
        }
    }
    public static void TestEnum()
    {        
        Console.WriteLine("TestEnum");
        const ESeason Season = ESeason.Spring;

        {
            Console.WriteLine($"Type \"{nameof(ESeason)}\" contains {Enum.GetValues(typeof(ESeason)).Length} values");
            Console.WriteLine($"Value of {Season} is {(int)Season}");
            Console.WriteLine($"IsGreen {Season} = {Season.IsGreen()}");
        }

        // Converting to enum, passing string (CS0030!)
        {
            // CS0030!
            //ESeason NewSeason = (ESeason)"Spring";
            //Console.WriteLine($"NewSeason={NewSeason} (value={(int)NewSeason})");
        }


        // Converting to enum, passing wrong value:
        // NEVER fails - OK!!!    
        {
            Console.WriteLine("Converting to enum, passing wrong value");
            ESeason NewSeason = (ESeason)98;
            Console.WriteLine($"NewSeason={NewSeason} (value={(int)NewSeason})");
        }

        // Converting to ESeason enum
        {            
            ESeason NewSeason = 0;
            const int InputValue = 1;
            if (Enum.IsDefined(typeof(ESeason), InputValue))
            {
                NewSeason = (ESeason)InputValue;
            }
            else
            {
                Console.WriteLine("Unable to convert the input value to season");
                NewSeason = 0;
            }
            Console.WriteLine($"NewSeason={NewSeason} (value={(int)NewSeason})");
        }



        // Trying to parse the ESeason from value string
        {
            Console.WriteLine("Parsing from value");
            ESeason NewSeason = 0;
            bool bParsed = Enum.TryParse<ESeason>("1", out NewSeason);
            if (bParsed)
            {
                Console.WriteLine($"NewSeason={NewSeason} (value={(int)NewSeason})");
            }
            else
            {
                Console.WriteLine($"Unable to parse {nameof(ESeason)} from value string");
            }
        }

        // Trying to parse the ESeason from value string
        // WARNING!!! 
        // - Case -sensitive!!! 
        // - Fails if unable to cast!!!
        {
            Console.WriteLine("Parsing from string");
            ESeason NewSeason = 0;
            bool bParsed = Enum.TryParse<ESeason>("summer", out NewSeason);
            if (bParsed)
            {
                Console.WriteLine($"NewSeason={NewSeason} (value={(int)NewSeason})");
            }
            else
            {
                Console.WriteLine($"Unable to parse {nameof(ESeason)} from value string");
            }
        }

        // Trying to parse the ESeason from WRONG value string
        // RESULT: returns false
        {
            Console.WriteLine("Parsing from WRONG string");
            ESeason NewSeason = 0;
            bool bParsed = Enum.TryParse<ESeason>("WRONG_VALUE", out NewSeason);
            if (bParsed)
            {
                Console.WriteLine($"NewSeason={NewSeason} (value={(int)NewSeason})");
            }
            else
            {
                Console.WriteLine($"Unable to parse {nameof(ESeason)} from value string");
            }
        }

        // Trying to parse the ESeason from OUT-of-range VALUE
        // RESULT: returns TRUE always!!!
        {
            Console.WriteLine("Parsing from WRONG string");
            ESeason NewSeason = 0;
            bool bParsed = Enum.TryParse<ESeason>("63", out NewSeason);
            if (bParsed)
            {
                Console.WriteLine($"NewSeason={NewSeason} (value={(int)NewSeason})");
            }
            else
            {
                Console.WriteLine($"Unable to parse {nameof(ESeason)} from value string");
            }
        }
    }
}

