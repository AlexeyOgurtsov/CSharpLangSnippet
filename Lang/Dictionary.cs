using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

public static class DicionaryTests
{
    public static void DoTests()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        SimpleDictionaryTest();
        Test_DictionaryWithStructKey();
    }

    public static void SimpleDictionaryTest()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // @TODO: Is it hash or map?
        var NameById = new Dictionary<int, string>();
        // Count of elements initially:
        Contract.Assert(NameById.Count == 0);

        const int FIRST_KEY = 0;
        const string FIRST_VALUE = "First";
        var FIRST_KEY_VALUE = (Key: FIRST_KEY, Value: FIRST_VALUE);

        const int SECOND_KEY = 1;

        // WARN! Never add key with this value!
        const int NON_REGISTERED_KEY = 1000;

        // Fill
        {
            // WARNING!!! Add never returns value for Dictionary!
            {
                NameById.Add(FIRST_KEY, FIRST_VALUE);
                Contract.Assert(NameById.ContainsKey(FIRST_KEY), "The key must be contained after the add operation");
            }
            NameById.Add(SECOND_KEY, "Second");

            // Trying to Add already-registered key will cause System.ArgumentException            
            {
                bool bArgumentException = false;
                try
                {
                    NameById.Add(FIRST_KEY, "UPDATED_FIRST");
                }
                catch (System.ArgumentException)
                {
                    bArgumentException = true;
                }
                Contract.Assert(bArgumentException);
            }
        }

        // Foreach
        {
            foreach (KeyValuePair<int, string> KeyValue in NameById)
            {
                int Key = KeyValue.Key;
                string Value = KeyValue.Value;

                // We can NOT update value inside the for-each loop!!!
                //KeyValue.Value = "NEW";

                // KeyValuePair are outputted as [Key, Value] format
                Console.WriteLine($"Key={Key}, Value={Value} (KeyValue={KeyValue})");
            }
        }

        // Try get key
        {
            bool bFoundInexistent = NameById.TryGetValue(NON_REGISTERED_KEY, out string InexistentKeyValue);
            Contract.Assert(!bFoundInexistent);

            bool bFound = NameById.TryGetValue(FIRST_KEY, out string Value);
            Contract.Assert(bFound && Value == FIRST_VALUE);
        }

        // Access by inexistent key
        {
            // WARNING!!! Unlike Add, It throws KeyNotFoundException!
            bool bKeyNotFoundCatched = false;
            try
            {
                string VALUE = NameById[NON_REGISTERED_KEY];
            }
            catch (System.Collections.Generic.KeyNotFoundException Ex)
            {
                Console.WriteLine($"Exception catched: {Ex.GetType()}");
                System.Collections.IDictionary Dict = Ex.Data;
                Console.WriteLine($"Dictionary={Dict}");
                bKeyNotFoundCatched = true;
            }
            Contract.Assert(bKeyNotFoundCatched);
        }

        // Remove key with the given id
        {
            bool bInexistentRemoved = NameById.Remove(NON_REGISTERED_KEY);
            Contract.Assert(false == bInexistentRemoved);

            bool bRemoved = NameById.Remove(FIRST_KEY);
            Contract.Assert(bRemoved);
        }

        // @TODO: How to find by value?
    }

    struct Struct_DefaultHashCode
    {
        public int x, y;
    };
    public static void Test_DictionaryWithStructKey()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        var Dict = new Dictionary<Struct_DefaultHashCode, int>();
        Dict.Add(new Struct_DefaultHashCode { x=2, y=1 }, 4);
        Dict.Add(new Struct_DefaultHashCode { x = 7, y = 3 }, 2);
    }
};