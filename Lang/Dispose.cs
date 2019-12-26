using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;


public class MySceneManager : IDisposable
{
    public int Id { get; set; } = GenerateId();
    static int NextId = 0;
    static int GenerateId() =>NextId++;

    // From IDisposable
    public void Dispose()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"Id = {Id}");
        DoDispose(true);
        System.GC.SuppressFinalize(this);
    }

    // Destructor
    ~MySceneManager()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        DoDispose(false);
    }

    void DoDispose(bool bDisposing)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"DoDispose: bDisposing={bDisposing}");
        // @TODO: Dispose resources here
        bDisposed = true;
    }

    bool bDisposed = false;
}

public static class DisposeTestes
{
	public static void DoAllTests()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        TestGCCollect();

        DoSimpleDispose();

        DoUsingDispose();
	}

    public static void TestGCCollect()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var SceneManager = new MySceneManager();
        SceneManager = null;
        GC.Collect();
        // Wait for pending finalizers does not wait until gc.collect will start garbage collection
        GC.WaitForPendingFinalizers();

    }

	public static void DoSimpleDispose()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var SceneManager = new MySceneManager();
        SceneManager.Dispose();        
    }

    public static void DoUsingDispose()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // C# 8.0
        //using var SceneManager = new MySceneManager();

        using (var SceneManager = new MySceneManager())
        {
            //
        }
    }
};
