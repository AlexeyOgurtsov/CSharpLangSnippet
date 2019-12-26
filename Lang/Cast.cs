using System;
using System.Diagnostics.Contracts;

public static class CastTests
{
    public static void DoAllTests()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Test_CastNull();
    }

    public static void Test_CastNull()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // To-class casting
        {
            // OK, works fine
            var C = (MyMetaClass)null;
        }

        // To-struct casting
        {
            // Compile-time error
            //var s = (MyVecStruct)null;
        }
    }
};