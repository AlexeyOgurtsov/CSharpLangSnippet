using System;
class MethodTest
{
    public static void TestMethod()
    {
        TestLocalFunction();
        OverloadTest();
        MyGenericTest();
    }
    static void PrintIntField(string VarName, int Value)
        => Console.WriteLine($"{VarName}={Value}");

    static void TestPrintIntField()
    {
        // We can pass variable arguments as named arguments!
        PrintIntField(VarName: "A", Value: 3);
    }

    const int STATIC_CONST_TO_CAPTURE = 3;
    static int static_var_to_capture = 5;
    const int const_to_capture = 3;
    int var_to_capture = 7;
    static void TestLocalFunction()
    {
        var c = new MethodTest();
        c.TestLocalFunctionInstance();
    }
    void TestLocalFunctionInstance()
    {
        #pragma warning disable 8321, 168
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // We DO can CAPCTURE variables of the function in the local function!
        float x = 2.4F;
        const float MY_PI = 3.14F;
        // But we NEVER can capture ref structs!!
        // @TODO: Try to capture ref struct
        MyRefIdentStruct RefStruct;

        // WARNING!!! Local function DO can CAPTURE variables of the function!
        void MyLocalFunction()
        {                        
            Console.WriteLine($"Printing variables and consts from enclosing function {x}, {MY_PI}");
            Console.WriteLine($"Printing captured static vars: {static_var_to_capture}, {STATIC_CONST_TO_CAPTURE}");
            Console.WriteLine($"Printing captured static vars: {const_to_capture}, {var_to_capture}");

            //Console.WriteLine($"Ref structs can NOT be capturef: {RefStruct}");
        }

        // Static local function never capture the variables!

        // @TODO: static local function is preview
        //static void MyStaticLocalFunction()
        // {
        //Console.WriteLine($"Printing variables and consts from enclosing function {x}, {MY_PI}");
        //Console.WriteLine($"Printing captured static vars: {static_var_to_capture}, {STATIC_CONST_TO_CAPTURE}");
        //Console.WriteLine($"Printing captured static vars: {const_to_capture}, {var_to_capture}");
        //}
        #pragma warning restore 8321, 168
    }

    static void TestPrintArrayDoubled()
    {
        var arr = new int[] { 1, 2, 3 };
        // We DO can defined function inside another function!
        void PrintArrayDoubled(int[] Arr)
        {
            foreach (int a in Arr)
            {
                Console.WriteLine(2 * a);
            }
        }
        PrintArrayDoubled(arr);
    }

    // Test of anonymous methods!
    static void TestAnonymousMethods()
    {
        #pragma warning disable 219, 168
        // Warn! Before using anonymous-methods: Conside lambda-expressions!
        // @TODO

        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        int sum = 0;

        // Old style: 
        //var AddToSum = delegate(int val)
        //{
        //   sum += val;
        //};        

        // NewStyle: =>
        #pragma warning restore 219, 168
    }

    static void TestLambdaExpressions()
    {
        // WARNING!!! Cannot assign lambda to non-typed variable!
        //var SqrFunc = x => x * x;

        // OK: Delegates are assigned to Func or Action (NEVER var!!!)
        // bool - result, int - argument
        // Func is delegate that returns
        // SIMPLE: Lambda-EXPRESSION
        Func<int, bool> IsEven = x => x % 2 == 0;

        // Accessing environment:        
        int s = 0;

        // Action is delegate that does NOT return
        // MORE COMPLEX:        
        // Lambda-OPERATOR
        Action<int, int> AddDiffToSum = (x, y) =>
        {
            s += x-y;            
        };
    }

    static bool TryParseXY(string Input, out float X, out float Y)
    {
        X = 0; Y = 0;
        // @TODO        
        return false;
    }

    static void TestTryParseXY()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        // Since C# 7.0 we can define the variable to output DIRECTLY inside the method call
        // (out type varname)
        bool bParsed = TryParseXY("1 2", out float X, out float Y);        
    }
    #region overloading
    // It's right: by ref
    // BUT: CS0664: overload by ref and out both - wrong
    //public static void OverloadedFunc(ref int i)
    //{
    //    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
    //    Console.WriteLine("arg: ref int");
    //}

    
    public static void OverloadedFunc(out int i)
    {
        i = 0;
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine("arg: int");
    }
    public static void OverloadedFunc(int i)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine("arg: int");
    }
    public static void OverloadTest()
    {
        OverloadedFunc(1);        
        int i = 4;
        // we can overload both with out or ref,
        // but NOT both!
        OverloadedFunc(out i);
    }
    #endregion // overloading

    #region generic
    public static void MySwap<T>(ref T a, ref T b)
    {
        T t = a;
        a = b;
        b = t;
    }
    public static void MySwapTest()
    {
        #pragma warning disable 219
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        {
            (int a, int b) = (3, 5);
            // ok: it works (type inference)
            MySwap(ref a, ref b);
            // Also ok
            //MySwap<int>(ref a, ref b);
        }

        {
            int a = 1;
            long b = 2;
            // CS1503, arguments cannot be defined by usage
            //MySwap(ref a, ref b);

            // Does NOT work also (cannot ref long to ref int)
            //MySwap<int>(ref a, ref b);
        }

        // Trying to infere type from two types, derived from the same base
        {
            MyVec a = new MyVec(1, 2, 3);
            MyVec4 b = new MyVec4(1, 2, 3, 4);
            // Type inference here does NOT work here anymore!
            //MySwap(ref a, ref b);

            // This does NOT work also
            //MySwap<MyVec>(ref a, ref b);            
        }
        #pragma warning restore 219
    }

    public static void MyGenericTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        MySwapTest();
    }
    #endregion generic
};