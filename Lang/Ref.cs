using System;
using System.Diagnostics.Contracts;

public struct BookStruct
{
	public string Name;

	public BookStruct(string Name)
	{
		this.Name = Name;
	}
    public void MakeInvalid()
    {
        this.Name = INVALID_BOOK_NAME;
    }
    public static readonly string INVALID_BOOK_NAME = string.Empty;
};

public class BookStructCollection
{
    public BookStruct this[int Index]
    {
        get { return _Books[Index]; }
    }

    public ref BookStruct GetBookRef(int Index) => ref _Books[Index];
    public ref readonly BookStruct GetBookRefReadOnly(int Index) => ref _Books[Index];

	public BookStructCollection()
	{
		_Books = new BookStruct[]
		{
			new BookStruct("RedBook"),
			new BookStruct("BlueBook")
		};
	}

	#region impl
	BookStruct[] _Books;
	#endregion // impl
};

public static class RefTests
{
	public static void DoAllRefTests()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Test_ReturnLocalRef();
        Test_TryModifyStructByRef();
        Test_ForEachRef();
    }    

	public static void Test_TryModifyStructByRef()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);		

		const int INDEX_TO_MODIFY = 0;
		const string CHANGED_NAME = "CHANGED_NAME";

		// Saving reference
		{
            var Books = new BookStructCollection();
            // CS8172: Unable to initialize variable by ref: if we not passed ref to the initializer expression
            ref BookStruct ReturnedStructRef = ref Books.GetBookRef(INDEX_TO_MODIFY);
            // WARNING!!! We can ONLY specify "ref readonly", but NOT readonly ref!!!
			ref readonly BookStruct ReturnedReadOnlyStructRef = ref Books.GetBookRefReadOnly(INDEX_TO_MODIFY);

            // This initialization is performed well:
            ref readonly BookStruct readonly_ref_from_ref = ref ReturnedStructRef;
		}

        // Unable to change what the reference point to after it's created!
        {
            var Books = new BookStructCollection();
            ref BookStruct BookRef = ref Books.GetBookRef(0);
            // It works
            BookRef = Books.GetBookRef(1);

            // It ALSO works!!!
            BookRef = ref Books.GetBookRef(1);

            // @TODO: Perform checks (reference semantics!)
        }

		// Try to modify struct returned by ref
		{
			var Books = new BookStructCollection();		
			Books.GetBookRef(INDEX_TO_MODIFY) = new BookStruct(CHANGED_NAME);
			Contract.Assert(Books[0].Name == CHANGED_NAME);
		}

		// Try to modify struct through read-only ref
		{
			var Books = new BookStructCollection();

            ref readonly BookStruct BookRef = ref Books.GetBookRefReadOnly(INDEX_TO_MODIFY);

            // CS8331: Unable to assign through read-only ref, as it's read-only variable
            //BookRef = new BookStruct(CHANGED_NAME);       

            // WARNING!!! WARNING!!! WARNING!!!
            // However we DO may call any function through REF READONLY!!!!
            // Makes HIDDEN COPY!!!!
            BookRef.MakeInvalid();

            Contract.Assert(Books[0].Name != BookStruct.INVALID_BOOK_NAME);
		}
	}

    public static void Test_ForEachRef()
    {
        
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        {
            var arr = new int[] { 1, 2, 3, 4, 5 };

            {
                var arr_span = arr.AsSpan();
                // OK: this WORKS ok (over Span!!!!)
                foreach (ref readonly int i in arr_span)
                {
                    // @TODO
                }
            }

            {
                var arr_span = arr.AsSpan();
                // OK: this WORKS ok (over Span!!!!)
                foreach (ref int i in arr_span)
                {
                    // @TODO
                }
            }

            {
                // CS1510: Values of ref and out must be variable that can be assigned
                //foreach (ref int a in arr)
                //{
                //}

                // CS1510: Values of ref and out must be variable that can be assigned
                //foreach (ref readonly int a in arr)
                //{
                //}

                // CS1510: Values of ref and out must be variable that can be assigned
                //foreach (ref readonly var a in arr)
                //{
                //}
            }
        }

        {
            var arr = new BookStruct[] { };

            {
                // CS1510: Values of ref and out must be variable that can be assigned
                //foreach (ref BookStruct a in arr)
                //{
                //}

                // CS1510: Values of ref and out must be variable that can be assigned
                //foreach (ref readonly BookStruct a in arr)
                //{
                //}
            }
        }
    }

    static int TestIntVar = 1;
    public static void Test_ReturnLocalRef()
    {
        #pragma warning disable 219
        //#pragma warning disable 
        ref int CreateID()
        {
            int ID = 3;

            // CS8168: Cannot return by reference local "ID", as it's NOT ref variable!
            //return ref ID;

            // OK
            return ref TestIntVar;
        }
        #pragma warning restore 219
    }
};
