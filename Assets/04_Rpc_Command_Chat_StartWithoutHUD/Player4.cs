// attached to player prefab

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Player4 : NetworkBehaviour
{
    Text textfield;
    InputField message;
    Button button;

    void Start()
    {
        // assigned references to the player prefab using the ReferenceManager (attached to the Manager gameobject which is always available in scene)
        textfield = ReferenceManager4.Instance.textfield;  
        message = ReferenceManager4.Instance.message; 
        button = ReferenceManager4.Instance.button;       

        EventManager4.buttonPressed += PushMessageToServer;
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
