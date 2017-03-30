// attached to Manager
// inspired by http://answers.unity3d.com/questions/1080427/unity-521-unet-ui-elements-cant-call-server-comman.html

using UnityEngine;
public class EventManager2 : MonoBehaviour {

	public delegate void ButtonEvent();

	public static event ButtonEvent buttonPressed;

	// called when the UI button is pressed
	public void PushMessage()
    {
			if (buttonPressed != null) buttonPressed ();
    }
}