using System;

// Interfaces are ALSO internal by default
// But interface members are always public by default!
public interface IMyObjectBase
{
	#region interface static members 
    // Only in preview
	//static int NextId = 0;
	//static int GenerateNextId() => NextId++;
	#endregion interface static members 

	// Interface property
    // Only in preview
	//int Id { get; protected set; }
    int Id { get; }
};

public interface IMySceneObject : IMyObjectBase
{
};

public static class InterfaceTests
{
	public static void DoAllTests()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
	}
}
