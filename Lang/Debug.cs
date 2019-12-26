using System;
using System.Diagnostics.Contracts;

public static class DebugTests
{
    public static void TestDebug()
    {
        TestContracts();
        TestAssert();
        //TestNotImpl();
    }
    public static void TestContracts()
    {

    }
    public static int GetCapacityForString(string S)
    {
        Contract.Requires(S != null);
        // Here we check that OUT return value for POST-condition
        Contract.Ensures(Contract.Result<int>() >= S.Length);
        return 2 * S.Length;
    }
    public static void TestAssert()
    {
        // @see contracts better
        //System.Diagnostics.Debug.Assert;
    }
    public static void TestNotImpl()
    {
        // OK
        //throw new NotImplementedException();

        // Version with String
        throw new NotImplementedException("Just an example");
    }
}