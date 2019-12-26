using System;
using System.Diagnostics.Contracts;
static class Types
{
    public static void DoTypeTest()
    {
        NullableTest();
        CharTest();
        FloatTest();
        BoolClassTest();        
        IntTest();
        
    }

    public static void NullableTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        // @TODO
    }

    public static void CharTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        const char KeyChar = 'A';

        // Converting to string
        {
            // WARNING! Cannot implicitly convert char to string!
            //string c = KeyChar;

            string c = KeyChar.ToString(); // OK
        }

        // Getting char from string
        {
            // WRONG
            //char fromString = (char)"";

            char fromString ="s"[0]; // OK
        }

        // Char equality
        {
            // No special operations for char equality, just the same!
            bool bCharZero = (KeyChar == 0); // OK
        }

        // Char operations
        {
            char UpperChar = char.ToUpper(KeyChar);
            // Used to perform conversions using localization-related rules
            char UpperInvariantChar = char.ToUpperInvariant(KeyChar);
        }

        // Extacting info from char
        {
            // Digits
            {
                bool bDigit = char.IsDigit(KeyChar);
                if (bDigit)
                {
                    // WARNING: GetNumericValue returns double
                    double DigitValue = char.GetNumericValue(KeyChar);
                }
            }

            // Letters
            {
                bool bLetter = char.IsLetter(KeyChar);
                bool bLetterOrDigit = char.IsLetterOrDigit(KeyChar);
            }

            // Whitespace
            {
                bool bWhiteSpace = char.IsWhiteSpace(KeyChar);
            }

            // Other
            {
                bool bControl = char.IsControl(KeyChar);
                bool bSymbol = char.IsSymbol(KeyChar);
            }
        }

        // Conversions to char
        {
            const int ord = 34;
            // Only explicit from int
            char C = (char)ord;

            // Ever from short we must explicitly convert
            const short short_ord = 34;
            char C2 = (char)short_ord;

            // We do can cast to char from float
            const float float_ord = 34.3F;
            char C3 = (char)float_ord;
        }

        // Conversions from char 
        {
            // Converting char to int is implicit
            {
                int CharValue = KeyChar;
                Console.WriteLine($"KeyChar={KeyChar}, CharValur={CharValue}");
            }

            // converting char to unsigned int is also implicit
            {
                uint CharValue = KeyChar;
            }

            // Converting char to float is implicit
            {
                float CharValue = KeyChar;
            }

            // WARNING: Converting char to byte is narrowing conversion!!
            {
                //byte CharValue = KeyChar;
                var CharValue = (byte)KeyChar;
            }
        }

        // Coding conversions
        {
            // Converts to UTF16
            string CharInUTF16 = char.ConvertFromUtf32(34);

            // @TODO
        }
    }

    public static void FloatTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        // @TODO
        // Is near equal?
        // Tolerance?
    }

    public static void BoolClassTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        const bool bTrueValue = true;
        const bool bFalseValue = false;

        const MyMetaClass C = null;
        // WRONG: CS0023: Unable to cast
        //const bool bNotNull = ! C;
        //const bool bNotNull = C;
        //const bool bNotNull = (bool)C;
        const bool bNotNull = C != null;

        // Trying to convert bool to int
        {
            // Compilation error: Unable to cast!!!
            //int IntFromTrueBool = (int)bTrueValue;

            int IntFrom_TrueBool = Convert.ToInt32(bTrueValue);
            Contract.Assert(IntFrom_TrueBool == 1);

            int IntFrom_FalseBool = Convert.ToInt32(bFalseValue);
            Contract.Assert(IntFrom_FalseBool == 0);
        }
    }

    public static void IntTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // Safe automatic conversions from int to floating-point
        {
            int Count = 3;
            float fCount = Count;
            double dCount = Count;
            decimal decCount = Count;
        }

        // Safe automatic conversions from int to integer
        {
            int Count = 3;
            long lCount = Count;
            // WRONG: Cannot implicitly to ulong (CS0266)
            //ulong ulCount = Count;
            ulong ulCount = checked((ulong)Count);
        }

        // Reading count from string
        {
            int count = Convert.ToInt32("   3");
            Console.WriteLine($"We read count from string: {count}");
        }

        // Reading count from wrong string
        {
            // Will cause FormatException !!!
            //int count = Convert.ToInt32(" ssdfsdf");            
        }
    }
};