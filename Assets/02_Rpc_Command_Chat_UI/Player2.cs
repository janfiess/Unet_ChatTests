// attached to player prefab

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player2 : NetworkBehaviour
{
    Text textfield;
    InputField message;
    Button button;

    void Start()
    {
        textfield = GameObject.Find("Textfield").GetComponent<Text>();
        message = GameObject.Find("Message").GetComponent<InputField>();
        button = GameObject.Find("Button").GetComponent<Button>();

        EventManager2.buttonPressed += PushMessageToServer;
    }

    // Executed at this client
    void PushMessageToServer()
    {
        if (!isLocalPlayer) return;
        string messageToServer = message.text;
        CmdInformServer(messageToServer);
    }

    // Executed at server
    [Command]
    void CmdInformServer(string msgFromClient)
    {
        if (!isServer) return;
        RpcInformClients(msgFromClient);
    }

    // Executed at all clients
    [ClientRpc]
    void RpcInformClients(string msgFromServer)
    {
        if (!isClient) return;
        textfield.text = msgFromServer + "\n" + textfield.text;
    }
}
