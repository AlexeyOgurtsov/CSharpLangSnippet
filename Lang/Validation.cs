using System;
using System.Diagnostics.Contracts;
using System.Reflection;

using System.ComponentModel.DataAnnotations;


// Why it does not work - validation?
public class MyValidateableStruct
{
    [Required]
    [Range(1,10)]
    public int UserId { get; set; }

    [Required]
    [StringLength(30, MinimumLength=1)]
    public string UserName { get; set; }
    public string UserSurname { get; set; }
};

public static class ValidationTests
{
    public static void DoAllTests()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        DoSimpleValidateStruct();
    }

    public static void DoSimpleValidateStruct()
    {
        Console.WriteLine(MethodBase.GetCurrentMethod().Name);
        {
            var User_Wrong = new { UserId = 0, UserName = "", UserSurname = "ABC" };
            var User_Context = new ValidationContext(User_Wrong);
            var ValidationResults = new System.Collections.Generic.List<ValidationResult>();
            const bool bValidateAllProps = true;
            if (!Validator.TryValidateObject(User_Wrong, User_Context, ValidationResults, bValidateAllProps))
            {
                foreach (ValidationResult Error in ValidationResults)
                {
                    Console.WriteLine(Error.ErrorMessage);
                }
            }
        }
    }
}
