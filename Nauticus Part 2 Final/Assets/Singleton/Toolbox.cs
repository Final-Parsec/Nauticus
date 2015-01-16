using UnityEngine;
using System.Collections;

public class Toolbox : Singleton<Toolbox> {
	protected Toolbox () {} // guarantee this will be always a singleton only - can't use the constructor!

	public TravelClass travis;
	public string charClass;
	public Inventory inv;

	void Awake () {
		DontDestroyOnLoad (this.gameObject);
		//travis.GetOrAddComponent<TravelClass>();
	}
	
//	// (optional) allow runtime registration of global objects
//	static public TravelClass RegisterComponent<TravelClass> () {
//		return TravelClass;
//		//return Instance.GetOrAddComponent<TravelClass>();
//	}
}
//
//static public class MethodExtensionForMonoBehaviourTransform {
//	/// <summary>
//	/// Gets or add a component. Usage example:
//	/// BoxCollider boxCollider = transform.GetOrAddComponent<BoxCollider>();
//	/// </summary>
//	static public T GetOrAddComponent<T> (this Component child) where T: Component {
//		T result = child.GetComponent<T>();
//		if (result == null) {
//			result = child.gameObject.AddComponent<T>();
//		}
//		return result;
//	}
//}

public class TravelClass : Singleton<TravelClass>{
	public string charClass;

	void Awake(){
		DontDestroyOnLoad (this.gameObject);
		}
}