// This script is designed to establish references for instantiated prefabs because you cannot 
// assign references to prefabs directly. 
// Attach this script to an empty gameobject (maybe called "Manager") which is always available in scene

using UnityEngine;
using UnityEngine.UI;

public class ReferenceManager3 : MonoBehaviour {

	public Text textfield;
    public InputField message;
    public Button button;
	// reference to this script, must be static
	private static ReferenceManager3 refManagerScript;
	void Awake(){
		// reference to this script
		refManagerScript = this;
	}
	
	public static ReferenceManager3 Instance {
		get {
			return refManagerScript; // returns a reference to this script
		}
	}
}

// in order to reference other gameobjects in prefabs (no linking possible), reference these gameobjects
// in this script which is always available in scene. When the prefab needing the reference
// becomes instantiated, the reference to the reference needed. In the Start function. Like this:
// GameObject camera;
// void Start(){
// 		camera = ReferenceManager.Instance.camera;
// }