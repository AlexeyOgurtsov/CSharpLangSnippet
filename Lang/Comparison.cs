using System;
using System.Diagnostics.Contracts;
using System.Collections.Generic;

#region comparable classes testee
struct MyComparableIdStruct : IComparable<MyComparableIdStruct>
{
    public int Id { get; set; }

    public override string ToString() => $"{Id}";

    public int CompareTo(MyComparableIdStruct Other)
    {
        return Id.CompareTo(Other.Id);
    }
}

//class MyComparableIdClass : IComparable<MyComparableIdClass>
//{
//}
#endregion // comparable classes testee

#region comparer classes testee

// WARN! It's ok struct implements IComparer<char>
struct MyCharComparer : IComparer<char>
{
	public int Compare(char a, char b)
	{
		char UpperA = char.ToUpper(a), UpperB = char.ToUpper(b);
		if(UpperA == UpperB) { return 0; }
		if(UpperA < UpperB) { return -1; }
		return 1;
	}
};
#endregion // comparer classes testee

public static class ComparisonTests
{
	public static void DoAllTests()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Test_Comparable();
        Test_CustomComparer();
	}

    public static void Test_Comparable()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var arr = new MyComparableIdStruct[] 
        {
            new MyComparableIdStruct() { Id = 3 },
            new MyComparableIdStruct() { Id = 1 }
        };

        // Sort the arr
        {
            Console.WriteLine($"Sorting array of {nameof(MyComparableIdStruct)}");
            // WARNING!!! NOTE the syntax, as IT IS array!
            Array.Sort(arr);
            for (int i = 0; i < arr.Length; i++)
            {
                Console.Write($"{arr[i]} ;");
            }
            Console.WriteLine();
        }
    }

	public static void Test_CustomComparer()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

		// Testee initialization
		var arr = new char[] { 'b', 'A', 'd', 'c', 'a', 'C', 'd' };
		var list = new List<char>(arr);

		// Sort the arr
		{
			Console.WriteLine($"Sorting array with Custom Comparer {nameof(MyCharComparer)}");
            // WARNING!!! NOTE the syntax, as IT IS array!
			Array.Sort(arr, new MyCharComparer());
			for(int i = 0; i < arr.Length; i++)
			{
				Console.Write($"{arr[i]} ;");
			}
            Console.WriteLine();
        }

		// Sort the list
		{
			Console.WriteLine($"Sorting list with Custom Comparer {nameof(MyCharComparer)}");
			list.Sort(new MyCharComparer());
			for(int i = 0; i < list.Count; i++)
			{
				Console.Write($"{list[i]} ;");
			}
		}
	}
};
