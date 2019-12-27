using System;
using System.Reflection;
using System.Diagnostics.Contracts;

// Interfaces are ALSO internal by default
// But interface members are always public by default!
public interface IMyObjectBase
{
	#region interface static members 
	static int NextId = 0;
	static int GenerateNextId() => NextId++;
	#endregion interface static members 

	#region interface private methods
	// WARNING!!! Private method are NOT necessarily to be implemented!
	// Private property (like any method - must define implementation)
	private int PrivateId { get => 0; }
	private int PrivateGenerateDefaultId() => 0;
	#endregion // interface private methods

	#region protected properties
	//protected int ProtectedId { get; }
	#endregion // protected properties

	#region Default protected members
	// Default protected members NOT necessarily to be implemented in the derived class
	protected string GetDefaultProtectedImplementorName() => this.GetType().ToString();
	#endregion

	#region Default public methods
	void WritePrivateId()
	{
		Console.WriteLine($"PrivateId={PrivateId}");
	}

	// How to call interface method in default implementation?
	void Default_CallInterfaceMethod()
	{
		// PUBLIC: Just call - and it's ok!
		ShowSender();

		// Protected: Just call - and it's ok!
		ProtectedShowSender();

		// Private: Just call - and it's ok!
		int code = PrivateGenerateDefaultId();
	}
	string GetDefaultImplementorName() => nameof(IMyObjectBase);
	#endregion Default public methods

	#region public methods
	void ShowSender();
	void ShowSenderExplicit();
	#endregion // public methods

	#region protected methods
	protected void ProtectedShowSender();
	#endregion protected methods
};

public interface IMySceneObject : IMyObjectBase
{
	// How to call interface method in default implementation?
	void Default_CallInterfaceMethod_InDerivedInterface()
	{
		// PUBLIC: Just call - and it's ok!
		ShowSender();

		// Protected: Just call - and it's ok!
		ProtectedShowSender();

		// Private: Cannot call (due to access specifiers)
		//int code = PrivateGenerateDefaultId();
	}

	#region Default public methods	

	#region interface private methods
	// WARNING!!! Private method are NOT necessarily to be implemented!
	// Private property (like any method - must define implementation)
	private int PrivateId { get => 0; }
	#endregion
	
	// The RIGHT way to override default implementation when deriving interface from other interface
	string IMyObjectBase.GetDefaultImplementorName() => nameof(IMySceneObject);

	// WARNING!!! If we hide THIS way, then this implementation is really a DIFFERENT method
	// (for example, for IMyObjectBase it will call the base's default implementation,
	// but for IMySceneObject - will use this implementation)!!!
	//new string GetDefaultImplementorName() => nameof(IMySceneObject);
	#endregion
};

class MySceneObject : IMySceneObject
{
	// WARNING!!! When we define properties, we can NOT add access specifiers
	// (ever in interfaces!!! )

	#region interface private methods
	// WARNING!!! Private method are NOT necessarily to be implemented, but CAN be!!
	// Private property (like any method - must define implementation)

	// will NOT override
	//long PrivateId
	//will NOT override
	//int PrivateId
	//int IMyObjectBase.PrivateId // Error
	//int IMyObjectBase.PrivateId
	// OUTCOME: We can NOT overrie Private methods at ALL!
	int PrivateId
	{
		get
		{
			Console.WriteLine($"Using private get method in {this.GetType()}");
			return 5;
		}
		set 
		{ 
			Console.WriteLine($"Using private set method {this.GetType()}"); 
		} 
	}
	//private int PrivateGenerateDefaultId() => 0;
	#endregion // interface private methods


	#region Default protected members
	// Default protected members NOT necessarily to be implemented by the implementing class!
	//protected string GetDefaultProtectedImplementorName() => this.GetType().ToString();
	#endregion

	#region Default public members override
	// Default public method NOT necessatily to be implemented by the implementing class

	// OVERRIDES default method implementation of the interface (ever if NOT explicit definition syntax used!)
	public string GetDefaultImplementorName() => nameof(MySceneObject);
	#endregion Default public members override

	#region public methods
	public void ShowSender()
	{
		Console.WriteLine($"ShowSender: sender={nameof(MySceneObject)}");
	}
	void IMyObjectBase.ShowSenderExplicit()
	{
		Console.WriteLine($"ShowSender explicit: sender={nameof(MySceneObject)}");
	}
	#endregion // public methods

	#region protected members
	// NOT implementation!
	//void ProtectedShowSender()
	// NOT implementation!
	//protected void ProtectedShowSender()
	void IMyObjectBase.ProtectedShowSender()
	{
		// Q? How to get nameof(IMyObjectBase.ProtectedShowSender) to work?
		Console.WriteLine($"nameof(IMyObjectBase.ProtectedShowSender), DeclaringType={MethodBase.GetCurrentMethod().DeclaringType}");
	}

	void CallExplicitInterfaceMethods_InClass()
	{
		// Public methods!
		{
			// method does NOT exist in this context!
			//ShowSenderExplicit();

			// You need a reference to object!
			//IMyObjectBase.ShowSenderExplicit();

			// Ok
			((IMyObjectBase)this).ShowSenderExplicit();
		}

		// Protected methods
		{
			// Protected show sender does NOT exist!!!!
			//ProtectedShowSender();

			// IMyObjectBase.ProtectedShowSender does NOT exist
			//IMyObjectBase.ProtectedShowSender();

			// Unable to call!
			//((IMyObjectBase)this).ProtectedShowSender();
		}
	}
	#endregion // protected members
}

public static class InterfaceTests
{
	public static void DoAllTests()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);
		DoSimpleTest();
	}

	public static void DoSimpleTest()
	{
		Console.WriteLine(System.Reflection.MethodBase.GetCurrentMethod().Name);

		var obj = new MySceneObject();
		IMySceneObject i_scene_obj = obj;
		IMyObjectBase i_obj_base = obj;

		// Access private
		{
			// Wrong to access by object
			//int id = obj.PrivateId;

			// Wrong to access by scene object
			//int id = i_scene_obj.PrivateId;

			// Wrong to access by object base
			//int id = i_obj_base.PrivateId;
		}

		// Call default-implemented public method
		{
			i_scene_obj.WritePrivateId();
			i_obj_base.WritePrivateId();
		}

		// Call interface method explicitly
		{
			// Unable to call explicit method through object reference: compile time error !
			//obj.ShowSenderExplicit();
			i_obj_base.ShowSenderExplicit();
			i_scene_obj.ShowSenderExplicit();
		}

		// Call protected method
		{
			// WARNING!!! Unable to access the protected method directly!!!
			//obj.ProtectedShowSender();
			//i_obj_base.ProtectedShowSender();
			//i_scene_obj.ProtectedShowSender();
		}

		// Call default-implemented public member
		{
			Console.WriteLine($"obj.GetDefaultImplementorName()={obj.GetDefaultImplementorName()}");
			Console.WriteLine($"i_scene_obj.GetDefaultImplementorName()={i_scene_obj.GetDefaultImplementorName()}");
			Console.WriteLine($"i_obj_base.GetDefaultImplementorName()={i_obj_base.GetDefaultImplementorName()}");
		}
		
	}
}
