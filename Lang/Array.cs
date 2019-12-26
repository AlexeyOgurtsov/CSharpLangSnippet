using System;
using System.Linq;

public interface IMyLogger
{
    void Log(string Message);

    // C# 8.0: default implementations in interface: Not yet supported (use preview)
    //void LogInt(int i)
    //{
    //   Log(i);
    //}

};

public class MyConsoleLogger : IMyLogger
{
    void IMyLogger.Log(string Message)
    {
        Console.WriteLine(Message);
    }
};

public class MyVec
{
    public float x, y, z;
    public MyVec()
    {
    }
    public MyVec(float InX, float InY, float InZ)        
    {
        x = InX;
        y = InY;
        z = InZ;
    }
};
public class MyVec4 : MyVec
{
    public float w;

    public MyVec4() { }
    public MyVec4(float InX, float InY, float InZ, float InW) :
        base(InX, InY, InZ)
    {
        w = InW;
    }
};

public class ArrayHolder
{
    ArrayHolder()
    {
        // OK: Ever read-only prop can be initialized inside the ctor
        ArrayProp = new int[] { 1, 2, 3 };
        ReadOnlyArray = new int[] { 1, 2, 3 };
        // WRONG to initialize const in ctor!
        //ConstArray = new int[] { 1, 2, 3 };
    }

    void Test()
    {
        // CS0200: Impossible to set value for readonly property or indexer
        // (however - possible in ctor)
        //ArrayProp = new int[] { 1, 2, 3 };
    }


    public int[] ArrayProp { get; }
    public int[] Array;
    // Array props DO can be uninitialized if readonly (ever in ctor)
    public readonly int[] ReadOnlyArray;

    // Wrong to make const uninitialized:
    //public const  int[] ConstArray;
    // const arrays are impossible at all (CS0133)!
    //public const int[] ConstArray = new int[] { 1, 2, 3 };
    public const int[] ConstNullArray = null;

    static ArrayHolder()
    {

    }

    public static int[] StatArray;
    public static readonly int[] ReadOnlyStatArray;

    // @TODO: Refs to arrays, readonly refs to arrays
}

public static class ArrayTestClass
{
    public static void TestArray()
    {	    
        ArrayCastTest();

        TestAccessArrayByIndex();
        TestArrayAndRange();

        ArraySpanTest();
	    ReadOnlySpanTest();

        ReadOnlyListTest();
        ArraySegmentTest();

        IndexOfTest();
        BinarySearchTest();

        SimpleTestArray();
        MultiArrTest();
        ArrayByRef();
    }
    public static void SimpleTestArray()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // WARNING! We cannot apply const here!
        int[] SizedArr = new int[4];

        // WARNING! We cannot apply const here either!
        int[] InitializedArr = new int[] { 1, 2, 3, };

        // Looping array
        // WARNING! For one-dimensional array we could use Length property as well
        // (but NOT include it while iterating!)
        for (int i = 0; i <= InitializedArr.GetUpperBound(0); i++)
        {
            Console.WriteLine($"InitializedArr[{i}]={InitializedArr[i]}");
        }
    }

    public static void UninitializedArrayTest()
    {
        var arr = new int[4];
    }

    public static void MultiArrTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        {
            Console.WriteLine("Checking initialized 2D mas");
            int[,] mas =
            {
                {  1, 2, 3 },
                { 4, 5, 6 },
            };
            // Length of multi-dimensional array is equal to the TOTAL length of ALL elements in the array
            Console.WriteLine($"Length of {nameof(mas)} is {mas.Length}");
            Console.WriteLine($"UpperBound0={mas.GetUpperBound(0)}, UpperBound1={mas.GetUpperBound(1)}");
            // We can for-each ALL elements in the multi-dimensional array
            foreach (int i in mas)
            {
                Console.WriteLine($"{i}");
            }
        }

        {
            Console.WriteLine("Checking stepped array");
            // WARN! It's a different representation of array rather [,],
            // so foreach and Length have a different effect!
            //int[][] mas = new int[3][];

            // Length of multi-dimensional array is equal to the TOTAL length of ALL elements in the array
            //Console.WriteLine($"Length of {nameof(mas)} is {mas.Length}");
            //Console.WriteLine($"UpperBound0={mas.GetUpperBound(0)}, UpperBound1={mas.GetUpperBound(1)}");
            // We can for-each ALL elements in the multi-dimensional array
            //foreach (int[] i in mas)
            //{
            //   Console.WriteLine($"{i}");
            //}
        }
    }

    // ARRAYS are automatically passed by REF!
    public static void ArrayByRef()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        int[] arr = { 1, 2, 3 };
        void NegateArray(int[] InOutArr)
        {
            for (int i = 0; i < InOutArr.Length; i++)
            {
                InOutArr[i] = -InOutArr[i];
            }
        }
        NegateArray(arr);
        foreach (int i in arr)
        {
            Console.WriteLine(i);
        }
    }

    // Testing Array.IndexOf static function
    // WARNING!!! It's static function!
    public static void IndexOfTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        int[] arr = { 1, 6, 3, 1, 5, 3 };
        int SearchedIndex = Array.IndexOf(arr, 3);
        if (SearchedIndex == arr.GetLowerBound(0) - 1)
        {
            Console.WriteLine("Cannot find");
        }
        else
        {
            Console.WriteLine($"IndexOf returned valid index: {nameof(Array)}.IndexOf({nameof(arr)}, 3)={SearchedIndex}");
        }
    }

    public static void BinarySearchTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        // @TODO
    }

    public static void ArrayCastTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        ArrayToDerivedCastTest();
        ArrayToBaseCastTest();
        ArrayWidenCastTest();
    }

    public static void ArrayToDerivedCastTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        

        {
            // We DO can explicitly convert array to DERIVED
            MyVec[] arr = new MyVec4[] { new MyVec4(1, 2, 3, 4) };
            MyVec4[] derived_arr = (MyVec4[])arr;
            System.Diagnostics.Contracts.Contract.Assert(derived_arr.Length == arr.Length);
        }


        {
            // BUT we can NOT explicitly cast INTERFACES to BASE-classes this way!
            IMyLogger[] arr = new IMyLogger[] { new MyConsoleLogger() };
            bool bInvalidCastException = false;
            try
            {
                MyConsoleLogger[] derived_arr = (MyConsoleLogger[])arr;
            }
            #pragma warning disable CS0168
            catch (InvalidCastException Ex)
            {
            #pragma warning restore CS0168
                bInvalidCastException = true;
            }
            System.Diagnostics.Contracts.Contract.Assert(bInvalidCastException);
        }

        {

            MyVec[] arr = new MyVec[] { new MyVec(1, 2, 3) };
            bool bInvalidCastException = false;
            try
            {
                MyVec4[] derived_arr = (MyVec4[])arr;
            }
            #pragma warning disable CS0168
            catch (InvalidCastException Ex)
            {
            #pragma warning restore CS0168
                bInvalidCastException = true;                
            }
            System.Diagnostics.Contracts.Contract.Assert(bInvalidCastException);
        }
    }

    // WARNING!!! Casting from Derived[] to Base[] is implicit!
    public static void ArrayToBaseCastTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        {
            var arr = new MyVec4[] { new MyVec4(1, 2, 3, 4) };
            // OK: Compiles WELL!!!
            MyVec[] base_arr = arr;
            void TakeArrayToBase(MyVec[] Arr) { }
            TakeArrayToBase(arr);
        }

        {
            // WORKS with interfaces!
            MyConsoleLogger[] arr = null;
            IMyLogger[] base_arr = arr;
        }
    }
    public static void ArrayWidenCastTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var arr = new int[] { 1, 2, 3, 4, 5 };

        // Conversion int[] -> long[] using Array.ConvertAll
        // THE ONLY VALID Method!!!
        {
            // WRONG: CS0029: Unable to convert int[] to long[]
            //long[] longArray = Array.ConvertAll(arr, i => i);
            long[] longArray = Array.ConvertAll(arr, i => (long)i);
            //foreach (long i in longArray)
            //{
            //    Console.WriteLine(i);
            //}
        }

        // Performing widening conversions:
        {
            // CS0029: Cannot implicitly convert int[] to long[]
            //long[] arr_long = arr;
            // CS0030: Cannot convert int[] to long[]
            //long[] arr_long = (long[])arr;

            // (conversion int[] to loing[] using arr.Cast<long>()!!!!
            // It's WRONG!!! Because ToArray() will fail!
            {
                // At this point it will NOT fail, but it WILL on the ToArray() stage!
                System.Collections.Generic.IEnumerable<long> longEnumerable = arr.Cast<long>();                
                bool bInvalidCastException = false;
                try
                {
                    // WARNING!!! Wrong: This will cause InvalidCastException 
                    long[] longArray = longEnumerable.ToArray();
                }
                catch (System.InvalidCastException Ex)
                {
                    bInvalidCastException = true;
                }
                System.Diagnostics.Contracts.Contract.Assert(bInvalidCastException);
            }
        }

        // OfType: returns SUBSET that are convertible!
        // So it's ALSO wrong for converting int[] -> long[]
        {
            System.Collections.Generic.IEnumerable<long> longEnumerable = arr.OfType<long>();
            long[] longArray = longEnumerable.ToArray();
            System.Diagnostics.Contracts.Contract.Assert(longArray.Length == 0);            
        }
    }

    public static void ArraySpanTest()
    {
        // WARNING!!! We need to include Nu-get package System.Memory
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var arr = new int[] { 1, 2, 3, 4, 5 };
        // Span of array, containing elements from 1 to end.
        var span = new Span<int>(arr, 1, arr.Length - 1);
        var as_span = arr.AsSpan(1, arr.Length - 1); // OK
        foreach (int i in arr)
        {
            Console.WriteLine(i);
        }
    }

    public static void ReadOnlySpanTest()
    {
	    Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
	    var arr = new int[] { 1, 2, 3, 4, 5 };
	    var read_only_span = new ReadOnlySpan<int>(arr);

        {
            // WRONG: Fails to compile
            //int[] arr_from_span = (int[])(read_only_span);        
            // Just Copying
            int[] arr_from_span = read_only_span.ToArray();
        }

        {
            // WARN: No such operation Array.Slice or arr.Slice
            //ReadOnlySpan<int> read_only_span_from_slice = Array.Slice();

            ReadOnlySpan<int> read_only_span_from_slice = read_only_span.Slice(1, arr.Length - 1);
            System.Diagnostics.Contracts.Contract.Assert(read_only_span_from_slice.Length == arr.Length - 1);
        }

        void TakeReadOnlySpawn(ReadOnlySpan<int> S) { }
        // Converting IMPLICITLY read-only span from array
        {
            
            TakeReadOnlySpawn(arr);
        }

        // Converting IMPLICITLY read-only span from span
        {           
            TakeReadOnlySpawn(new Span<int>(arr));
        }
    }

    // Like view in array
    public static void ArraySegmentTest()
    {
        // WARN!!! Better see Span<int>
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var arr = new int[] { 1, 2, 3, 4, 5 };
        // segment of array, containing elements from 1 to end.
        var arr_segment = new ArraySegment<int>(arr, 1, arr.Length - 1);
        foreach (int i in arr)
        {
            Console.WriteLine(i);
        }
    }

    public static void ReadOnlyListTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var arr = new int[] { 1, 2, 3 };

        {
            // WARNING!!! Right is to use Array.AsReadOnly(arr) to convert to IReadOnlyList!!!
            // We DO can cast back to array from IReadOnlyList to remove read-onlyness!
            System.Collections.Generic.IReadOnlyList<int> readonly_arr = arr;

            int[] arr_from_readonly = (int[])readonly_arr;
            arr_from_readonly[0] = 0;
            System.Diagnostics.Contracts.Contract.Assert(arr_from_readonly[0] == 0);
        }

        {
            System.Collections.Generic.IReadOnlyList<int> readonly_arr = Array.AsReadOnly<int>(arr);
            bool bCatched = false;
            try
            {
                // HERE we will get InvalidCastException!!!
                int[] arr_from_readonly = (int[])readonly_arr;
            }
            #pragma warning disable 168
            catch(InvalidCastException Ex)
            #pragma warning restore 168
            {
                bCatched = true;
            }   
            System.Diagnostics.Contracts.Contract.Assert(bCatched);
        }
    }

    public static void TestAccessArrayByIndex()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        // WARNING To use Index, you need to target .Net Core 3.0!
        //Index index = new Index(0, false);
    }

    public static void TestArrayAndRange()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
}
