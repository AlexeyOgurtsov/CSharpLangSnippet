using System;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.Linq;

public struct MyDamageType
{
    int _id;
    string _name;
    public MyDamageType(string name)
    {
        this._name = name;
        _id = 0; // @TODO
    }

    public void ResetName()
    {
        _name = "";
    }
    public readonly void Print()
    {
        // We can NOT assign fields inside read-only members!
        //_id = 1; // Compilation error
        Console.WriteLine($"name={_name}; id={_id}");
    }
};
public static class MyDamageTypeUtils
{
    public static void MyPrintWithMessage(this MyDamageType S, string message)
    {
        Console.WriteLine(message);
        S.Print();
    }
}
public static class StructReadonlyMembersTests
{
    public static void DoAllTest()
    {
        Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");
        TestExtMethod();
        TestReadonlyMethod();
    }
    public static void TestExtMethod()
    {
        Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");
        MyDamageType s = new MyDamageType($"{nameof(s)}");
        s.MyPrintWithMessage($"{nameof(TestExtMethod)}");
    }
    public static void TestReadonlyMethod()
    {
        Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");

        {            
            static void CallByValue(MyDamageType s)
            {
                Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");
                s.ResetName();
            }

            MyDamageType s = new MyDamageType($"{nameof(s)}");
            CallByValue(s);
            s.Print();
        }

        {
            static void CallByRef(ref MyDamageType s)
            {
                Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");
                s.ResetName();
            }

            MyDamageType s = new MyDamageType($"{nameof(s)}");
            CallByRef(ref s);
            s.Print();
        }

        {
            // WARNING!!! WARNING!!! WARNING!!!
            // When we pass struct with "in" keyword,
            // and modify it inside the method,
            // its COPY will be modified inside the method, 
            // NOT the struct itself!!!!
            static void CallByIn(in MyDamageType s)
            {
                Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");
                s.ResetName();
            }

            MyDamageType s = new MyDamageType($"{nameof(s)}");
            CallByIn(s);
            s.Print();
        }

        {
            // COMPILATION error: readonly cannot be used for this element:

            //static void CallByRefReadonly(ref readonly MyDamageType s)
            //{
            //    Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");
            //    s.ResetName();
            //}
            //
            //MyDamageType s = new MyDamageType($"{nameof(s)}");
            //CallByReadonlyRef(s);
        }
    }
}