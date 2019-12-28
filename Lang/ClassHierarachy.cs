using System;
using System.Reflection;
using System.Diagnostics.Contracts;

public abstract class MySystemBase
{
    public string Name { get; }
    public string DebugString
    {
        get => _DebugString;
        set => _DebugString = value;
    }
    public virtual string CurrentState
    {
        get => "NoState";
    }
    public virtual string GoalState
    {
        get => "GoalState";
        set
        {
            Console.WriteLine("No setup");
        }
    }
    // We always must provide get/set (or both) for abstract property
    public abstract bool IsActive { get; set; }

    public virtual void PrintDebugInfo()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"Name=\"{Name}\"");
        Console.WriteLine(DebugString);
    }

    public readonly void PrintExtInfo()
    {
    }

    public abstract void BeginPlay();
    public MySystemBase(string Name)
    {
        // We can initialize properties, ever they have NO set operation in it
        this.Name = Name;
    }

    string _DebugString;    
};

public class MyRenderSystem : MySystemBase
{    
    public MyRenderSystem() :
        base("Render")
    {
    }
    public override string CurrentState
    {
        get => "Rendering";        
    }
    // When overriding property we DO may override only PART of setters!
    public override string GoalState
    {
        set
        {
            Console.WriteLine("Setting up");
        }
    }

    // When overriding abstract property:
    // - we DO may implement it automatically
    public override bool IsActive { get; set; }
    public override void PrintDebugInfo()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);        
        base.PrintDebugInfo();
        Console.WriteLine("Printing extra debug info here");
    }

    public override void BeginPlay()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        // Wrong: call call abstract method of the base class
        //base.BeginPlay();
    }
};

public static class ClassHierarchyTests
{
    public static void DoAllTests()
    {        
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        DoOverloadTest();
    }

    public static void DoOverloadTest()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        MySystemBase ren = new MyRenderSystem();
        ren.BeginPlay(); // Calling overriden abstract method
        ren.PrintDebugInfo();
    }
}