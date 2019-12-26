using System;
using System.Reflection;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Collections.Generic;

public static class LinqTests
{
    public static void DoAllTests()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        SimpleLinqTest();
        LinqDictTest();
    }

    public static void SimpleLinqTest()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);

        var arr = new string[] { "FirstCar", "SecondTime", "SeventhCar", "ThirdCar" };
        var cars =
            from s in arr
            where s.ToLower().Contains("car")
            select s;
        foreach (string s in cars)
        {
            Console.WriteLine($"Car = {s}");
        }
    }

    public static void LinqDictTest()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        var dict = new Dictionary<int, string>(30);
        dict.Add(1, string.Empty);
        dict.Add(2, null);
        dict.Add(3, "PlayerController");
        dict.Add(4, "PlayerInput");
        dict.Add(5, "GameInfo");
        dict.Add(6, null);

        // @TODO: Select all key-values that start with Player
    }
}