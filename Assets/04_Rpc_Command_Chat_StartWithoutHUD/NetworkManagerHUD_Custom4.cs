// script attached to NetworkManager gameobject
// inspired by http://wontonst.blogspot.de/2015/07/making-custom-networkmanagerhud-in.html

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NetworkManagerHUD_Custom4 : MonoBehaviour {

	public Button btn_host;
	public Button btn_client;
	NetworkManager networkManager;
	void Start(){
		networkManager = GetComponent<NetworkManager>();
	}
	public void StartHost(){
		print("start host");
		networkManager.StartHost();
		Destroy(btn_host.gameObject);
		Destroy(btn_client.gameObject);
	}

	public void StartClient(){
		print("start client");
		networkManager.StartClient();
		Destroy(btn_host.gameObject);
		Destroy(btn_client.gameObject);
	}
}