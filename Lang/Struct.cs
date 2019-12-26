using System;
using System.Diagnostics.Contracts;

interface IMyID
{ 
};

// ref struct: 
// struct that is allocated on the stack, NOT on the heap!
// 
// - cannot be inherited from interface;
public ref struct MyRefIdentStruct //: IMyID // Reference types cannot implement interfaces
{
    // we DO can include struct (ever readonly!) inside the ref struct
    public MyIdentStruct Ident;
};

public struct MyRefUserStruct
{
    // CS8345: Ref structs cannot be part of the ordinary structs!
    //public MyRefIdentStruct S;

    // as well for properties
    //public MyRefIdentStruct S { get; set; }
};

static class RefStructTestFuncs
{
    static void TestRefStruct()
    {
        MyRefIdentStruct s = new MyRefIdentStruct { Ident = new MyIdentStruct(2, -1) };

        // WRONG: CS0029: cannot convert nameof(structname) to object
        // (BOXING/Unboxing is NOT supported for ref structs!)
        //object obj = s;

        // Cannot convert ref ident struct to object
        //Console.WriteLine($"Ref struct is {s}");

        Console.WriteLine($"Ref struct is {{ Id={s.Ident.Id}, ClassId={s.Ident.ClassId} }}");
    }
}

// readonly ref structs are valid!
public readonly ref struct MyRefReadonlyStruct {};
// WRONG: Use readonly ref struct!
//public ref readonly struct MyRefReadonlyStruct2 { };


// readonly struct: immutable struct, cannot be changed after initialized!
public readonly struct MyIdentStruct : IMyID // Ok, structs DO can implement interfaces (even READ-only structs!)
{
    // WARNING! Fields must only be readonly in readonly structs!
    public readonly int Id;

    // WARNING! Properties: only get!
    public int ClassId { get;  }

    // WARNING!!! In readonly struct we must always provide the ctor,
    // otherwise - how will be initialize it?
    public MyIdentStruct(int InId, int InClassId)
    {
        this.Id = InId;
        this.ClassId = InClassId;
    }
};

public struct MyVecStruct
{
    public const float ZERO = 0.0F;

    // WARNING!!! we DO must explicitly state that struct variable is public (as it's PRIVATE by default!)
    // or use struct initializer syntax
    public float x, y;

    //When using private variables, we cannot create struct instance without construction
    // or use struct initializer syntax
    //private float z;

    // When using properties inside the struct, we cannot create struct instance without construction
    // or use struct initializer syntax
    //public float PropX { get; set; }

    // WARNING!!! Structs DO can oveload ToString() method!
    public override string ToString() => $"{{ x={x}, y={y} }}";
};

// Unable to inherit from struct: parent is NOT interface!
//public struct MyVec4Struct : MyVecStruct
//{
//};

public struct MyClassStruct
{
    // Static variables and methods are valid inside the struct!
    public static int WRONG_ID = -1;
    public static int NextId = -1;
    public static int GenerateId() => NextId++;

    public bool IsValid() => ID != WRONG_ID;    

    // CS0573: Struct cannot contain property or field initializers (i.e. directly after the field declaration)
    //public int ID = -1;
    public int PropID { get; set; }

    public int ID;
    public string ClassName;
    public MyMetaClass MyMetaClass;
    //public MyClassStruct Parent; // @TODO: ref to struct?

    // CS0568: structs cannot contain explicit constructors without params!    
    //MyClassStruct() {}


    // ToString?
    // Hash?

    MyClassStruct(string InClassName) :
        this(GenerateId(), InClassName)
    {
    }
    
    MyClassStruct(int InID, string InClassName)
    {
        // CS0171 - WE Must initialize ALL struct fields in ctor!
        ID = InID;
        ClassName = InClassName;
        MyMetaClass = null;

        // CS0171 - All properties must also be initialized in ctor! 
        PropID = InID;
    }
};

public struct StructWithRef
{
    // When we define, we get an error CS05051: Body method must be defined, as it's not abstract, extern or partial!
   // public ref MyVecStruct RefToVec;
}

public struct GenericStruct
{ 
    // @TODO: Generic struct
}

public static class StructTest
{
    public static void TestStruct()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        TestOutputStruct();
        TestStructBoxing();
        TestConstructStruct();
        RefToStruct();
    }

    public static void TestOutputStruct()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        MyVecStruct V = new MyVecStruct();
        Console.WriteLine($"Output struct using ToString (implicitly): {V}");
    }

    public static void TestStructBoxing()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        MyVecStruct V = new MyVecStruct();
        // OKEY: Boxing works for struct (but NOT for read structs!)
        object obj_V = V;
        // Unboxing also works
        MyVecStruct V2 = (MyVecStruct)obj_V;
    }
    public static void RefToStruct()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        MyVecStruct V = new MyVecStruct();

        // Reference to V: OK: we must just use the ref keyword!
        ref MyVecStruct refV = ref V;

        // EVER when we pass to function reference-typed variable, we must pass ref!
        void TakeRef(ref MyVecStruct InOutV) { }
        TakeRef(ref refV);
    }

    public static void TestReadOnlyStruct()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        {
            // WRONG: CS0191: Assigning of readonly field is only in ctor or initializer!
            //MyIdentStruct S = new MyIdentStruct { Id = 1 };
        }

        {            
            MyIdentStruct S = new MyIdentStruct ( InId:1, InClassId:4 );
        }
    }
    public static void TestConstructStruct()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // When we create struct without using constructor, we must initialize all fields manually
        {
            MyVecStruct V;
            V.x = 0.0F; V.y = 0.0F;
            // WARNING!!! When using properties in the structs (EVER get/set) we can NOT assign them value,
            // so we cannot create such struct without using constructor!
            //V.PropX = 0.0F;

            // {V} Outputs as a struct class name only!
            Console.WriteLine($"{nameof(MyVecStruct)} instance created: {{ x={V.x}, y={V.y} }}"); 
        }

        // Create struct with struct initializer!
        {

            // Creating struct using struct initializer syntax
            // WARNING!!! Here we can only PARTLY initialize the struct fields and properties!
            MyVecStruct V = new MyVecStruct { x = 1.0F };
            Console.WriteLine($"{nameof(MyClassStruct)} instance created: {V}");
        }

        // Creating struct using struct initializer syntax
        // WARNING!!! Here we can only PARTLY initialize the struct fields and properties!
        {
            MyClassStruct C = new MyClassStruct
            {                
                ID = MyClassStruct.WRONG_ID,
                PropID = 3
            };
            Console.WriteLine($"{nameof(MyClassStruct)} instance created: {{ ID={C.ID}, ClassName={C.ClassName} }}");
        }

        // When we create struct without using constructor, we must initialize all fields and props manually
        {

            MyClassStruct C;
            C.ID = MyClassStruct.WRONG_ID;
            C.ClassName = "TestClass";
            C.MyMetaClass = null;
            //C.PropID = 0; // Why? Unable to assign to prop with get/set using variable C with unassigned value?
            //Console.WriteLine($"{nameof(MyClassStruct)} instance created: {C}");
        }
    }
};