using System;

public enum EMyProfession
{ 
    Programmer,
    SysAdmin,
    Librarian
};

public readonly struct MyPersonPassportStruct
{
    public int PassportId { get; }
    public string Name { get; }
    public string Surname { get; }
    public bool IsMan { get; }
    public int Age { get; }
    public string Country { get; }
    public string City { get; }
    public DateTime DataOfIssuance { get; }

    public MyPersonPassportStruct(int PassportId, string Name, string Surname, bool IsMan = true, int Age = 18)
    {
        this.PassportId = PassportId;
        this.Name = Name;
        this.Surname = Surname;
        this.IsMan = IsMan;
        this.Age = Age;
        this.Country = "";
        this.City = "";
        this.DataOfIssuance = new DateTime(1999, 4, 1);
    }    
};
public struct MyPersonStruct
{
    public EMyProfession Profession { get; set; }
    public MyPersonPassportStruct Passport { get; set; }

    public MyPersonStruct(EMyProfession Profession, MyPersonPassportStruct Passport)
    {
        this.Profession = Profession;
        this.Passport = Passport;
    }

    // WARNING: readonly-members should only be used in preview at this time!
    //public readonly bool IsValid()
    //{
    //}
}

public static class TestStatementExtra
{
    public static void TestAll()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        TestPatternSwitch();
        TestNullableReferenceTest();
    }

    public static void TestPatternSwitch()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var andrey_passport = new MyPersonPassportStruct(34, "Andrey", "Petrov");
        var andrey = new MyPersonStruct(EMyProfession.Programmer, andrey_passport);

        // @TODO: Use preview for ("recursive patterns")
        //bool bShouldChangeIdentityPaper = andrey switch
        //{
        //   { Passport : { Country == "USA" } } => true,
        //    { Passport : { Age == 18 } } => true
        //    // Like default, but NOT null
        //    {} => false,
        //    // If null
        //    _ => false
        //};
    }
    public static void TestNullableReferenceTest()
    {
        //#nullable enable
        //string not_nullable = null;
        //string? nullable = null;
        //#nullable disable
    }

    public static void SwitchFunctionTest()
    {
        #pragma warning disable 219
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        EMyProfession Prof = 0;
        // Not yet supported
        //string ProfString = Prof switch
        // {
        //   EMyProfession.Librarian => "Librarian",
        //   EMyProfession.Programmer => "Programmer",
        //  EMyProfession.SysAdmin => "SysAdmin",
        // _ => "Unknown"
        //};
        #pragma warning restore 219
    }

    public static void TuplePatternTest()
    {
        #pragma warning disable 219
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        bool bMatchBothZero = true;
        int X = 0, Y = 0;

        bool bMatched = false;
        //switch (bMatchBothZero, X, Y)
        //{
        //    case (true, 0, 0):
        //        bMatched = true;
        //    default:
        //        break;
        //}
        #pragma warning restore 219
    }
};