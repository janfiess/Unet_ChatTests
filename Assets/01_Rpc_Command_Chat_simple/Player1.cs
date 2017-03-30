using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player1 : NetworkBehaviour {
	public Text textfield;

	void Start(){
		textfield = GameObject.Find("Textfield").GetComponent<Text>();
	}
	void Update () {

		// Executed at client
		if(isLocalPlayer &&Input.GetKeyDown("k")){
			CmdInformServer("huhu from client");
		}
	}

	[Command]
	void CmdInformServer(string msgFromClient){
		textfield.text= msgFromClient + "\n" + textfield.text;
		RpcInformClients("huhu from server");
	}

	[ClientRpc]
	void RpcInformClients(string msgFromServer){
		textfield.text= msgFromServer + "\n" + textfield.text;
	}
}
