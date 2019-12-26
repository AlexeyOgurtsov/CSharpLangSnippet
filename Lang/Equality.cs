using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

// Equatable class
class MyEquatableRecordClass : IEquatable<MyEquatableRecordClass>
{
    public int ClassId { get; set; }
    public string Name { get; set; }


    // WARN!!! Not Necessarily good to overload operator== and operator!= for classes

    // Typed IEquatable's method Equal: checks all fields
    bool IEquatable<MyEquatableRecordClass>.Equals(MyEquatableRecordClass Other)
    {
        if (Other == null) return false;
        return ClassId == Other.ClassId && Name == Other.Name;
    }
    public override bool Equals(object Other)
    {
        if (Other is MyEquatableRecordClass TypedOther)
        {
            return ((IEquatable<MyEquatableRecordClass>)this).Equals(TypedOther);
        }
        return false;
    }
    public override int GetHashCode() => ClassId ^ Name.GetHashCode();
};

// Equatable struct
struct MyEquatableRecord : IEquatable<MyEquatableRecord>
{
    public int ClassId;
    public string Name;

    // Typicall we overload operator== and operator!=
    public static bool operator==(MyEquatableRecord a, MyEquatableRecord b)
    {
        return a.ClassId == b.ClassId && a.Name == b.Name;
    }
    public static bool operator!=(MyEquatableRecord a, MyEquatableRecord b) => !(a == b);

    // Typed IEquatable's method Equal: checks all fields
    bool IEquatable<MyEquatableRecord>.Equals(MyEquatableRecord Other) => this == Other;
    public override bool Equals(object Other)
    {
        // Very IMPORTANT: Requirement: When Other is null, never nullptr
        if (Other == null) return false;
        return this == (MyEquatableRecord)Other;
    }
    public override int GetHashCode() => ClassId ^ Name.GetHashCode();
};


// WARNING: Structures can NOT be derived!!!
//public struct DerivedEquatableRecord : MyEquatableRecord { }

public static class EqualityTests
{
    public static void DoAllTests()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Test_DefaultStructEquals();
        Test_StructCustomEquals();
        Test_FindDefaultStructByValue();
        Test_ClassCustomEquals();
    }
    public static void Test_DefaultStructEquals()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var a = new MyVecStruct { x = 1, y = 2 };
        var a2 = new MyVecStruct { x = 1, y = 2 };
        var b = new MyVecStruct { x = 6, y = 3 };

        Contract.Assert(a.Equals(a2));
        Contract.Assert(!a.Equals(b));
        // WARNING!!! Comparing to null must always return false (requirement for Equals!)
        Contract.Assert(!a.Equals(null));

        // Compile error: operator== and operator!= are NOT defined by default for structs!
        //Contract.Assert( a == a2 );
    }

    public static void Test_StructCustomEquals()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var a = new MyEquatableRecord { ClassId = 1, Name = "2" };
        var a2 = new MyEquatableRecord { ClassId = 1, Name = "2" };
        var b = new MyEquatableRecord { ClassId = 6, Name = "3" };


        Contract.Assert(a.Equals(a2));
        Contract.Assert(!a.Equals(b));
        // WARNING!!! Comparing to null must always return false (requirement for Equals!)
        Contract.Assert(!a.Equals(null));

        Contract.Assert(a == a2);
        Contract.Assert(a != b);
    }

    public static void Test_FindDefaultStructByValue()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var a = new MyVecStruct { x = 1, y = 2 };
        var a2 = new MyVecStruct { x = 1, y = 2 };
        var b = new MyVecStruct { x = 6, y = 3 };

        var arr_vecs = new MyVecStruct[]
        {
            b, a2, a
        };

        // Find index of struct without any operatos in array by its value,
        // WORKS WELL OUT OF BOX!!!
        {
            int index = Array.IndexOf(arr_vecs, a);            
            Contract.Assert(index != (arr_vecs.GetLowerBound(0) - 1));
            Contract.Assert(arr_vecs[index].Equals(a));
        }

        var dict = new Dictionary<int, MyVecStruct>();
        dict.Add(0, b);
        dict.Add(1, a2);
        dict.Add(2, a);

        {
            // @TODO: How to find by value?
        }
    }

    public static void Test_ClassCustomEquals()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var a = new MyEquatableRecordClass { ClassId = 1, Name = "2" };
        var a2 = new MyEquatableRecordClass { ClassId = 1, Name = "2" };
        var b = new MyEquatableRecordClass { ClassId = 6, Name = "3" };

        Contract.Assert(a.Equals(a2));
        Contract.Assert(!a.Equals(b));
        // WARNING!!! Comparing to null must always return false (requirement for Equals!)
        Contract.Assert(!a.Equals(null));
    }
}
