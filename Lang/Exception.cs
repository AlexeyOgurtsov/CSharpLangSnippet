using System;
using System.Reflection;

public class MyException : System.Exception
{
	public MyException(string Name)
		: base(Name)
	{
	}

    public MyException(string Name, System.Exception InnerException)
        : base(Name, InnerException)
    {
    }
}

public static class MyExceptionTests
{
	public static void DoAllTest()
	{
		Console.WriteLine(MethodBase.GetCurrentMethod().Name);
		TryCatchFinallyTest();
	}

	public static void TryCatchFinallyTest()
	{
		Console.WriteLine(MethodBase.GetCurrentMethod().Name);
		
		// No Throw
		{
			Console.WriteLine("NO throwing");
			MyThrowConditionallyAndCatch(bThrow: false);
		}

		// Throwing
		{
			Console.WriteLine("Throwing");
			MyThrowConditionallyAndCatch(bThrow: true);
		}
	}

	static void MyThrowConditionallyAndCatch(bool bThrow)
	{
		try
		{
			if(bThrow)
			{
				throw new MyException($"Test throwing");
			}
		}
		catch(MyException Ex)
		{
			Console.WriteLine($"catched");
			Console.WriteLine($"Ex.Message={Ex.Message}");
		}
		finally
		{
			// Always executed (whether catched or not)
			Console.WriteLine($"finally");
		}
	}
};
