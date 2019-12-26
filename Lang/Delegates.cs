using System;
using System.Diagnostics.Contracts;

// Note, we always derive arguments from EventArgs,
// and EventArgs is class
public class MyCustomEventArgs : EventArgs {};
public class MyClickEventArgs : EventArgs {};
public class MyMathOpEventArgs : EventArgs {};

public delegate void MyClickDelegate(object sender, MyClickEventArgs e);
public delegate int MyMathOpDelegate(object sender, MyMathOpEventArgs e);

public delegate int Other_MyMathOpDelegate(object sender, MyMathOpEventArgs e);

public interface IDelegateTestsHandler
{
    // By default, interface methods are public, and access modifiers are NOT allowed, until C# 8.0!
    void OnMyClickInterface(object Sender, MyClickEventArgs e);

    // Event in the interface
    event MyClickDelegate MyClickEventInterface;
};

public class DelegateTestsHandler : IDelegateTestsHandler
{
    public int Id { get; } = GenerateId();

    public int MyMathOp(object sender, MyMathOpEventArgs e) => 4;

    public void OnMyEvent(object sender, EventArgs e)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"ThisId={Id} Sender={sender}, e={e}");
    }

    public void OnMyClick(object sender, MyClickEventArgs e)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"ThisId={Id} Sender={sender}, e={e}");
    }
    public virtual void OnMyClickVirt(object sender, MyClickEventArgs e)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"ThisId={Id} Sender={sender}, e={e}");
    }

    #region IDelegateTestsHandler
    public void OnMyClickInterface(object sender, MyClickEventArgs e)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"ThisId={Id} Sender={sender}, e={e}");
    }
    #endregion // IDelegateTestsHandler


    #region interface event
    // WARNING!!! It's sufficient to implement the interface event: 
    // Just to redefine it in the implementing class with public
    public event MyClickDelegate MyClickEventInterface;
    #endregion // interface event

    #region events
    public void InvokeClickEvent()
    {
        // WARNING! We also must check before invoking - whether event is null
        //MyClickEvent(this, new MyClickEventArgs());
        _MyClickEvent?.Invoke(this, new MyClickEventArgs());

        // We DO CAN call interface event also
        MyClickEventInterface?.Invoke(this, new MyClickEventArgs());
    }
    public void ResetClickEvent()
    {
        // WARNING!!! When we defined add/remove in event, we can NOT longer use the public event itself,
        // only internal, because the public event now only allows add/remove!
        _MyClickEvent = null;
    }
    
    public event MyClickDelegate MyClickEvent
    {
        // WARNING 1!!! this access methods add/remove: not necessary in most cases
        // WARNING 2!!! When we defined add/remove in event, we can NOT longer use the public event itself,
        // only internal, because the public event now only allows add/remove!
        add => _MyClickEvent += value;
        remove => _MyClickEvent -= value;
    }
    //  WARNING! When we use add/remove, we must additionally define INTERNAL event;
    event MyClickDelegate _MyClickEvent; // WARNING!!! Initialization is not necessary +=, -= can be used on null event

    protected event MyClickDelegate MySomeEvent;
    protected virtual event MyClickDelegate MySomeEventVirt;
    #endregion // events

    #region id 
    static int NextId = 0;
	static int GenerateId() => NextId++;
	#endregion // id
};

public class DerivedDelegateTestsHandler : DelegateTestsHandler, IDelegateTestsHandler
{
    public override void OnMyClickVirt(object sender, MyClickEventArgs e)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"ThisId={Id} Sender={sender}, e={e}");
        Console.WriteLine($"Method called from {nameof(DerivedDelegateTestsHandler)} !!!");
    }

    // Trying to call event of the base class from the derived
    public void CallEventFromDervied()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // WARNING!!! It's ALSO impossible to call event from the DERIVED class!!!
        // (CS0070)
        //MySomeEvent(this, new MyClickEventArgs());
        //MySomeEventVirt(this, new MyClickEventArgs());

        // Also it's impossible to call interface event from DERIVED class
        //MyClickEventInterface?.Invoke(this, new MyClickEventArgs());
    }

    #region IDelegateTestsHandler
    // WARNING!!! The only way to overload already implemented method of the interface class is 
    // 1. to make the class itself derive from interface
    // 2. implement method exeplicitly! (interface_name.method_name)
    void IDelegateTestsHandler.OnMyClickInterface(object sender, MyClickEventArgs e)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"ThisId={Id} Sender={sender}, EventArgs={e}");
        Console.WriteLine($"Method called from {nameof(DerivedDelegateTestsHandler)} !!!");
    }

    // (Cs0065): WARNING!!! When explicitly definining interface event,
    // we must define add/remove methods manually
    event MyClickDelegate IDelegateTestsHandler.MyClickEventInterface
    {
        add
        {
            Console.WriteLine($"From {this.GetType().Name}");
            _MyClickEventInterface += value;
        }
        remove
        {
            Console.WriteLine($"From {this.GetType().Name}");
            _MyClickEventInterface -= value;
        }
    }
    event MyClickDelegate _MyClickEventInterface;
    #endregion // IDelegateTestsHandler
}
public struct MyStructWithFuncs
{
    public void MyFunc(object sender, EventArgs e)
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        Console.WriteLine($"sender={sender}, e={e}");
    }
};

public class DelegateTests
{
    #region testee helpers
    DelegateTestsHandler HandlerObject = new DelegateTestsHandler();
    DelegateTestsHandler HandlerObjectTwo = new DelegateTestsHandler();
    // WARNING: Important for test: compile-time type must be base, not derived!
    DelegateTestsHandler HandlerDerivedObject = new DerivedDelegateTestsHandler();
    IDelegateTestsHandler MyDerivedInterface = new DerivedDelegateTestsHandler();
    #endregion // testee helpers

    #region utils
    public static void WriteSeps() => Console.WriteLine("----------------------------------------");
    #endregion // utils


    public EventHandler OnMyEvent;
    public EventHandler<MyCustomEventArgs> OnMyCustomEvent;
    public MyClickDelegate OnMyClick;

    public MyMathOpDelegate OnMyMathOp;
    public MyMathOpDelegate OnMyMathOp_Other;
    public Other_MyMathOpDelegate OnMyMathOp_OtherDelegate;

    public static void DoAllDelegateTests()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        var DelTests = new DelegateTests();
        DelTests.TestGenericDelegate();
        DelTests.TestPredicateRefParams();
        DelTests.TestDelegateInfo();
        DelTests.TestUnsubscribeFromDelegate();
        DelTests.TestVoidDelegate();
        DelTests.TestIntDelegate();
        DelTests.DoEventTests();
        // @TODO
    }

    public delegate bool MyPredicateOnTypeDelegate<T>(T Val);
    public MyPredicateOnTypeDelegate<int> PredicateFromInt;
    public MyPredicateOnTypeDelegate<float> PredicateFromFloat;

    public delegate T MyOnTypeDelegate<T>(object sender, T Val);
    
    public void TestGenericDelegate()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        PredicateFromInt = x => (x % 2) == 0;
        PredicateFromFloat = x => (x % 2) == 0;
    }

    public delegate bool MyIntParserDelegate(string S, out int Value);
    MyIntParserDelegate IntParser;
    public delegate bool MyParserDelegate<T>(string S, out T Value);

    public void TestPredicateRefParams()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        IntParser = Int32.TryParse;
        int ParsedValue = 0;
        const int InputValue = 23;
        bool bParsed = IntParser.Invoke(InputValue.ToString(), out ParsedValue);
        Contract.Assert(bParsed && ParsedValue == InputValue);
    }

    public void TestVoidDelegate()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // Can we bind to delegate variable, that is NOT initialized with new()?
        // YES!!!
        //WriteSeps();
        //{
        //    OnMyClick += OnMyClickHandler;
        //}        

        // @TODO: Try to check, whether delegate is binded?

        // Invoke uninitialized delegate: will fail!
        //  System.NullReferenceException
        //WriteSeps();
        //{
        //	OnMyClick(this, new MyClickEventArgs());
        //	//OnMyClick.Invoke(this, new MyClickEventArgs());
        //}

        // Invoke delegate with a single binded static function      
        WriteSeps();
        {
            Contract.Assert(OnMyClick == null);
            OnMyClick += OnMyClickHandler;
            Contract.Assert(OnMyClick.GetInvocationList().Length > 0);
            OnMyClick(this, new MyClickEventArgs());
        }

        // Try to bind the same static function to delegate a second time
        // WILL call THE SAME function TWICE!!!
        // Q. How to check whether a given function is already bound to delegate?
        WriteSeps();
        {
            OnMyClick += OnMyClickHandler;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Invoke delegate with multiple binded static functions
        // (will work as expected)
        //WriteSeps();
        //{
        //	OnMyClick += OnMyClickHandler_2;
        //	OnMyClick(this, new MyClickEventArgs());
        //}

        // Bind member function to delegate
        WriteSeps();
        {
            OnMyClick += HandlerObject.OnMyClick;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Try to bind function for the same object a second time
        // WILL call THE SAME function TWICE!!!
        // Q. How to check whether a given function is already bound to delegate?
        WriteSeps();
        {
            OnMyClick += HandlerObject.OnMyClick;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Bind a second member function to delegate
        WriteSeps();
        {
            OnMyClick += HandlerObjectTwo.OnMyClick;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Bind virtual function to delegate
        WriteSeps();
        {
            OnMyClick += HandlerObject.OnMyClickVirt;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Bind overriden virtual function to delegate
        WriteSeps();
        {
            OnMyClick += HandlerDerivedObject.OnMyClickVirt;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Bind interface function
        WriteSeps();
        {
            OnMyClick += HandlerObject.OnMyClickInterface;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Bind explicit interface function
        WriteSeps();
        {
            OnMyClick += MyDerivedInterface.OnMyClickInterface;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Bind local function to delegate
        // Works WELL!
        WriteSeps();
        {
            // Test that local function captures;
            float x = 0;
            void OnMyClickLocal(object Sender, MyClickEventArgs e)
            {
                x = 1;
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
                Console.WriteLine($"Sender={Sender}, e={e}");
            }
            OnMyClick += OnMyClickLocal;
            OnMyClick(this, new MyClickEventArgs());
        }

        // Bind lambda function to delegate
        // WORKS WELL
        WriteSeps();
        {
            // Test that lambda captures
            float x = 0;
            OnMyClick += (object Sender, MyClickEventArgs e) =>
            {
                x = 1;
                Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
                Console.WriteLine($"Sender={Sender}, e={e}");
            };
            OnMyClick(this, new MyClickEventArgs());
        }

        // Binds structure function to delegate
        WriteSeps();
        {
            MyStructWithFuncs s;
            OnMyClick += s.MyFunc;
            OnMyClick(this, new MyClickEventArgs());
        }

        // @TODO: testing type-casting in delegates

        // Unsubscribe from the delegate

        // Unsubscribe from all functions
    }

    public void TestDelegateInfo()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        OnMyMathOp += Op1;

        // WARNING 1!!! .Method and .Target - are computed from the last binded value!!
        // WARNING 2!!! .Target is nullptr for static function!

        WriteSeps();
        {
            Contract.Assert(OnMyMathOp != null, "we must only get info about delegate's method if it's NOT null");
            // OnMyMathOp.Method.ToString() Will return: entire signature with name, return type, params
            Console.WriteLine($"OnMyMathOp.Method.ToString()={OnMyMathOp.Method.ToString()}");
        }

        // When we have one static and one non-static method binded
        WriteSeps();
        {

            Contract.Assert(OnMyMathOp != null, "we must only get info about delegate's method if it's NOT null");
            OnMyMathOp += HandlerObject.MyMathOp;

            if (OnMyMathOp != null && OnMyMathOp.Target != null)
            {
                Contract.Assert(OnMyMathOp != null && OnMyMathOp.Target != null, "we must only get info about delegate's target if it's NOT null");
                Console.WriteLine($"OnMyMathOp.Method.ToString()={OnMyMathOp.Method.ToString()}");
                Console.WriteLine($"OnMyMathOp.Target.ToString()={OnMyMathOp.Target.ToString()}");
            }    
        }

        // When we have one static and two non-static methods for different objects
        WriteSeps();
        {
            OnMyMathOp += HandlerObjectTwo.MyMathOp;
            Console.WriteLine($"OnMyMathOp.Method.ToString()={OnMyMathOp.Method.ToString()}");
            Console.WriteLine($"OnMyMathOp.Target.ToString()={OnMyMathOp.Target.ToString()}");
        }

        OnMyMathOp = null;
    }
    public void TestUnsubscribeFromDelegate()
    {
        Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        void LocalHandler(object sender, EventArgs e)
        {
            Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
        }

        OnMyEvent += LocalHandler;
        OnMyEvent += LocalHandler;

        // Here we removed ONLY the LAST subscriber from the end
        WriteSeps();
        {
            OnMyEvent -= LocalHandler;
            OnMyEvent(this, EventArgs.Empty);
        }


        // Here we removed a new subscriber

        WriteSeps();
        {
            OnMyEvent -= LocalHandler;
            // WARNING: here ?. is required because delegate DO can be null!!!
            OnMyEvent?.Invoke(this, EventArgs.Empty);
        }

        // What is we remove subscribed that is not subscribed?
        WriteSeps();
        {
            // Unsubscribing when NOT subscribed - ok
            OnMyEvent -= LocalHandler;
            // WARNING: here ?. is required because delegate DO can be null!!!
            OnMyEvent?.Invoke(this, EventArgs.Empty);
        }

        // Unsubscribe single object from delegate
        WriteSeps();
        {
            OnMyEvent += HandlerObject.OnMyEvent;
            OnMyEvent += HandlerObject.OnMyEvent;
            OnMyEvent += HandlerObjectTwo.OnMyEvent;

            OnMyEvent -= HandlerObject.OnMyEvent;
            OnMyEvent.Invoke(this, EventArgs.Empty);
        }

        // Unsubscribe first object second object from delegate
        WriteSeps();
        {
            OnMyEvent -= HandlerObject.OnMyEvent;
            OnMyEvent.Invoke(this, EventArgs.Empty);
        }

        // Unsubscribe second object from delegate
        WriteSeps();
        {
            OnMyEvent -= HandlerObjectTwo.OnMyEvent;
            OnMyEvent?.Invoke(this, EventArgs.Empty);
        }


        OnMyEvent += HandlerObject.OnMyEvent;
        OnMyEvent += HandlerObject.OnMyEvent;
        OnMyEvent += HandlerObjectTwo.OnMyEvent;
        // Test unsubscribing ALL for the given object
        //WriteSeps();
        //{
        //    OnMyEvent.(OnMyEvent, HandlerObject.OnMyEvent);
        //   OnMyEvent.Invoke(this, EventArgs.Empty);
        //}
    }

    public static int Op1 (object sender, MyMathOpEventArgs e) => 3;
    public static int Op2 (object sender, MyMathOpEventArgs e) => 6;

    public void TestIntDelegate()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // OK
        Contract.Assert(OnMyMathOp == null);
        Contract.Assert(OnMyMathOp == OnMyMathOp_Other);

        // Trying to bind static function with value;
        {
            OnMyMathOp += Op1;
            OnMyMathOp += Op2;

            // The last binded will be result!
            int result = OnMyMathOp.Invoke(this, new MyMathOpEventArgs());
            Console.WriteLine($"Result is {result}");

            // The right way to completely clear the delegate invocation list
            OnMyMathOp = null;
        }

        // Delegate conversion:
        // NOTE: Func<Arg1, RetType> - is a delegate
        Func<object, MyMathOpEventArgs, int> func = (object sender, MyMathOpEventArgs e) => 3;
        {
            // @. How to convert delegates?
            //OnMyMathOp = func; // WRONG
            OnMyMathOp = new MyMathOpDelegate(func); // OK

            OnMyMathOp = null;
        }

        // Compare delegates with the same signature
        {
            // Different delegates are ONLY the same by operator== if both null!
            Contract.Assert(OnMyMathOp == OnMyMathOp_Other);

            OnMyMathOp += Op1;
            OnMyMathOp += Op2;

            OnMyMathOp_Other += Op2;
            OnMyMathOp_Other += Op1;

            // WARNING: EVER delegates with THE SAME type are differnt with operator==!!!
            Contract.Assert( OnMyMathOp != OnMyMathOp_Other);

            // But equals checks equality correctly!
            Contract.Equals( OnMyMathOp, OnMyMathOp_Other );

            OnMyMathOp = OnMyMathOp_Other = null;            
        }

        {
            // CS0019: operator== cannot be done to OnMyMathOp, OnMyMathOp_Other
            //OnMyMathOp == OnMyMathOp_OtherDelegate;

            // WARNING!!! Should NOT be null (Equals)!
            //Contract.Assert(OnMyMathOp.Equals(OnMyMathOp_OtherDelegate));

            OnMyMathOp = new MyMathOpDelegate(func);
            Contract.Assert(!OnMyMathOp.Equals(OnMyMathOp_OtherDelegate));

            // WARNING! They are STILL DIFFERENT!!!
            OnMyMathOp_OtherDelegate = new Other_MyMathOpDelegate(func);
            Contract.Assert( ! OnMyMathOp.Equals(OnMyMathOp_OtherDelegate));
        }

        // Comparing delegates of different types but with the same signature,
        {
            OnMyMathOp = Op1;
            OnMyMathOp_OtherDelegate = Op1;
            // WARNING!!! They're still DIFFERENT
            Contract.Assert( ! OnMyMathOp.Equals(OnMyMathOp_OtherDelegate) );
            // Fails to compile
            //Contract.Assert(OnMyMathOp == OnMyMathOp_OtherDelegate);

            // Fails to compile: unable to cast different delegates
            //Contract.Assert(OnMyMathOp == (OnMyMathOp)OnMyMathOp_OtherDelegate);

            // WARNING!!! They're still DIFFERENT
            Contract.Assert(OnMyMathOp != new MyMathOpDelegate(OnMyMathOp_OtherDelegate));

            OnMyMathOp = null;
            OnMyMathOp_OtherDelegate = null;
        }

        // Comparing delegates of different types but with the same signature,
        // pointing to different delegates
        {
            // CS0019: operator== cannot be done to OnMyMathOp, OnMyMathOp_Other
            //OnMyMathOp == OnMyMathOp_OtherDelegate;

            // WARNING!!! Should NOT be null (Equals)!
            //Contract.Assert(OnMyMathOp.Equals(OnMyMathOp_OtherDelegate));
            
            OnMyMathOp = new MyMathOpDelegate(func);
            Contract.Assert( ! OnMyMathOp.Equals(OnMyMathOp_OtherDelegate) );

            // WARNING! They are STILL DIFFERENT!!!
            OnMyMathOp_OtherDelegate = new Other_MyMathOpDelegate(func);
            Contract.Assert( ! OnMyMathOp.Equals(OnMyMathOp_OtherDelegate) );
        }
    }

	public void DoEventTests()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

        // Forbidden calls on the client site of the event (CS0070)
        // WARNING!! ON THE CLIENT SIDE we ONLY can call += and -= on events!!!
        {
            //HandlerObject.MyClickEvent = null;
            //HandlerObject.MyClickEvent = OnMyClickHandler;
            //HandlerObject.MyClickEvent(this, new MyClickEventArgs());
            //bool bEventIsNull = HandlerObject.MyClickEvent == null;
        }

        // Try to subscribe and unsubscribe from event
        WriteSeps();
        {
            HandlerObject.MyClickEvent += OnMyClickHandler;
            HandlerObject.MyClickEvent += OnMyClickHandler_2;
            HandlerObject.MyClickEvent += OnMyClickHandler;
            HandlerObject.InvokeClickEvent();
            HandlerObject.MyClickEvent -= OnMyClickHandler;
            HandlerObject.MyClickEvent -= OnMyClickHandler_2;
            HandlerObject.MyClickEvent -= OnMyClickHandler;
            // Here event is null, but we still can do the unsubscribe call
            {
                // WRONG: ever check call is wrong for events!!!
                //Contract.Assert(HandlerObject.MyClickEvent == null);
                HandlerObject.MyClickEvent -= OnMyClickHandler;
            }
            HandlerObject.InvokeClickEvent();
        }
	}

	public static void OnMyClickHandler(object Sender, MyClickEventArgs e)
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		Console.WriteLine($"Sender={Sender}, e={e}");
	}

	public static void OnMyClickHandler_2(object Sender, MyClickEventArgs e)	
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		Console.WriteLine($"Sender={Sender}, e={e}");
	}
};
