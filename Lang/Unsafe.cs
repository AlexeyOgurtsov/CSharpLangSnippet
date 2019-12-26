using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

public static class UnsafeTests
{
	public static void DoAllTests()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		//Test_UnsafePointer();
	}

    // Unsafe code can only be used when compiling with parameter /unsafe
	//public static unsafe void Test_UnsafePointer()
	//{
	//	Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
	//}
}
