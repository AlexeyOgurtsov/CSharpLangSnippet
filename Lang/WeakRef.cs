using System;
using System.Reflection;

public class WKTestable
{
	string msg = "UNINITIALIZED";

	public WKTestable(string msg)
	{
		this.msg = msg;
	}
}

public static class WeakRefTests
{
	public static void DoAllTests()
	{
		SimpleTest();
	}

	public static void SimpleTest()
	{
		Console.WriteLine($"{MethodBase.GetCurrentMethod().Name}");

		// WRONG: Cannot implicitly convert to weak reference!
		//WeakReference<WKTestable> weakRef = new WKTestable("WKTestable");

		WeakReference<WKTestable> weakRef = new WeakReference<WKTestable>(new WKTestable("WKTestable"));
		int iterationIndex = 0;
		int maxIterations = 10;
		while (true)
		{
			// WARNING IsAlive is NOT supported for typed WeakReference<T>!
			WKTestable targetTestable;
			bool bTargetExtracted = weakRef.TryGetTarget(out targetTestable);
			if ( ! bTargetExtracted )
			{
				Console.WriteLine($"Trying to extract weak reference value (using {nameof(weakRef.TryGetTarget)}) failed first type on {iterationIndex}-th iteration");
				break;
			}
			targetTestable = null;
			++iterationIndex;
			if (iterationIndex >= maxIterations)
			{
				Console.WriteLine($"Max iterations gained, but reference is still alive");
				return;
			}
		}
	}
}
