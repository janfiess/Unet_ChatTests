/**

// attached to the player prefab
// This script adds authority for the local player to change the position of a gameobject over Network. 
// Making an item draggable is done by DraggableXZ.cs. All draggable items have the Tag "Item"
// Each draggable object requires a NetworkIdentity script, a networkTransform component and a Rigidbody attached 
// Without this script the items' position would only be synchronized if the position was changed by the client on whose Unity 
// instance also runs the server.
// In this example: Mouse down -> add local player authority, mouse down -> remove local player authority
// inspired by http://stackoverflow.com/questions/33469930/how-do-i-sync-non-player-gameobject-properties-in-unet-unity5
// and http://answers.unity3d.com/answers/1283526/view.html

*/
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAuthority : NetworkBehaviour {
    private Transform camTransform;
	GameObject hitGameObject;
	
	void Start(){
		// in case there is only one camera in the scene: camTransform = Camera.main; // ReferenceManager.cs is not needed then.
		camTransform = ReferenceManager.Instance.camera.transform;
	}

    void Update (){
		
		// set local player authority on mouse down
		if (isLocalPlayer && Input.GetMouseButtonDown(0)){
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out hit)){
				hitGameObject = hit.collider.gameObject;
				if(hitGameObject.tag == "DraggableItem"){
					CmdAddLocalAuthority(hitGameObject);
				}
			}
		}

		// remove local player authority on mouse up
		if (isLocalPlayer && Input.GetMouseButtonUp(0)){
			if(hitGameObject.tag == "DraggableItem"){
				CmdRemoveLocalPlayerAuthority(hitGameObject);
			}	
		}
    }

 	[Command]
     void CmdAddLocalAuthority (GameObject obj) {
         NetworkInstanceId nIns = obj.GetComponent<NetworkIdentity> ().netId;
         GameObject client = NetworkServer.FindLocalObject (nIns);
         NetworkIdentity ni = client.GetComponent<NetworkIdentity> ();
         ni.AssignClientAuthority(connectionToClient);
     }

	 [Command]
	 void CmdRemoveLocalPlayerAuthority(GameObject obj){
		 NetworkInstanceId nIns = obj.GetComponent<NetworkIdentity> ().netId;
         GameObject client = NetworkServer.FindLocalObject (nIns);
         NetworkIdentity ni = client.GetComponent<NetworkIdentity> ();
         ni.RemoveClientAuthority (ni.clientAuthorityOwner);
	 }
}