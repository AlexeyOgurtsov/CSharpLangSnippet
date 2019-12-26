using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang
{
    static class Output
    {
        public static void StringInterpolation()
        {
            #pragma warning disable 219
            Console.WriteLine("------ StringInterpolation");

            // We can use any EXPRESSION!!!
            {
                int[] marks = { 1, 3, 2 };
                string OutS = $"{{Length of {nameof(marks)} is {marks.Length}}}";
                Console.WriteLine($"OutS={OutS}");
            }

            // Escape-sequences
            {
                const int Numerator = 3;
                const int Denom = 5;
                {
                    // @ - verbatim (no-using escape sequences)
                    string OutS = $@"Numerator\Denom={Numerator}\{Denom}";
                    //string OutS = $"Numerator\Denom={Numerator}\{Denom}";
                    Console.WriteLine($"OutS={OutS}");
                }

                {
                    MyMetaClass C = null;
                    string OutS = $"{(C != null ? C.ToString() : "null")}";
                    Console.WriteLine($"OutS={OutS}");
                }
            }

            // Output various types
            {
		        const ESeason Season = ESeason.Spring;
                //const EMyFlags Flags = EMyFlags.LogTime | EMyFlags.LogFunctions;
                const EMyFlags Flags = 0;
                const bool bEnabled = true;
                const string null_s = null;
                const MyMetaClass null_class = null;
                int[] Arr = { 1, 2, 3 };
                int? NullableInt = null;

                string OutS = $"{{NullableInt={NullableInt}, Arr = {Arr}, Flags = {Flags}, Season = {Season}, bEnabled={bEnabled}, null_s={null_s} null_class={null_class}}}";                
                Console.WriteLine($"OutS={OutS}");
            }

            // Using ToString() is Interpolated string
            {
                var mc = new MyMetaClass();
                // Cannot make const, as initializer is NOT-const!
                // (CAS0133: Expression must be constant)
                string OutS = $"{{MetaClass={mc}}}";
                Console.WriteLine($"OutS={OutS}");
            }

            const double PI = Math.PI;
            const int MAX_ELEMS = 7;
            int Count = 4;
            {
                // Cannot make const, as initializer is NOT-const!
                // (CAS0133: Expression must be constant)
                string ElemsDesc = $"{{PI={PI}, MAX_ELEMS={MAX_ELEMS}}}";
                Console.WriteLine($"ElemsDesc={ElemsDesc}");
            }

            // Formatting alignment
            {
                // -5 - to left, 5 - to right
                string Res = $"{{left={MAX_ELEMS, -5}, right={MAX_ELEMS, 5}}}";
                Console.WriteLine($"Res={Res}");
            }
            #pragma warning restore 219
        }

        public static void FormatFloat()
        {
            Console.WriteLine("FormatFloat");

            {
                const float F = 3.1415926F;
                string s_f = $"{F:F2}";
                Console.WriteLine($"s_f={s_f}");
            }

            {
                const double D = 3.1415926F;
                string s_d = $"{D:F2}";
                Console.WriteLine($"s_d={s_d}");
            }


            {
                const decimal M = 3.1415926M;
                string s_m = $"{M:F2}";
                Console.WriteLine($"s_d={s_m}");
            }
        }
    }
}
