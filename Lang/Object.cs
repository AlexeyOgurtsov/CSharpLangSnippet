using System;
public static class ObjectTest
{
    public static void TestObject()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
    }
};