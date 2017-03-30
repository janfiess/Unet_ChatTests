// attached to client game object

using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ChatClient5 : MonoBehaviour
{

    private string _ip = "127.0.0.1";
    private int _port = 7777;
    private NetworkClient _client;

    [SerializeField]
    private Text _chatText;
    [SerializeField]
    private InputField _sendTextInput;
    [SerializeField]
    private InputField _nameInput;
    void Start()
    {
        _chatText.text = string.Empty;
        _sendTextInput.text = string.Empty;
        _nameInput.text = "Client " + Random.Range(0, int.MaxValue);

        Application.runInBackground = true;

        var config = new ConnectionConfig();
        config.AddChannel(QosType.Reliable);
        config.AddChannel(QosType.Unreliable);

        _client = new NetworkClient();
        _client.Configure(config, 1);

        RegisterHandlers();
        _client.Connect(_ip, _port);
    }

    private void RegisterHandlers()
    {
        _client.RegisterHandler((short)MessageType.Message, OnMessageReceived);
    }

    private void OnMessageReceived(NetworkMessage message)
    {
        var mes = message.ReadMessage<ChatMessage5>().Message;
        _chatText.text = mes + "\n" + _chatText.text;
    }

    public void SendChatMessage()
    {
        var mes = new ChatMessage5();
        mes.Message = _nameInput.text + ": " + _sendTextInput.text;
        _sendTextInput.text = "";
        _client.Send((short)MessageType.Message, mes);
    }

    void OnApplicationQuit()
    {
        if (_client != null)
        {
            _client.Disconnect();
            _client.Shutdown();
            _client = null;
        }
    }
}
