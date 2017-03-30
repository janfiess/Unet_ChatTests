// attached to Server gameobject

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public enum MessageType
{
    Message = MsgType.Highest + 1,
    Connected = MsgType.Connect,
    Disconnected = MsgType.Disconnect
}
public class ChatServer5 : MonoBehaviour
{
    private string _ip = "127.0.0.1";
    private int _port = 7777;
    private int _maxConnections = 1000;

    [SerializeField]
    private Text _chatText;
	[SerializeField]
    private InputField _sendTextInput;

    // Use this for initialization
    void Start()
    {
        _chatText.text = "";
        _sendTextInput.text = "";
        Application.runInBackground = true;
        RegisterHandlers();
        var config = new ConnectionConfig();
        config.AddChannel(QosType.Reliable);
        config.AddChannel(QosType.Unreliable);

        var ht = new HostTopology(config, _maxConnections);

        if (!NetworkServer.Configure(ht) || !NetworkServer.Listen(_port))
        {
            Debug.LogError("No server created");
            return;

        }
    }

    private void RegisterHandlers()
    {
        NetworkServer.RegisterHandler((short)MessageType.Message, OnMessageReceived);
        NetworkServer.RegisterHandler((short)MessageType.Connected, OnClientConnected);
        NetworkServer.RegisterHandler((short)MessageType.Disconnected, OnClientDisconnected);
    }

    private void OnMessageReceived(NetworkMessage netMes)
    {
        var packet = netMes.ReadMessage<ChatMessage5>();
        AddMessageToChat(packet.Message);
        NetworkServer.SendToAll((short)MessageType.Message, packet);
		
    }

    private void OnClientConnected(NetworkMessage netMes)
    {
        var mes = new ChatMessage5();
        mes.Message = "Player " + netMes.conn.connectionId + " connected.";
        AddMessageToChat(mes.Message);
        NetworkServer.SendToAll((short)MessageType.Message, mes);
    }

    private void OnClientDisconnected(NetworkMessage netMes)
    {
        var mes = new ChatMessage5();
        mes.Message = "Player " + netMes.conn.connectionId + " disconnected.";
        AddMessageToChat(mes.Message);
        NetworkServer.SendToAll((short)MessageType.Message, mes);
    }

    private void AddMessageToChat(string message)
    {
        _chatText.text = message + "\n" + _chatText.text;
    }

    public void SendChatMessage()
    {
        var mes = new ChatMessage5();
        mes.Message = "[Server] " + _sendTextInput.text;
        AddMessageToChat(mes.Message);
        _sendTextInput.text = string.Empty;
        NetworkServer.SendToAll((short)MessageType.Message, mes);
    }



    void OnApplicationQuit()
    {
        NetworkServer.Shutdown();
    }
}
