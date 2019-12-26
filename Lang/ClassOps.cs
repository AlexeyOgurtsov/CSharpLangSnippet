using System;

class OpClass
{
	public int Val { get; set; }


	public OpClass(int Val)
	{
		this.Val = Val;
	}

	// 1. All operations are defined with static
	// 2. operator++ defines both postfix and prefix form by a single one!
	public static OpClass operator++(OpClass a)
	{
		return new OpClass(a.Val + 1);
	}

    // WARNING!!! operator+= will work automaticall, when operator+ is defined!
    public static OpClass operator+(OpClass a, OpClass b)
    {
        return new OpClass(a.Val + b.Val);
    }

    // Indexer
    // 1. this
    // 2. NOT static
    // 3. Can be overloaded
    public int this[int i]
    {
        get { return i; }
        set { Val = i; }
    }

};

public static class ClassOps
{
	public static void TestClassOps()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        TestClassIncrement();
        TestClassOperatorPlus();
    }

    public static void TestClassIncrement()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var op = new OpClass(3);
        var next_op = ++op;
        var op2 = op++;
        Console.WriteLine($"op={op}");
    }

    public static void TestClassOperatorPlus()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var op = new OpClass(3);
        var op2 = new OpClass(7);
        var result_op = op + op2;
        var my_op = new OpClass(0);
        // WARNING operator-= works automaticall, when operator+ defined!!!
        my_op += new OpClass(2);
        // But this will NOT automatically (when operator+ is defined)!
        //my_op -= new OpClass(2);

        Console.WriteLine($"result_op={result_op}");
    }
};
