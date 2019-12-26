using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lang
{
    static class Statement
    {
        public static void ConstTest()
        {
            #pragma warning disable 168, 219
            // CS0822: Vars cannot be const!
            //const var i = 1;

            const int count = 1;
            const MyMetaClass c = null;

            // readonly cannot be used!
            //readonly MyMetaClass c2 = new MyMetaClass();
            // CS1033: Expression must be const
            //const MyMetaClass c2 = new MyMetaClass();
            #pragma warning restore 168, 219
        }

        // Compiles well 
        public static string GetHello() => "test";
    }
}
