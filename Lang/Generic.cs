using System;
using System.Diagnostics.Contracts;

public interface IMyGenericHelper
{
    string GetHelperString();
}

public interface IMyGenericHelperTwo
{
    string GetHelperStringTwo();
};
public class MyMathHelper<T> : IMyGenericHelper, IMyGenericHelperTwo
    where T : struct
{
    public string GetHelperString() { return "Helper"; }
    public string GetHelperStringTwo() { return "HelperTwo"; }
}

public class MyIdBase<T>
{
    // default(T) - returns default value for the given generic type
    public T Id { get; } = default(T);
}

// We can use many constraints on the type
public class MyMathHelper<HelperT, MathType> 
    where HelperT : IMyGenericHelper, IMyGenericHelperTwo
    where MathType : struct // all other constraints must go AFTER struct and class constraints
{
}

public class MyConcreteMathHelper<HelperT, MathType>
    where MathType : struct // we MUST set it because the MyMathHelper required the MathType to be struct!
    where HelperT : MyMathHelper<MathType>
{ 
}

// WRONG!!!
//public class MyFloatMathHelper<HelperT, float> // WRONG
//{ 
//}

public class MyFactory<T> where T : new()
{
    public T Create()
    {
        // CS0304 - if we do NOT add new new() constraint for the class univeral param!
        return new T();
    }
}

public class MyPair<U, V>
{
    public U a;
    public V b;
};

// Helper for defining ids
public class MyClassIdValue
{
    int Id { get; set; }
};

// Using Generic and extension methods!
public static class GeneritTests
{
    public static void DoGenericTests()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        TestCastingsToT();
        DoSimpleTest();
        DoTestsRefParamFuncs();
    }

    public static void DoSimpleTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var IntId = new MyIdBase<int>();
        var ShortId = new MyIdBase<short>();
        var StructId = new MyIdBase<MyIdentStruct>();
        var ClassId = new MyIdBase<MyClassIdValue>();
    }

    // how to call operator+ on values?
    //public static T GetSumOfValues<T>(T a, T b)
    //{
    //    return a + b;
    //}

    public static T GetTypedSumOfInts<T>(int a, int b) where T : struct
    {
        // CS0030: Cannot convert int to T!!!
        //return (T)(a + b);

        // Compiles
        //return (T)Convert.ChangeType(a + b, typeof(T));

        // It works
        return (T)(object)(a+b);

        // wrong
        //return (a + b) as T;
    }

    public static void TestCastingsToT()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var Sum = GetTypedSumOfInts<int>(2, 3);
        Contract.Assert(Sum == 5);
        Console.WriteLine($"Sum={Sum}");
    }

    //static void MyOutputDefault<T>(out T Value)
    //{
    //        out Value = (T)3;
    //}


    // Constraints in functions

    public static void DoTestsRefParamFuncs()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        
    }
}