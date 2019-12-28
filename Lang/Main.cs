using System;
using Lang;

static class LangApp
{
    static void Main()
    {
        Console.WriteLine("Main");

        ClassHierarchyTests.DoAllTests();

        RegexTests.DoAllRegexTests();

        MyExceptionTests.DoAllTest();

        LinqTests.DoAllTests();

        ValidationTests.DoAllTests();

        StringTest.DoAllTests();

        CastTests.DoAllTests();

        EqualityTests.DoAllTests();

        DisposeTestes.DoAllTests();

        ComparisonTests.DoAllTests();

        DicionaryTests.DoTests();

        GeneritTests.DoGenericTests();

	    InterfaceTests.DoAllTests();

        DelegateTests.DoAllDelegateTests();

        TestStatementExtra.TestAll();

	    RefTests.DoAllRefTests();

        ClassOps.TestClassOps();

        MethodTest.TestMethod();

        StructTest.TestStruct();

        ArrayTestClass.TestArray();

        Types.DoTypeTest();

        Enums.TestFlagEnum();
        Enums.TestEnum();
        MyMetaClass.ClassTest();
        Output.StringInterpolation();
        Output.FormatFloat();

        Console.WriteLine("Press any key...");
        Console.ReadLine();
    }
};
