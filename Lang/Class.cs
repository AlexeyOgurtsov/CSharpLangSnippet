using System;

public class MyMetaClass
{
    public static void ClassTest()
    {
        Console.WriteLine("ClassTest");

        MyMetaClass DefC = new MyMetaClass();
        Console.WriteLine("DefC={0}", DefC);

        MyMetaClass NamedC = new MyMetaClass("MyClass");
        Console.WriteLine("NamedC={0}", NamedC);

        MyMetaClass NamedWithID = new MyMetaClass(3, "MyClassWithID");
        Console.WriteLine("NamedWithID={0}", NamedWithID);
    }

    #region construction
    // Default ctor: must be provided if to-be default-constructible, if constructor with parameters is defined
    public MyMetaClass()
    {
    }
    public MyMetaClass(string InClassName)
    {
        ClassName = InClassName;
    }

    public MyMetaClass(int InID, string InClassName)
    {
        ID = InID;
        ClassName = InClassName;
    }
    #endregion // construction

    #region properties
    public string ClassName { get; }
    public int ID
    {
        get { return _ID; }
        set { _ID = value; }
    }
    public bool IsSealed
    {
        get { return _bSealed; }
        set { _bSealed = value; }
    }
    #endregion // properties

    #region functions
    void Compile()
    {
        Console.WriteLine("Compile");
    }

    public override string ToString()
    {
        return $"ID={ID}, ClassName={ClassName}, IsSealed={IsSealed}";        
    }
    #endregion // functions


    static int LastId = -1;
    static int GeneratedID()
    {
        // WARNING! Cannot use "Static" in this context (CS0103)
        //static int LastId = -1;
        return ++LastId;
    }

    #region variables
    private bool _bSealed;
    private int _ID = GeneratedID();
    #endregion // variables
};
